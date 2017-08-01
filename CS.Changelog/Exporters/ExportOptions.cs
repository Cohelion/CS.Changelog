using System.Text.RegularExpressions;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// 
	/// </summary>
	public class ExportOptions
	{
		/// <summary>Wether to append to an existing file, or when <c>false</c> replace any existing file.</summary>
		public bool Append = true;
		/// <summary>Logs changesets in reverse order, newest one on top. (default is <c>true</c>)</summary>
		public bool Reverse = true;

		/// <summary>
		/// Wether to resolve issue numbers, when <c>true</c>, requires <see cref="IssueNumberRegex"/> to be set, see <see cref="IssueNumberRegex"/>.
		/// </summary>
		public bool ResolveIssueNumbers = true;
		/// <summary>The issue tracker URL, used when <see cref="ResolveIssueNumbers"/> is <c>true</c>.<c>$0</c> will be substituded with the issue number.</summary>
		public string IssueTrackerUrl;

		/// <summary>The repository URL, used when <see cref="LinkHash"/> is true.</summary>
		public string RepositoryUrl;
		/// <summary>Whether to linkify the <see cref="ChangeLogMessage.Hash"/>, uses <see cref="RepositoryUrl"/></summary>
		public bool LinkHash = true;
		/// <summary>Whether to display the short hash.</summary>
		public bool ShortHash = true;

		/// <summary>The regular expression for recognizing issue numbers. The entire match will be substituded in argument <c>$0</c> in <see cref="IssueTrackerUrl"/>.</summary>
		public Regex IssueNumberRegex;
	}
}