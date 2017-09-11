using CommandLine;
using CommandLine.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CS.Changelog.Console
{
    /// <summary>
    /// Options that can be passed to the console application:
    /// <code>
    ///  -g, --pathToGit           (Default: git)
    ///                            Path to git
    ///                               
    ///  -r, --repositoryDirectory (Default: ``)
    ///                            Path to the repository
    ///                               
    ///  -o, --overwrite           (Default: False)
    ///                            Overwrite the target file. Default is not to overwrite, but to append.
    ///                               
    ///  -f, --filename            (Default: Changelog)
    ///                            The file to write to, if no file extension is specified, output-specific extension will be added.
    ///                               
    ///  -v, --verbosity           (Default: Debug)
    ///                            Prints all messages to standard output
    ///                               
    ///  --issueformat             (Default: \w{ 1,5}-\d{1,5})
    ///                            Expression for recognizing issue numbers
    ///                               
    ///  --issuetrackerurl         (Default: https://project.cs.nl/issue/{0})
    ///                            Url for recognizing issue numbers. '{0}' will be substituted with issue number
    ///                               
    ///  --repositoryurl           Url for showing commit details
    ///                               
    ///  --linkifyissuenumbers     (Default: True)
    ///                            Recognized issue numbers will be converted to links
    ///                               
    ///  --branch_development      (Default: develop)
    ///                            The development branch
    ///                               
    ///  --branch_master           (Default: master)
    ///                            The master branch
    ///                               
    ///  --branch_preview          (Default: preview)
    ///                            The preview branch
    ///                               
    ///  --prefix_hotfix           (Default: hotfix)
    ///                            The prefix of hotfix branches
    ///                               
    ///  --prefix_release          (Default: release)
    ///                            The prefix of release branches
    ///                               
    ///  --prefix_feature          (Default: feature)
    ///                            The prefix of release ranches
    ///                               
    ///  --category_feature        (Default: Feature)
    ///                            The display label of the feature category
    ///                               
    ///  --category_hotfix         (Default: Hotfix)
    ///                            The display label of the hotfix category
    ///                               
    ///  -h, --help                Display this help screen.
    /// </code>
    /// </summary>
	public class Options
	{
		//[Option('r', "read",
		//	Required = false,
		//	HelpText = "Input file to be processed.")]
		//public string InputFile { get; set; }

		/// <summary>Gets or sets the path to git.</summary>
		/// <value>The path to git, defaults to <c>git</c>.</value>
		[Option(
			'g',
			"pathToGit",
			DefaultValue = "git",
			HelpText = "Path to git")]
		public string PathToGit { get; set; } = "git";

		/// <summary>Gets or sets the repository location, defaults to the current directory.</summary>
		/// <value>The repository location.</value>
		[Option(
			'r',
			"repositoryDirectory",
			DefaultValue = @"D:\Users\Robert\Documents\SwissportCargoDCM",
			HelpText = "Path to the repository")]
		public string RepositoryLocation { get; set; }

		/// <summary>
		/// Overwrite the target file. Default is not to overwrite, but to append.
		/// </summary>
		/// <value><c>true</c> if the exported file should be overwritten; otherwise, the file is appended.</value>
		[Option(
			'o',
			"overwrite",
			DefaultValue = false,
			HelpText = "Overwrite the target file. Default is not to overwrite, but to append.")]
		public bool OverWrite { get; set; }

		/// <summary>Gets or sets the target file name.</summary>
		/// <value>The target file.</value>
		/// <remarks>If no file extension is specified, output-specific extension will be added.</remarks>
		[Option(
			'f',
			"filename",
			DefaultValue = "Changelog",
			HelpText = "The file to write to, if no file extension is specified, output-specific extension will be added.")]
		public string TargetFile { get; set; }

		/// <summary>Gets or sets the verbosity.</summary>
		/// <value>The verbosity.</value>
		[Option(
			'v',
			"verbosity",
			DefaultValue = Utils.ConsoleExtensions.DefaultVerbosity,
			HelpText = "Prints all messages to standard output")]
		public LogLevel Verbosity
		{
			get { return Utils.ConsoleExtensions.Verbosity; }
			set { Utils.ConsoleExtensions.Verbosity = value; }
		}

		/// <summary>
		/// The default issue number regular expression
		/// </summary>
		public const string DefaultIssueNumberRegex = @"\w{1,5}-\d{1,5}";

		/// <summary>Gets or sets the issue number regular expression.</summary>
		/// <value>The issue number regex.</value>
		/// <remarks>Defaults to <see cref="DefaultIssueNumberRegex"/>.</remarks>
		/// <seealso cref="LinkifyIssueNumbers"/> 
		/// <seealso cref="IssueTrackerUrl"/> 
		[Option(
		 	"issueformat",
			DefaultValue = DefaultIssueNumberRegex,
			HelpText = "Expression for recognizing issue numbers")]
		public string IssueNumberRegex { get; set; }

		/// <summary>Gets or sets the issue tracker URL, for linkifying issue numbers.</summary>
		/// <value>The issue tracker URL.</value>
		/// <seealso cref="LinkifyIssueNumbers"/> 
		/// <seealso cref="IssueNumberRegex"/> 
		[Option(
		 	"issuetrackerurl",
			DefaultValue = @"https://project.cs.nl/issue/{0}",
			HelpText = "Url for recognizing issue numbers. '{0}' will be substituted with issue number")]
		public string IssueTrackerUrl { get; set; }


		/// <summary>Gets or sets the commit details URL, used when commit hashes are to be linkified.</summary>
		/// <value>The commit details URL.</value>
		[Option(
		 	"repositoryurl",
			DefaultValue = @"https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM/commit/{0}",
			HelpText = "Url for showing commit details")]
		public string CommitDetailsUrl { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether issue number should be linkified.
		/// </summary>
		/// <value><c>true</c> if issue numbers should be linkified; otherwise, <c>false</c>.</value>
		/// <seealso cref="IssueNumberRegex"/> 
		/// <seealso cref="IssueTrackerUrl"/> 
		[Option(
		 	"linkifyissuenumbers",
			DefaultValue = true,
			HelpText = "Recognized issue numbers will be converted to links")]
		public bool LinkifyIssueNumbers { get; set; }

		/// <summary>Gets or sets the last state of the parser.</summary>
		/// <value>The last state of the parser.</value>
		[ParserState, XmlIgnore]
		public IParserState LastParserState { get; set; }

		/// <summary>Gets the instructions for these parameters when used in a console application.</summary>
		/// <returns>A help text explaining how to use the application.</returns>
		[HelpOption('h', "help")]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this,
			  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}

		//CS-enhanced Gitflow branches
		#region Gitflow settings
		/// <summary>Gets or sets the name of the development branch for GitFlow purposes</summary>
		/// <value>The development branch name.</value>
		[Option("branch_development", DefaultValue = "develop", HelpText = "The development branch")]
		public string branch_development { get; set; }

		/// <summary>Gets or sets the name of the master branch for GitFlow purposes</summary>
		/// <value>The master branch name.</value>
		[Option("branch_master", DefaultValue = "master", HelpText = "The master branch")]
		public string branch_master { get; set; }

		/// <summary>Gets or sets the name of the preview branch for GitFlow+ purposes</summary>
		/// <value>The preview branch name.</value>
		[Option("branch_preview", DefaultValue = "preview", HelpText = "The preview branch")]
		public string branch_preview { get; set; }

		/// <summary>Gets or sets the prefix of the hotfix branches for GitFlow purposes</summary>
		/// <value>The hotfix branches prefix.</value>
		[Option("prefix_hotfix", DefaultValue = "hotfix", HelpText = "The prefix of hotfix branches")]
		public string prefix_hotfix { get; set; }

		/// <summary>Gets or sets the prefix of the release branches for GitFlow purposes</summary>
		/// <value>The release branches prefix.</value>
		[Option("prefix_release", DefaultValue = "release", HelpText = "The prefix of release branches")]
		public string prefix_release { get; set; }

		/// <summary>Gets or sets the prefix of the feature branches for GitFlow purposes</summary>
		/// <value>The feature branches prefix.</value>
		[Option("prefix_feature ", DefaultValue = "feature", HelpText = "The prefix of release branches")]
		public string prefix_feature { get; set; }

		/// <summary>Gets or sets header for features in the change log.</summary>
		/// <value>The feature-changes header text.</value>
		[Option("category_feature", DefaultValue = "Feature", HelpText = "The display label of the feature category")]
		public string category_feature { get; set; }

		/// <summary>Gets or sets header for hotfixes in the change log.</summary>
		/// <value>The hotfix-changes header text.</value>
		[Option("category_hotfix", DefaultValue = "Hotfix", HelpText = "The display label of the hotfix category", Required = false)]
		public string category_hotfix { get; set; }
		#endregion

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			var s = new XmlSerializer(this.GetType());

			using (var t = new StringWriter())
			{
				var w = new XmlTextWriter(t);
				{
					w.Formatting = Formatting.Indented;

					s.Serialize(w, this);

					return t.ToString();
				}
			}
		}
	}
}