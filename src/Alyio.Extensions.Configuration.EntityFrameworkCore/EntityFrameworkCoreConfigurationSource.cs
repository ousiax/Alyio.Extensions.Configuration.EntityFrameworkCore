using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Alyio.Extensions.Configuration.EntityFrameworkCore;

internal sealed class EntityFrameworkCoreConfigurationSource : IConfigurationSource
{
    private readonly Action<DbContextOptionsBuilder> _dbContextOptionsBuilderAction;
    private readonly EntityOptions _entityOptions;

    public EntityFrameworkCoreConfigurationSource(
        Action<DbContextOptionsBuilder> dbContextOptionsBuilderAction,
        EntityOptions entityOptions)
    {
        (_dbContextOptionsBuilderAction, _entityOptions) = (dbContextOptionsBuilderAction, entityOptions);
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new EntityFrameworkCoreConfigurationProvider(_dbContextOptionsBuilderAction, _entityOptions);
    }
}
