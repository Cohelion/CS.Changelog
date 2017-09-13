using CommandLine;
using CS.Changelog.Exporters;
using CS.Changelog.Utils;
using System;
using System.Text.RegularExpressions;

namespace CS.Changelog.Console
{
	class Program
	{
		internal static readonly Options _options = new Options();

		static void Main(string[] args)
		{
			if (!Parser.Default.ParseArguments(args, _options))
				return;

			_options.Dump(LogLevel.Info);

			var firstrun = true;

			while (firstrun
				|| (!System.Console.IsInputRedirected && System.Console.ReadKey().Key != ConsoleKey.X))
			{
				firstrun = false;

				var log = GitExtensions.GetHistory(_options.RepositoryLocation, _options.PathToGit);

				$"Raw log : {log}".Dump(loglevel: LogLevel.Debug);

				var parseOptions = new ParseOptions()
				{
					prefix_feature = _options.Prefix_feature,
					prefix_hotfix = _options.Prefix_hotfix,
					prefix_release = _options.Prefix_release,

					branch_development = _options.Branch_development,
					branch_master = _options.Branch_master,
					branch_preview = _options.Branch_preview,

					category_feature = _options.Category_feature,
					category_hotfix = _options.Category_hotfix,
				};

				var entries = Parsing.Parse(log, parseOptions);
				entries.Name = _options.ReleaseName;

				//
				var exportOptions = new ExportOptions()
				{
					Append = _options.Append,
					Reverse = true,
					ResolveIssueNumbers = _options.LinkifyIssueNumbers,
					IssueTrackerUrl = _options.IssueTrackerUrl,
					RepositoryUrl = _options.CommitDetailsUrl,
					LinkHash = !string.IsNullOrWhiteSpace(_options.CommitDetailsUrl),
					ShortHash = true,
					IssueNumberRegex = new Regex(_options.IssueNumberRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled)
				};

				//Always Output to console for now
				entries.Export(OutputFormat.Console, _options.TargetFile, exportOptions);

				System.Console.WriteLine();
				var file = entries.Export(_options.OutputFormat, _options.TargetFile, exportOptions);

				if (!System.Console.IsInputRedirected)
				{
					if (file.Exists)
					{
						$"Opening file: {file.FullName}".Dump();
						System.Diagnostics.Process.Start(file.FullName);
					}
					else
						$"Changelog does not exist at: {file.FullName}".Dump();

					"Press 'X' to exit (and anything else to run again)".Dump();
				}
			}
		}
	}
}