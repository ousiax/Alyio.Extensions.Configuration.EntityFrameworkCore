using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Alyio.Extensions.Configuration.EntityFrameworkCore;

internal sealed class EntityFrameworkCoreConfigurationProvider : ConfigurationProvider
{
    private readonly Action<DbContextOptionsBuilder> _optionsAction;

    public EntityFrameworkCoreConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction) => _optionsAction = optionsAction;

    public override void Load()
    {
        var builder = new DbContextOptionsBuilder<KeyValuePairDbContext>();

        _optionsAction(builder);

        using (var dbContext = new KeyValuePairDbContext(builder.Options))
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