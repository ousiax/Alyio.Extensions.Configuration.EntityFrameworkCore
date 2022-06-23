using Alyio.Extensions.Configuration.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Configuration;

/// <summary>
/// Extension methods for adding <see cref="EntityFrameworkCoreConfigurationProvider"/>.
/// </summary>
public static class ConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds the EntityFrameworkCore configuration provider.
    /// </summary>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    /// <param name="dbContextOptionsBuilderAction">The delegate for configuring the <see cref="DbContextOptionsBuilder"/> that will be used
    /// to construct the <see cref="DbContext"/>.</param>
    /// <param name="tableName">Specifies the database table that the configuration entity is mapped to.</param>
    /// <param name="keyColumnName">Specifies the name of the column the configuration key is mapped to.</param>
    /// <param name="valueColumeName">Specifies the name of the column the configuration value is mapped to.</param>
    /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddEntityFrameworkCore(
        this IConfigurationBuilder builder,
        Action<DbContextOptionsBuilder> dbContextOptionsBuilderAction,
        string tableName,
        string keyColumnName,
        string valueColumeName)
    {
        return builder.Add(new EntityFrameworkCoreConfigurationSource(dbContextOptionsBuilderAction, new(tableName, keyColumnName, valueColumeName)));
    }
}
