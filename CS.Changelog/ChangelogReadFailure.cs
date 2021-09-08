using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CS.Changelog
{
	/// <summary>
	/// A changelog reading failure class, containing reason and message for failure
	/// </summary>
	public class ChangelogReadFailure
	{

		/// <summary>
		/// Gets or sets the reason.
		/// </summary>
		/// <value>
		/// The reason.
		/// </value>
		[JsonConverter(typeof(StringEnumConverter))]
		public ChangelogReadFailureReason Reason { get; set; }

		/// <summary>
		/// Gets or sets the failure message.
		/// </summary>
		/// <value>
		/// The message containing a human friendly, English error message.
		/// </value>
		public string Message { get; set; }

	}
}
