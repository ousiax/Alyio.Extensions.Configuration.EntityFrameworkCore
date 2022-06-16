[![Alyio.Extensions.Configuration.EntityFrameworkCore](https://github.com/qqbuby/Alyio.Extensions.Configuration.EntityFrameworkCore/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/qqbuby/Alyio.Extensions.Configuration.EntityFrameworkCore/actions/workflows/ci.yml)

EntityFrameworkCore configuration provider implementation for Microsoft.Extensions.Configuration to load configuration arbitrary from database.

```cs
var host = Host.CreateDefaultBuilder(args);

host.ConfigureHostConfiguration(builder =>
{
    builder.AddJsonFile("hostsettings.json", optional: true);
});

host.ConfigureAppConfiguration((ctx, builder) =>
{
    var connectString = ctx.Configuration.GetConnectionString("kvdb");
    var tableName = ctx.Configuration.GetValue<string>("tableName");
    var keyColumnName = ctx.Configuration.GetValue<string>("keyColumnName");
    var valueColumeName = ctx.Configuration.GetValue<string>("valueColumeName");
    builder.AddEntityFrameworkCore(dbOpt => dbOpt.UseSqlite(connectString), tableName, keyColumnName, valueColumeName);
});
```