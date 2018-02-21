using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CS.Changelog
{
	/// <summary>
	/// A set of <see cref="ChangeLogMessage">changes</see>, release / deployed on a specific <see cref="Date"/>.
	/// </summary>
	/// <seealso cref="List{T}" />
	[JsonObject(MemberSerialization.OptIn)]
	public class ChangeSet : List<ChangeLogMessage>
	{
		/// <summary>The date of the release / deployment</summary>
		[JsonProperty(Order = 1)]
        [XmlAttribute]
        public DateTime? Date { get; set;} = DateTime.UtcNow;

		/// <summary>The name of the release</summary>
		[JsonProperty(Order = 2)]
        [XmlAttribute]
        public string Name { get; set; } = string.Empty;

		/// <summary>Gets the changes, for serialization purposes only</summary>
		/// <value>The changes.</value>
		[JsonProperty(Order = 3)]
        [XmlArrayItem("Change")]
        private IEnumerable<ChangeLogMessage> Changes { get{
				return ToArray(); ;
			}
			set{
				AddRange(value);
			}
		}

		/// <summary>Adds the specified hash.</summary>
		/// <param name="hash">The hash.</param>
		/// <param name="category">The category.</param>
		/// <param name="message">The message.</param>
		public void Add(string hash, string category, string message = null)
		{
			Add(
				new ChangeLogMessage {
					Hash = hash,
					Category = category,
					Message = message == null ? string.Empty : message.Trim()
				});
		}
	}
}