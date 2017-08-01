using CommandLine;
using CommandLine.Text;

namespace CS.Changelog.Console
{
	class Options
	{
		//[Option('r', "read",
		//	Required = false,
		//	HelpText = "Input file to be processed.")]
		//public string InputFile { get; set; }

		[Option(
'o',
"overwrite",
DefaultValue = false,
HelpText = "Overwrite the target file. Default is not to overwrite, but to append.")]
		public bool OverWrite { get; set; }

		[Option(
			'f',
			"filename",
			DefaultValue = "Changelog",
			HelpText = "The file to write to, if no file extension is specified, output-specific extension will be added.")]
		public string TargetFile { get; set; }

		[Option(
			'v',
			"verbosity",
		  DefaultValue = LogLevel.Debug,
		  HelpText = "Prints all messages to standard output")]
		public LogLevel Verbosity { get; set; }

		[Option(
		 	"issueformat",
		  DefaultValue = @"\w{1,5}-\d{1,5}",
		  HelpText = "Expression for recognizing issue numbers")]
		public string IssueNumberRegex { get; set; }


		[Option(
		 	"issuetrackerurl",
		  DefaultValue = @"https://project.cs.nl/issue/{0}",
		  HelpText = "Url for recognizing issue numbers. '{0}' will be substituted with issue number")]
		public string IssueTrackerUrl { get; set; }

		[Option(
		 	"repositoryurl",
		  DefaultValue = @"https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM/commit/{0}",
		  HelpText = "Url for showing commit details")]
		public string RepositoryUrl { get; set; }

		[Option(
		 	"linkifyissuenumbers",
		  DefaultValue = true,
		  HelpText = "Recognized issue numbers will be converted to links")]
		public bool LinkifyIssueNumbers { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption('h', "help")]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this,
			  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}

		//CS-enhanced Gitflow branches
		[Option("branch_development", DefaultValue = "develop", HelpText = "The development branch")]
		public string branch_development { get; set; }

		[Option("branch_master", DefaultValue = "master", HelpText = "The master branch")]
		public string branch_master { get; set; }

		[Option("branch_preview", DefaultValue = "preview", HelpText = "The preview branch")]
		public string branch_preview { get; set; }

		//Gitflow branch prefixes
		[Option("prefix_hotfix", DefaultValue = "hotfix", HelpText = "The prefix of hotfix branches")]
		public string prefix_hotfix { get; set; }

		[Option("prefix_release", DefaultValue = "release", HelpText = "The prefix of release branches")]
		public string prefix_release { get; set; }

		[Option("prefix_feature ", DefaultValue = "feature", HelpText = "The prefix of release branches")]
		public string prefix_feature { get; set; }

		//Standard category names, derived from their respective branches
		[Option("category_feature", DefaultValue = "Feature", HelpText = "The display label of the feature category")]
		public string category_feature { get; set; }

		[Option("category_hotfix", DefaultValue = "Hotfix", HelpText = "The display label of the hotfix category", Required = false)]
		public string category_hotfix { get; set; }
	}
}