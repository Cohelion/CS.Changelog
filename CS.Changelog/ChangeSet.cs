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
		public readonly DateTime? Date = DateTime.UtcNow;

		/// <summary>Adds the specified message.</summary>
		/// <param name="message">The message.</param>
		public new void Add(ChangeLogMessage message)
		{
			base.Add(message);
		}

		/// <summary>Adds the specified hash.</summary>
		/// <param name="hash">The hash.</param>
		/// <param name="category">The category.</param>
		/// <param name="message">The message.</param>
		public void Add(string hash, string category, string message = null)
		{
			base.Add(
				new ChangeLogMessage {
					Hash = hash,
					Category = category,
					Message = message == null ? string.Empty : message.Trim()
				});
		}
	}
}