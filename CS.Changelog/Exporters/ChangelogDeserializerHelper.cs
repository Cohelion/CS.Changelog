using System;
using System.Diagnostics.CodeAnalysis;

namespace CS.Changelog.Exporters
{
    /// <summary>
    /// Helper class for <see cref="IChangelogDeserializer"/> implementations
    /// </summary>
    public static class ChangelogDeserializerHelper
    {
        /// <summary>
        /// Tries to deserialize <paramref name="data"/> using <paramref name="deserializer"/>.
        /// </summary>
        /// <param name="deserializer">The deserializer.</param>
        /// <param name="data">The data.</param>
        /// <param name="changelog">The changelog.</param>
        /// <returns></returns>
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Pending more eloquent failure-reponse signature")]
        public static bool TryDeserialize(this IChangelogDeserializer deserializer, string data, out ChangeLog changelog)
        {
			if (deserializer == null) throw new ArgumentNullException(nameof(deserializer));

            changelog = null;
            if (string.IsNullOrWhiteSpace(data))
                return false;

            try
            {
                changelog = deserializer.Deserialize(data);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
    }
}
