using Alyio.Extensions.Configuration.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args);

host.ConfigureServices(services => services.AddSingleton<InitializeDatabaseService>());
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


var app = host.Build();

await app.Services.GetRequiredService<InitializeDatabaseService>().InitializAsync();

var configuration = app.Services.GetRequiredService<IConfiguration>();

foreach (var c in configuration.AsEnumerable())
{
    Console.WriteLine(c.Key + " = " + c.Value);
}


class KVDbContext : DbContext
{
    private readonly string _tableName;
    private readonly string _keyColumnName;
    private readonly string _valueColumeName;

    public KVDbContext(
        DbContextOptions<KVDbContext> dbContextOptions,
        string tableName,
        string keyColumnName,
        string valueColumeName) : base(dbContextOptions)
        => (_tableName, _keyColumnName, _valueColumeName) = (tableName, keyColumnName, valueColumeName);

    public IQueryable<Configuration> Configurations => Set<Configuration>().AsNoTracking();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Configuration>();
        entity.ToTable(_tableName).HasKey(e => e.Key);
        entity.Property(kv => kv.Key).HasColumnName(_keyColumnName);
        entity.Property(kv => kv.Value).HasColumnName(_valueColumeName);
    }
}

class Configuration
{
    public string? Key { get; set; }
    public string? Value { get; set; }

}

public sealed class InitializeDatabaseService
{
    private readonly IConfiguration _configuration;

    public InitializeDatabaseService(IConfiguration configuration) => _configuration = configuration;

    public async Task InitializAsync()
    {
        var connectString = _configuration.GetConnectionString("kvdb");
        var tableName = _configuration.GetValue<string>("tableName");
        var keyColumnName = _configuration.GetValue<string>("keyColumnName");
        var valueColumeName = _configuration.GetValue<string>("valueColumeName");

        using var context = new KVDbContext
            (new DbContextOptionsBuilder<KVDbContext>().UseSqlite(connectString).Options, tableName, keyColumnName, valueColumeName);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.AddRange(
            new Configuration { Key = "Blog1", Value = "http://blog1.com" },
            new Configuration { Key = "Blog2", Value = "http://blog2.com" });
        await context.SaveChangesAsync();
    }
}