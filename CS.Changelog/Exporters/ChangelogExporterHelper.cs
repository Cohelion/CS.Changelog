namespace CS.Changelog.Exporters
{
    /// <summary>
    /// Helper class for <see cref="IChangelogExporter"/> implementations
    /// </summary>
    public static class ChangelogExporterHelper {
        /// <summary>
        /// Indicates if <paramref name="exporter"/> als implements <see cref=" IChangelogDeserializer"/>
        /// </summary>
        /// <param name="exporter">The exporter.</param>
        /// <returns></returns>
        public static bool SupportsDeserializing(this IChangelogExporter exporter) {
            return (exporter is IChangelogDeserializer);
        }
    }
}