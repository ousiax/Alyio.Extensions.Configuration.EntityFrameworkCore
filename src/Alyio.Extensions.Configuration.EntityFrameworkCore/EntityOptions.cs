namespace Alyio.Extensions.Configuration.EntityFrameworkCore;

/// <summary>
/// Options used to configure the entity type.
/// </summary>
/// <param name="TableName">Specifies the database table that the configuration entity is mapped to.</param>
/// <param name="KeyColumnName">Specifies the name of the column the configuration key is mapped to.</param>
/// <param name="ValueColumeName">Specifies the name of the column the configuration value is mapped to.</param>
internal record EntityOptions(string TableName, string KeyColumnName, string ValueColumeName);
