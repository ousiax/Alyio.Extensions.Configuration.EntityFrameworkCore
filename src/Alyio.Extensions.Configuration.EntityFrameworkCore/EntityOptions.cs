namespace Alyio.Extensions.Configuration.EntityFrameworkCore;

/// <summary>
/// Options used to configure the entity type.
/// </summary>
internal record EntityOptions
{
    public EntityOptions(string table, string key, string value)
    {
        TableName = table;
        KeyColumnName = key;
        ValueColumeName = value;
    }

    /// <summary>
    /// Specifies the database table that the configuration entity is mapped to.
    /// </summary>
    public string TableName { get; }

    /// <summary>
    /// Specifies the name of the column the configuration key is mapped to.
    /// </summary>
    public string KeyColumnName { get; }

    /// <summary>
    /// Specifies the name of the column the configuration value is mapped to.
    /// </summary>
    public string ValueColumeName { get; }
}
