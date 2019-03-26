using CommandLine;
using CS.Changelog.Exporters;
using CS.Changelog.Utils;
using System;
using System.Text.RegularExpressions;

namespace CS.Changelog.Console
{
    /// <summary>
    /// A console application for generating a change log, performs the following steps:
    /// <list type="number">  
    ///    <item>  
    ///        <term>Read history</term>  
    ///        <description><see cref="GitExtensions.GetHistory">Obtain commit history, based on git commits</see></description>  
    ///    </item>  
    ///    <item>  
    ///        <term>Interpreting history</term>  
    ///        <description><see cref="Parsing.Parse">Interpret commits</see> based on <see cref="ParseOptions"/>.</description>  
    ///    </item>  
    ///    <item>  
    ///        <term>Writing or appending the changelog</term>  
    ///        <description><see cref="IChangelogExporter.Export">Export the changelog</see> to the requested <see cref="OutputFormat"/>.</description>  
    ///    </item>
    ///</list>
    /// </summary>
    class Program
    {
        /// <summary>The options to be passed to the program</summary>
        internal static Options _options = new Options();

        static void Main(string[] args)
        {
            $"CS /webworks changelog generator, version {Constants.Version}".Dump(LogLevel.Info);

            bool optionsParsed = true;
            var pr = Parser.Default.ParseArguments<Options>(args)
                            .WithParsed((opts) => _options = opts)
                            .WithNotParsed((errs) =>optionsParsed = false);

            if (!optionsParsed)
                return;

            _options.Dump(LogLevel.Info);

            var firstrun = true;

            while (firstrun
                || (!System.Console.IsInputRedirected && System.Console.ReadKey().Key != ConsoleKey.X))
            {
                firstrun = false;

                var log = GitExtensions.GetHistory(
                            workingDirectory: _options.RepositoryLocation,
                            pathToGit: _options.PathToGit,
                            incremental: !_options.Full,
                            startTag: _options.StartTag);

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

                //A lot of options can only be specific with negative default values when passed as command argument, hence the double negation
                var exportOptions = new ExportOptions()
                {
                    Append = !_options.Replace,
                    Reverse = true,
                    ResolveIssueNumbers = !_options.DontLinkifyIssueNumbers,
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
                        if (_options.OpenFile)
                        {
                            $"Opening file: {file.FullName}".Dump();
                            System.Diagnostics.Process.Start(file.FullName);
                        }
                        else
                            $"Not opening file: {file.FullName}".Dump();
                    }
                    else
                        $"Changelog does not exist at: {file.FullName}".Dump();

                    "Press 'X' to exit (and anything else to run again)".Dump();
                }
            }
        }
    }
}