using CS.Changelog.Exporters;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CS.Changelog
{
	/// <summary>
	/// The result of reading one or more changelog files
	/// </summary>
	/// <seealso cref="ChangelogService.GetChangelogs(bool, bool, IEnumerable{string}, IChangelogDeserializer, FileInfo[])"/>
	/// <seealso cref="ChangelogService.GetChangelogs(IReadOnlyCollection{FileInfo}, bool, bool, IEnumerable{string}, IChangelogDeserializer)"/>
	/// <seealso cref="ChangelogService.GetChangelogs(IReadOnlyDictionary{FileInfo, IChangelogDeserializer}, bool, bool, IEnumerable{string})"/>
	public class ChangelogReadResult
	{

		/// <summary>
		/// Gets the success.
		/// </summary>
		/// <value>
		/// The success.
		/// </value>
		public IEnumerable<ChangeLog> Success { get; } = new List<ChangeLog>();

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangelogReadResult"/> class.
		/// </summary>
		public ChangelogReadResult() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ChangelogReadResult"/> class, specifying <see cref="Failure"/>
		/// </summary>
		/// <param name="failures">The failures.</param>
		public ChangelogReadResult(IEnumerable<ChangelogReadFailure> failures) => Failure = new List<ChangelogReadFailure>(failures);

		/// <summary>
		/// Upon any failure, failures are listed here.
		/// </summary>
		/// <value>
		/// The failures.
		/// </value>
		public IEnumerable<ChangelogReadFailure> Failure { get; } = new List<ChangelogReadFailure>();

		/// <summary>
		/// Records success, by adding it to <see cref="ChangelogReadResult.Success"/>
		/// </summary>
		public void AddSuccess(ChangeLog changelog) => Success.ToList().Add(changelog);

		/// <summary>
		/// Records failure, by adding it to <see cref="Failure"/>
		/// </summary>
		public void AddFailure(ChangelogReadFailure changelogFailure) => Failure.ToList().Add(changelogFailure);
	}
}
