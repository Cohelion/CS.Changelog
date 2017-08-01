namespace CS.Changelog
{
	/// <summary>
	/// Basic log level for specifiying program verbosity.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>The error log level, will always be sent to output.</summary>
		Error = 0,
		/// <summary>Warnings, non necesarily errors, but may need attention.</summary>
		Warning = 1,
		/// <summary>Informational message, general progress</summary>
		Info = 2,
		/// <summary>The whole shebang, for debugging purposes only.</summary>
		Debug = 3,
	}
}