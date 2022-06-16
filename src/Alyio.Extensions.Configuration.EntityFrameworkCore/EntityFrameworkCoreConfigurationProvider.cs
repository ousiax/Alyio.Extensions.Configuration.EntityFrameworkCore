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
        => (_dbContextOptionsBuilderAction, _entityOptions) = (dbContextOptionsBuilderAction, entityOptions);

    public override void Load()
    {
        var builder = new DbContextOptionsBuilder<KeyValuePairDbContext>();
        _dbContextOptionsBuilderAction(builder);

        using (var dbContext = new KeyValuePairDbContext(builder.Options, _entityOptions))
        {
            if (dbContext == null || dbContext.KeyValuePairs == null)
            {
                throw new Exception("Null DB context");
            }

            this.Data = dbContext
                .KeyValuePairs
                .Where(kv => !string.IsNullOrEmpty(kv.Key))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}