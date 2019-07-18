namespace CS.Changelog.Exporters
{
    /// <summary>
    /// Interface for implementing <see cref="ChangeLog"/> deserializing
    /// </summary>
    public interface IChangelogDeserializer
    {
        /// <summary>
        /// Deserializes the specified data to a <see cref="ChangeLog"/>
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        ChangeLog Deserialize(string data);
    }
}
