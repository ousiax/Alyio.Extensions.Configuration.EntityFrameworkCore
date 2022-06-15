using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Alyio.Extensions.Configuration.EntityFrameworkCore;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddEntityFrameworkCore(this IConfigurationBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
    {
        return builder.Add(new EntityFrameworkCoreConfigurationSource(optionsAction)); 
    }
}
