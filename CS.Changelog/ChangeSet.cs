using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CS.Changelog
{
	/// <summary>
	/// A set of <see cref="ChangeLogMessage">changes</see>, release / deployed on a specific <see cref="Date"/>.
	/// </summary>
	/// <seealso cref="List{T}" />
	public class ChangeSet : List<ChangeLogMessage>
	{

		/// <summary>The date of the release / deployment</summary>
		[JsonProperty]
		public DateTime? Date { get;set;} = DateTime.UtcNow;

		/// <summary>The name of the release</summary>
		[JsonProperty]
		public string Name;

		/// <summary>The date of the release / deployment</summary>
		/// <value>The changes.</value>
		[JsonProperty]
		public IEnumerable<ChangeLogMessage> Changes { get{
				return ToArray(); ;
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