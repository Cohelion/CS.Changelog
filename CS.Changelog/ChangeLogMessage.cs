using CS.Changelog.Exporters;
using System.Xml.Serialization;

namespace CS.Changelog
{
	/// <summary>
	/// A change log message, usually derived from a commit message
	/// </summary>
	public class ChangeLogMessage
	{
        /// <summary>Gets or sets the category.</summary>
        /// <value>The category.</value>
        [XmlAttribute]
        public string Category { get; set; }

		/// <summary>Gets or sets the hash of the commit.</summary>
		/// <value>The hash.</value>
        /// <remarks>In the serialized change log the hash may be empty, for not referring to a commit, but just adding a change log entry.</remarks>
        [XmlAttribute]
		public string Hash { get; set; }

        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        [XmlElement]
        public string Message { get; set; }

        /// <summary>
        /// Marks a change log entry as not-to be displayed.
        /// </summary>
        /// <value><c>true</c> if to be ignored; otherwise, <c>false</c>.</value>
        /// <remarks>Allows marking a particular commit message as to be ignored.
        /// This works when the change log is appended to a serializable format (like <see cref="OutputFormat.JSON"/> or <see cref="OutputFormat.XML"/>.
        /// When this property is set in the serialized format, the entry is hidden from the final output (like <see cref="OutputFormat.Console"/>, <see cref="OutputFormat.Html"/> or <see cref="OutputFormat.MarkDown"/>)
        /// </remarks>
        [XmlAttribute]
        public bool Ignore { get; set; }

        /// <summary>Indicated whether <see cref="Ignore"/> should be serialized</summary>
        /// <returns><c>false</c> unless <see cref="Ignore"/> is <c>true</c>.</returns>
        /// <remarks>This applied to both <see cref="XmlChangelogExporter"/> and <see cref="JsonChangelogExporter"/></remarks>
        public bool ShouldSerializeIgnore() {
            return Ignore;
        }

		/// <summary>
		/// Returns a <see cref="string" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="string" /> that represents this instance.</returns>
		public override string ToString()
		{
			return $"{Hash} [{Category}] {Message}";
		}
	}
}