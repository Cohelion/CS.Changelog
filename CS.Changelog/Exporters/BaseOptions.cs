using System.Text.RegularExpressions;

namespace CS.Changelog.Exporters
{
	/// <summary>
	/// 
	/// </summary>
	public class BaseOptions
	{

		/// <summary>
		/// The default value for <see cref="IssueNumberRegex"/>
		/// </summary>
		public const string IssueNumberRegexDefault = @"[a-zA-Z]{1,4}-\d{1,4}";

		/// <summary>The regular expression for recognizing issue numbers.</summary>
		public Regex IssueNumberRegex { get; set; } = new Regex(IssueNumberRegexDefault, RegexOptions.IgnoreCase);
	}
}
