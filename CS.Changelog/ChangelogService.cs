using CS.Changelog.Exporters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CS.Changelog
{
	/// <summary>
	/// 
	/// </summary>
	public static class ChangelogService
	{
		/// <summary>
		/// Obtains multiple changelogs
		/// </summary>
		/// <param name="changelogs">The paths to the changelogs to parse.</param>
		/// <param name="hideIssueTrackerInfo">if set to <c>true</c> hides issue tracker information.</param>
		/// <param name="hideCommitDetails">if set to <c>true</c> hide commit details.</param>
		/// <param name="ignoredCategories">The ignored categories.</param>
		/// <param name="deserializer">The deserializer, defaults to <see cref="JsonChangelogExporter"/>.</param>
		/// <returns></returns>
		public static ChangelogReadResult GetChangelogs(
			  this IReadOnlyCollection<FileInfo> changelogs
			, bool hideIssueTrackerInfo = false
			, bool hideCommitDetails = false
			, List<string> ignoredCategories = null
			, IChangelogDeserializer deserializer = null)
		{
			return changelogs
				.ToDictionary(x => x, x => deserializer)
				.GetChangelogs(hideIssueTrackerInfo, hideCommitDetails, ignoredCategories);
		}

		/// <summary>
		/// Obtains multiple changelogs
		/// </summary>
		/// <param name="changelogs">The paths to the changelogs to parse.</param>
		/// <param name="hideIssueTrackerInfo">if set to <c>true</c> hides issue tracker information.</param>
		/// <param name="hideCommitDetails">if set to <c>true</c> hide commit details.</param>
		/// <param name="ignoredCategories">The ignored categories.</param>
		/// <param name="deserializer">The deserializer, defaults to <see cref="JsonChangelogExporter"/>.</param>
		/// <returns></returns>
		public static ChangelogReadResult GetChangelogs(
			  bool hideIssueTrackerInfo = false
			, bool hideCommitDetails = false
			, List<string> ignoredCategories = null
			, IChangelogDeserializer deserializer = null
			, params FileInfo[] changelogs
			 )
		{
			return changelogs
				.ToList()
				.GetChangelogs(hideIssueTrackerInfo, hideCommitDetails, ignoredCategories, deserializer);
		}


		/// <summary>
		/// Obtains multiple changelogs
		/// </summary>
		/// <param name="changelogs">The paths to the changelogs to parse. Allows <see cref="IChangelogDeserializer"/> specification, defaulting to <see cref="JsonChangelogExporter"/>.</param>
		/// <param name="hideIssueTrackerInfo">if set to <c>true</c> hides issue tracker information.</param>
		/// <param name="hideCommitDetails">if set to <c>true</c> hide commit details.</param>
		/// <param name="ignoredCategories">The ignored categories.</param>
		/// <returns></returns>
		public static ChangelogReadResult GetChangelogs(
			this IReadOnlyDictionary<FileInfo, IChangelogDeserializer> changelogs
			, bool hideIssueTrackerInfo
			, bool hideCommitDetails
			, List<string> ignoredCategories = null)
		{

			var result = new ChangelogReadResult();

			if (changelogs is null
				|| !changelogs.Any())
			{
				result.AddFailure(new ChangelogReadFailure() { Reason = ChangelogReadFailureReason.EmptyList, Message = $"The provided list of changelogs is empty" });
				return result;
			}

			foreach (var file in changelogs)
			{
				var fileName = file.Key;
				var deserializer = file.Value ?? new JsonChangelogExporter();


				string content;

				try
				{
					/* Read and parse file */
					if (!fileName.Exists)
					{
						result.AddFailure(new ChangelogReadFailure() { Reason = ChangelogReadFailureReason.FileDoesntExist, Message = $"The following file doesn't exist cannot be opened: {fileName.FullName}" });
						continue;
					}

					using var reader = fileName.OpenText();
					content = reader.ReadToEnd();
				}
				catch (Exception ex)
				{
					result.AddFailure(new ChangelogReadFailure() { Reason = ChangelogReadFailureReason.ReadFailure, Message = $"The file could not be read : {fileName.FullName} > {ex.Message}" });
					continue;
				}

				if (string.IsNullOrWhiteSpace(content))
				{
					result.AddFailure(new ChangelogReadFailure() { Reason = ChangelogReadFailureReason.FileEmpty, Message = $"The following file is empty: {fileName.FullName}" });
					continue;
				}

				ChangeLog changelog;

				try
				{
					changelog = deserializer.Deserialize(content);
				}
				catch (Exception ex)
				{
					result.AddFailure(new ChangelogReadFailure() { Reason = ChangelogReadFailureReason.DeserializationError, Message = $"The file could not be parsed using {deserializer.GetType().Name} : {fileName.FullName} > {ex.Message}" });
					continue;
				}


				/* Sanitize as requested */
				if (hideIssueTrackerInfo)
				{
					changelog.IssueNumberRegex = null;
					changelog.IssueTrackerUrl = null;
				}

				//Removed ignored entries
				changelog.ForEach(
					changeset =>
					{
						//Remove ignored commits / messages
						changeset.RemoveAll(x => x.Ignore);

						//Remove commit hash when user does not have applicable permissions
						if (hideCommitDetails)
							changeset.ForEach(msg => { msg.Hash = string.Empty; });

						if (ignoredCategories != null && ignoredCategories.Any())
							changeset.RemoveAll(x => ignoredCategories.Contains(x.Category, StringComparer.InvariantCultureIgnoreCase));

					});

				//Remove empty change sets
				changelog.Where(set => !set.Any()).ToList().ForEach(set => changelog.Remove(set));
				result.AddSuccess(changelog);
			}

			return result;
		}
	}
}
