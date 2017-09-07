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
			_options.GetUsage().Dump();

			if (!Parser.Default.ParseArguments(args, _options))
			{
				System.Console.ReadKey();
				return;
			}

			_options.Dump(LogLevel.Debug);

			var firstrun = true;

			while (firstrun
				|| !System.Console.IsInputRedirected && System.Console.ReadKey().Key != ConsoleKey.X)
			{
				firstrun = false;

				var log = GitExtensions.GetHistory(_options.RepositoryLocation, _options.PathToGit);

				$"Raw log : {log}".Dump(loglevel: LogLevel.Debug);

				var parseOptions = new ParseOptions()
				{
					prefix_feature = _options.prefix_feature,
					prefix_hotfix = _options.prefix_hotfix,
					prefix_release = _options.prefix_release,

					branch_development = _options.branch_development,
					branch_master = _options.branch_master,
					branch_preview = _options.branch_preview,

					category_feature = _options.category_feature,
					category_hotfix = _options.category_hotfix,
				};

				var entries = Parsing.Parse(log, parseOptions);

				//
				var exportOptions = new ExportOptions()
				{
					Append = !_options.OverWrite,
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
				entries.Export(OutputFormat.JSON, _options.TargetFile, exportOptions);

				if (!System.Console.IsInputRedirected)
					"Press 'X' to exit".Dump();
			}
		}
	}
}