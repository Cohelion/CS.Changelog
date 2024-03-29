﻿using System.Diagnostics.CodeAnalysis;
using CS.Changelog.Exporters;

namespace CS.Changelog
{
    /// <summary>
    /// Options for parsing the git changelog
    /// </summary>
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Names are exposed as command prompt options")]
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Names are exposed as command prompt options")]
    public class ParseOptions : BaseOptions
	{
        /// <summary>Gets or sets the prefix for feature branches.</summary>
        /// <value>The prefix for feature branches.</value>
        public string prefix_feature { get; set; } = "feature";

		/// <summary>Gets or sets the prefix for hotfix branches.</summary>
		/// <value>The prefix for hotfix branches.</value>
		public string prefix_hotfix { get; set; } = "hotfix";

		/// <summary>Gets or sets the prefix for release branches.</summary>
		/// <value>The prefix for release branches.</value>
		public string prefix_release { get; set; } = "release";

		/// <summary>Gets or sets the name of the development branch.</summary>
		/// <value>The name of the development branch.</value>
		public string branch_development { get; set; } = "develop";

		/// <summary>Gets or sets the name of the master branch.</summary>
		/// <value>The name of the master branch.</value>
		public string branch_master { get; set; } = "master";

		/// <summary>Gets or sets the name of the preview branch.</summary>
		/// <value>The name of the preview branch.</value>
		public string branch_preview { get; set; } = "preview";

		/// <summary>The display category for hotfixes</summary>
		public string category_hotfix { get; set;  } = "Hotfix";

		/// <summary>The display category for features</summary>
		public string category_feature { get; set; } = "Feature";

	}
}
