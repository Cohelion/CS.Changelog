namespace CS.Changelog
{
	/// <summary>
	/// A changelog message, usually derived from a commit message
	/// </summary>
	public class ChangeLogMessage
	{
		/// <summary>Gets or sets the category.</summary>
		/// <value>The category.</value>
		public string Category { get; set; }

		/// <summary>Gets or sets the hash of the commit.</summary>
		/// <value>The hash.</value>
		public string Hash { get; set; }

		/// <summary>Gets or sets the message.</summary>
		/// <value>The message.</value>
		public string Message { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return $"{Hash} [{Category}] {Message}";
		}
	}
}