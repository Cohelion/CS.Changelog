﻿using CS.Changelog.Utils;
using System.Diagnostics;
using System.Text;

namespace CS.Changelog
{
	/// <summary>
	/// Extensions for obtaining the history from Git.
	/// </summary>
	public static class GitExtensions
	{
		/// <summary>
		/// Gets the history as a string that can be parsed using <see cref="Parsing.Parse(string, ParseOptions)" />.
		/// </summary>
		/// <param name="workingDirectory">The working directory, should be the git repository directory.</param>
		/// <param name="pathToGit">The path to git, defaults to 'git', which should suffice. Usually git is a PATH variable.</param>
		/// <param name="incremental">if set to <c>true</c> obtains changes since the last release.</param>
		/// <returns>The log message in custom prettyprint format</returns>
		/// <exception cref="System.Exception">An error occurred whele reading the log.</exception>
		public static string GetHistory(
			string workingDirectory,
			string pathToGit = "git",
			bool incremental = true)
		{

			const string gitGetStartArgument = @"describe --tags --abbrev=0";

			$@"Getting history from git repository at working directory: {workingDirectory}
Using git path : {pathToGit}
Obtaining latest release using : {gitGetStartArgument}"
				.Dump(LogLevel.Debug);

			var psi = new ProcessStartInfo(pathToGit)
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				WorkingDirectory = workingDirectory,
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden
			};


			string start = string.Empty;

			if (incremental)
			{
				psi.Arguments = gitGetStartArgument;
				using (var p = Process.Start(psi))
				{
					p.WaitForExit();

					start = p.StandardOutput.ReadToEnd().Trim();

					$"Output : {start}".Dump(LogLevel.Debug);

					if (p.ExitCode != 0)
					{
						string errorMessage = p.StandardError.ReadToEnd();

						switch (errorMessage.Trim())
						{
							case "fatal: No names found, cannot describe anything.":
								break;
							default:
								throw new System.Exception($"Error while obtaining the previous release name : {errorMessage}");
						}
					}
				}

			(string.IsNullOrWhiteSpace(start)
				? $"Reading the entire history: no previous tagged release was found."
				: $"Reading history since tag : '{start}'"
				).Dump();
			}
			else {
				$"Reading entire history".Dump();
			}

			//Switches:
			//H  = full hash
			//cI = committer date, strict ISO 8601 format
			//B  = raw body (unwrapped subject and body)
			const string formatarguments = "%H '%cI' %B";
			var gitGetChangelogArgument = string.IsNullOrWhiteSpace(start)
				? $@"log               --pretty=format:""{formatarguments}"""
				: $@"log {start}..HEAD --pretty=format:""{formatarguments}""";

			var result = new StringBuilder();

			psi.Arguments = gitGetChangelogArgument;
			using (var p = Process.Start(psi))
			{
				while (!p.StandardOutput.EndOfStream || !p.HasExited)
				{
					var line = p.StandardOutput.ReadLine();
					if (!string.IsNullOrWhiteSpace(line))
						result.AppendLine(line);
				}

				p.WaitForExit();

				if (p.ExitCode != 0)
					throw new System.Exception(p.StandardError.ReadToEnd());
			}

			return result.ToString();
		}
	}
}