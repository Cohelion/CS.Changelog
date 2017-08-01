namespace CS.Changelog
{
	/// <summary>
	/// The supported output formats
	/// </summary>
	public enum OutputFormat
	{
		/// <summary>Console-only output, does not write to file</summary>
		Console,
		/// <summary>MarkDown output, slightly enhanced text file, cannot be deserialized.</summary>
		MarkDown,
		/// <summary>Serializes to JSON, can be deserialized.</summary>
		JSON,
		/// <summary>Serializes to XML, can be deserialized.</summary>
		XML,
		/// <summary>Converts to HTML, keeps intermediate file (in the form of <see cref="MarkDown"/>, <see cref="JSON"/> or <see cref="XML"/>) in order to be able to support <see cref="Exporters.ExportOptions.Append"/>.</summary>
		Html
	}
}