using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Alyio.Extensions.Configuration.EntityFrameworkCore;

internal sealed class EntityFrameworkCoreConfigurationProvider : ConfigurationProvider
{
    private readonly Action<DbContextOptionsBuilder> _dbContextOptionsBuilderAction;
    private readonly EntityOptions _entityOptions;

    public EntityFrameworkCoreConfigurationProvider(
        Action<DbContextOptionsBuilder> dbContextOptionsBuilderAction,
        EntityOptions entityOptions)
    {
        (_dbContextOptionsBuilderAction, _entityOptions) = (dbContextOptionsBuilderAction, entityOptions);
    }

    public override void Load()
    {
        var builder = new DbContextOptionsBuilder<KeyValuePairDbContext>();
        _dbContextOptionsBuilderAction(builder);

        using var dbContext = new KeyValuePairDbContext(builder.Options, _entityOptions);
        if (dbContext == null || dbContext.KeyValuePairs == null)
        {
#pragma warning disable CA2201 // Do not raise reserved exception types
            throw new Exception("Null DB context");
#pragma warning restore CA2201 // Do not raise reserved exception types
        }

        Data = dbContext
            .KeyValuePairs
            .Where(kv => !string.IsNullOrEmpty(kv.Key))
            .AsEnumerable()
            .DistinctBy(kv => kv.Key)
            .ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}