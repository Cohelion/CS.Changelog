using System.Text.RegularExpressions;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// Options for exporting the change log (not all options are applicable for all implementataions of <see cref="IChangelogExporter"/>).
	/// </summary>
	public class ExportOptions
	{
		/// <summary>Whether to append to an existing file, or when <c>false</c> replace any existing file.</summary>
		public bool Append = true;
		/// <summary>Logs changesets in reverse order, newest one on top. (default is <c>true</c>)</summary>
		public bool Reverse = true;

		/// <summary>
		/// Wether to resolve issue numbers, when <c>true</c>, requires <see cref="IssueNumberRegex"/> to be set, see <see cref="IssueNumberRegex"/>.
		/// </summary>
		public bool ResolveIssueNumbers = true;
		/// <summary>The issue tracker URL, used when <see cref="ResolveIssueNumbers"/> is <c>true</c>.<c>$0</c> will be substituded with the issue number.</summary>
		public string IssueTrackerUrl = @"https://project.cs.nl/issue/{0}";

		/// <summary>The repository URL, used when <see cref="LinkHash"/> is true.</summary>
		public string RepositoryUrl = @"https://tfs.cs.nl/tfs/DefaultCollection/_git/MYUNCONFIGUREDPROJECTNAME/commit/{0}";
		/// <summary>Whether to linkify the <see cref="ChangeLogMessage.Hash"/>, uses <see cref="RepositoryUrl"/></summary>
		public bool LinkHash = true;
		/// <summary>Whether to display the short hash.</summary>
		public bool ShortHash = true;

		/// <summary>
		/// The default value for <see cref="IssueNumberRegex"/>
		/// </summary>
		public const string IssueNumberRegexDefault = @"[a-zA-Z]{1,4}-\d{1,4}";

		/// <summary>The regular expression for recognizing issue numbers. The entire match will be substituted in argument <c>$0</c> in <see cref="IssueTrackerUrl"/>.</summary>
		public Regex IssueNumberRegex = new Regex(IssueNumberRegexDefault, RegexOptions.IgnoreCase);
	}
}