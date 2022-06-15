using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Alyio.Extensions.Configuration.EntityFrameworkCore;

public static class EntityFrameworkExtensions
{
    public static IConfigurationBuilder AddEFConfiguration(this IConfigurationBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
    {
        return builder.Add(new EntityFrameworkCoreConfigurationSource(optionsAction));
    }
}
