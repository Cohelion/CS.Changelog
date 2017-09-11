using CS.Changelog.Exporters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CS.Changelog
{
	/// <summary>
	/// A full changelog, containing multiple changesets
	/// </summary>
	/// <seealso cref="List{T}" />
	[JsonObject(MemberSerialization.OptIn)]
	public class ChangeLog : List<ChangeSet>
	{
		/// <summary>Gets the repository URL. Allows serialized, unformatted exports to create links to commit details.</summary>
		/// <value>The repository URL.</value>
		/// <seealso cref="ExportOptions.RepositoryUrl"/>
		[JsonProperty(Order = 1)]
		public string RepositoryUrl { get; internal set; }

		/// <summary>Gets the issue number regex. Allows serialized, unformatted exports to recognize references to issues.</summary>
		/// <value>The issue number regex.</value>
		/// <seealso cref="ExportOptions.IssueNumberRegex"/>
		[JsonProperty(Order = 3)]
		public string IssueNumberRegex { get; internal set; }

		/// <summary>Gets the issue tracker URL. Allows serialized, unformatted exports to create links to issue details.</summary>
		/// <value>The issue tracker URL.</value>
		/// <seealso cref="ExportOptions.IssueTrackerUrl"/>
		[JsonProperty(Order = 2)]
		public string IssueTrackerUrl { get; internal set; }

		[JsonProperty(Order = 4)]
		private List<ChangeSet> ChangeSets {
			get{
				return new List<ChangeSet>(this);
			}
		}
	}
}