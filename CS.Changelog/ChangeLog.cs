﻿using CS.Changelog.Exporters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CS.Changelog
{
	/// <summary>
	/// A full changelog, containing multiple changesets
	/// </summary>
	/// <seealso cref="List{T}" />
	[JsonObject(MemberSerialization.OptIn)]
	[SuppressMessage("Naming", "CA1724:Type names should not match namespaces", Justification = "Pending better naming")]
	public class ChangeLog : List<ChangeSet>
	{
		/// <summary>Gets the repository URL. Allows serialized, unformatted exports to create links to commit details.</summary>
		/// <value>The repository URL.</value>
		/// <seealso cref="ExportOptions.RepositoryUrl"/>
		[JsonProperty(Order = 1)]
		public Uri RepositoryUrl { get; set; }

		/// <summary>Gets the issue number regex. Allows serialized, unformatted exports to recognize references to issues.</summary>
		/// <value>The issue number regex.</value>
		/// <seealso cref="BaseOptions.IssueNumberRegex"/>
		[JsonProperty(Order = 3)]
		public string IssueNumberRegex { get; set; }

		/// <summary>Gets the issue tracker URL. Allows serialized, unformatted exports to create links to issue details.</summary>
		/// <value>The issue tracker URL.</value>
		/// <seealso cref="ExportOptions.IssueTrackerUrl"/>
		[JsonProperty(Order = 2)]
		public Uri IssueTrackerUrl { get; set; }

		/// <summary>Gets or sets the change sets. For serialization purposes only</summary>
		/// <value>The change sets.</value>
		[JsonProperty(Order = 4)]
		private IEnumerable<ChangeSet> ChangeSets
		{
			get => ToArray();
			set => AddRange(value);
		}
	}
}
