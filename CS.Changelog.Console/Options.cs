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
    ///  -n, --releasename         (Default: '')
    ///                            The name of the release (like 'operation high ground' or 'preview')
    ///                            
    ///  -g, --pathToGit           (Default: git)
    ///                            Path to git
    ///                               
    ///  -r, --repositoryDirectory (Default: '')
    ///                            Path to the repository
    ///                               
    ///      --replace             (Default: False)
    ///                            When set replaces the target file, instead of appending.
    ///                               
    ///  -f, --filename            (Default: Changelog)
    ///                            The file to write to, if no file extension is specified, output-specific extension will be added.
    ///                            
    ///  -o, --outputformat        (Default: JSON)
    ///                            The output format
    ///                               
    ///  -v, --verbosity           (Default: Debug)
    ///                            Prints all messages to standard output
    ///                            
    ///      --full     		   (Default: False)
    ///                            By default only changes since the last release need to be included, when set includes all changes.
    ///                            
    ///  -s, --startTag            (Default: null)
    ///                            In order to make an incremental change log since a specified tag. If no tag is specified auto-detects the last release tag.
    ///                            If a tag is specified, overrides option --full
    ///                               
    ///  --issueformat             (Default: <see cref="Exporters.ExportOptions.IssueNumberRegexDefault"/>)
    ///                            Expression for recognizing issue numbers
    ///                               
    ///  --issuetrackerurl         (Default: https://project.cs.nl/issue/{0})
    ///                            Url for recognizing issue numbers. '{0}' will be substituted with issue number
    ///                               
    ///  --repositoryurl           Url for showing commit details
    ///                               
    ///  --dontlinkifyissuenumbers (Default: False)
    ///                            When set recognized issue numbers will be not converted to links
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
    ///  --help                    Display this help screen.
    ///  
    ///  --openfile				   Open file after generation
    /// </code>
    /// </summary>
    public class Options
    {
        /// <summary>Gets or sets the path to git.</summary>
        /// <value>The path to git, defaults to <c>git</c>.</value>
        [Option(
            'n',
            "releasename",
            Default = "",
            HelpText = "The name ")]
        public string ReleaseName { get; set; }

        /// <summary>Gets or sets the path to git.</summary>
        /// <value>The path to git, defaults to <c>git</c>.</value>
        [Option(
            'g',
            "pathToGit",
            Default = "git",
            HelpText = "Path to git")]
        public string PathToGit { get; set; }

        /// <summary>Specified to generate an incremental change log or a full log.</summary>
        /// <value>Default to <c>false</c>.</value>
        [Option(
            "full",
            Default = false,
            HelpText = "false when only changes since the last release need to be included, true to include all changes.")]
        public bool Full { get; set; }

        private string _StartTag;

        /// <summary>The starting tag to use when making getting incremental history, overriding option 'full'.</summary>
        /// <value>Defaults to <c>null</c>.</value>
        [Option(
            's',
            "startTag",
            Default = "",
            HelpText = "The starting tag to use when making getting incremental history, overriding option 'full'.")]
        public string StartTag
        {
            get { return _StartTag; }
            set
            {
                _StartTag = value;
                if (!string.IsNullOrEmpty(value))
                    Full = false;
            }
        }

        /// <summary>Gets or sets the repository location, defaults to the current directory.</summary>
        /// <value>The repository location.</value>
        [Option(
            'r',
            "repositoryDirectory",
            Default = @"",
            HelpText = "Path to the repository")]
        public string RepositoryLocation { get; set; }

        /// <summary>
        /// Replace the target file. Default is not to overwrite, but to append.
        /// </summary>
        /// <value><c>true</c> if the exported file should be overwritten; otherwise, the file is appended.</value>
        [Option(
            "replace",
            Default = false,
            HelpText = "Replace the target file, instead of appending.")]
        public bool Replace { get; set; }

        /// <summary>Gets or sets the target file name.</summary>
        /// <value>The target file.</value>
        /// <remarks>If no file extension is specified, output-specific extension will be added.</remarks>
        [Option(
            'f',
            "filename",
            Default = "Changelog",
            HelpText = "The file to write to, if no file extension is specified, output-specific extension will be added.")]
        public string TargetFile { get; set; }

        /// <summary>When set opens the file after creation</summary>
        /// <value>Whether to open the generated file or not.</value>
        /// <remarks>Only works in interactive mode</remarks>
        [Option(
            "openfile",
            Default = false,
            HelpText = "When set, opens the file after the changelog has been generated")]
        public bool OpenFile { get; set; }

        /// <summary>The format of the changelog.</summary>
        /// <value>The target file.</value>
        /// <remarks>Some output formats (<see cref="OutputFormat.MarkDown"/>, <see cref="OutputFormat.Html"/>) do only correctly implement appending by when using an intermediate serializable format (JSON, XML) which is committed to the repository. </remarks>
        /// <seealso cref="OutputFormat"/>
        [Option(
            'o',
            "outputformat",
            Default = OutputFormat.JSON,
            HelpText = "The output format, choose between: Console, MarkDown, Html, Json or Xml")]
        public OutputFormat OutputFormat { get; set; }

        /// <summary>Gets or sets the <see cref="LogLevel">verbosity</see> of the output.</summary>
        /// <value>The verbosity. Default to <see cref="Utils.ConsoleExtensions.Verbosity"/>.</value>
        [Option(
            'v',
            "verbosity",
            Default = Utils.ConsoleExtensions.DefaultVerbosity,
            HelpText = "The threshold for printing messages to standard output")]
        public LogLevel Verbosity
        {
            get { return Utils.ConsoleExtensions.Verbosity; }
            set { Utils.ConsoleExtensions.Verbosity = value; }
        }

        /// <summary>
        /// The default issue number regular expression
        /// </summary>
        public const string DefaultIssueNumberRegex = Exporters.ExportOptions.IssueNumberRegexDefault;

        /// <summary>Gets or sets the issue number regular expression.</summary>
        /// <value>The issue number regex.</value>
        /// <remarks>Defaults to <see cref="DefaultIssueNumberRegex"/>.</remarks>
        /// <seealso cref="DontLinkifyIssueNumbers"/> 
        /// <seealso cref="IssueTrackerUrl"/> 
        [Option(
             "issueformat",
            Default = DefaultIssueNumberRegex,
            HelpText = "Expression for recognizing issue numbers")]
        public string IssueNumberRegex { get; set; }

        /// <summary>Gets or sets the issue tracker URL, for linkifying issue numbers.</summary>
        /// <value>The issue tracker URL.</value>
        /// <seealso cref="DontLinkifyIssueNumbers"/> 
        /// <seealso cref="IssueNumberRegex"/> 
        [Option(
             "issuetrackerurl",
            Default = @"https://project.cs.nl/issue/{0}",
            HelpText = "Url for recognizing issue numbers. '{0}' will be substituted with issue number")]
        public string IssueTrackerUrl { get; set; }


        /// <summary>Gets or sets the commit details URL, used when commit hashes are to be linkified.</summary>
        /// <value>The commit details URL.</value>
        [Option(
             "repositoryurl",
            Default = @"https://tfs.cs.nl/tfs/DefaultCollection/_git/Swissport%20Cargo%20DCM/commit/{0}",
            HelpText = "Url for showing commit details")]
        public string CommitDetailsUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether issue number should be linkified.
        /// </summary>
        /// <value><c>true</c> if issue numbers should be linkified; otherwise, <c>false</c>.</value>
        /// <seealso cref="IssueNumberRegex"/> 
        /// <seealso cref="IssueTrackerUrl"/> 
        [Option(
             "dontlinkifyissuenumbers",
            Default = false,
            HelpText = "Recognized issue numbers will be converted to links when set to false")]
        public bool DontLinkifyIssueNumbers { get; set; }

        ///// <summary>Gets or sets the last state of the parser.</summary>
        ///// <value>The last state of the parser.</value>
        //[ParserState, XmlIgnore]
        //public IParserState LastParserState { get; set; }

        ///// <summary>Gets the instructions for these parameters when used in a console application.</summary>
        ///// <returns>A help text explaining how to use the application.</returns>
        //[Option('h', "help")]
        //public string GetUsage()
        //{
        //    return HelpText.AutoBuild(this,
        //      (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        //}

        //CS-enhanced Gitflow branches
        #region Gitflow settings
        /// <summary>Gets or sets the name of the development branch for GitFlow purposes</summary>
        /// <value>The development branch name.</value>
        [Option("branch_development", Default = "develop", HelpText = "The development branch")]
        public string Branch_development { get; set; }

        /// <summary>Gets or sets the name of the master branch for GitFlow purposes</summary>
        /// <value>The master branch name.</value>
        [Option("branch_master", Default = "master", HelpText = "The master branch")]
        public string Branch_master { get; set; }

        /// <summary>Gets or sets the name of the preview branch for GitFlow+ purposes</summary>
        /// <value>The preview branch name.</value>
        [Option("branch_preview", Default = "preview", HelpText = "The preview branch")]
        public string Branch_preview { get; set; }

        /// <summary>Gets or sets the prefix of the hotfix branches for GitFlow purposes</summary>
        /// <value>The hotfix branches prefix.</value>
        [Option("prefix_hotfix", Default = "hotfix", HelpText = "The prefix of hotfix branches")]
        public string Prefix_hotfix { get; set; }

        /// <summary>Gets or sets the prefix of the release branches for GitFlow purposes</summary>
        /// <value>The release branches prefix.</value>
        [Option("prefix_release", Default = "release", HelpText = "The prefix of release branches")]
        public string Prefix_release { get; set; }

        /// <summary>Gets or sets the prefix of the feature branches for GitFlow purposes</summary>
        /// <value>The feature branches prefix.</value>
        [Option("prefix_feature ", Default = "feature", HelpText = "The prefix of release branches")]
        public string Prefix_feature { get; set; }

        /// <summary>Gets or sets header for features in the change log.</summary>
        /// <value>The feature-changes header text.</value>
        [Option("category_feature", Default = "Feature", HelpText = "The display label of the feature category")]
        public string Category_feature { get; set; }

        /// <summary>Gets or sets header for hotfixes in the change log.</summary>
        /// <value>The hotfix-changes header text.</value>
        [Option("category_hotfix", Default = "Hotfix", HelpText = "The display label of the hotfix category", Required = false)]
        public string Category_hotfix { get; set; }
        #endregion

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            var s = new XmlSerializer(GetType());

            using (var t = new StringWriter())
            {
                var w = new XmlTextWriter(t)
                {
                    Formatting = Formatting.Indented
                };

                s.Serialize(w, this);

                return t.ToString();
            }
        }
    }
}