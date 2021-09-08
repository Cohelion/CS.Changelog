namespace CS.Changelog
{
	/// <summary>
	/// A simple enumerator containing the possible changelog reading failure reasons
	/// </summary>
	public enum ChangelogReadFailureReason
	{
		/// <summary>
		/// The list of changelog filenames provided is empty
		/// </summary>
		EmptyList,
		/// <summary>
		/// The file doesn't exist
		/// </summary>
		FileDoesntExist,
		/// <summary>
		/// The file is empty
		/// </summary>
		FileEmpty,
		/// <summary>
		/// The file could not be read
		/// </summary>
		ReadFailure,
		/// <summary>
		/// The file could not be interpreted / parsed
		/// </summary>
		DeserializationError
	}
}
