using Alyio.Extensions.Configuration.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KeyValuePair = Alyio.Extensions.Configuration.EntityFrameworkCore.KeyValuePair;

const string databaseName = "Data Source=kv.db";
using (var context = new MemoryDbContext(new DbContextOptionsBuilder<MemoryDbContext>().UseSqlite(databaseName).Options))
{
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();
    context.AddRange(
        new KeyValuePair { Key = "Blog1", Value = "http://blog1.com" },
        new KeyValuePair { Key = "Blog2", Value = "http://blog2.com" });
    context.SaveChanges();
}

var host = Host.CreateDefaultBuilder();

host.ConfigureAppConfiguration(options => options.AddEntityFrameworkCore(opt => opt.UseSqlite(databaseName)));

var configuration = host.Build().Services.GetRequiredService<IConfiguration>();
foreach (var c in configuration.AsEnumerable())
{
    Console.WriteLine(c.Key + " = " + c.Value);
}


class MemoryDbContext : DbContext
{
    public MemoryDbContext(DbContextOptions<MemoryDbContext> options) : base(options)
    {
    }

    public DbSet<KeyValuePair> KeyValuePairs => Set<KeyValuePair>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<KeyValuePair>();
        entity.ToTable("KeyValuePairs");
        entity.Property(kv => kv.Key).HasColumnName("pName");
        entity.Property(kv => kv.Value).HasColumnName("pValue");
        entity.HasKey(x => x.Key);
    }
}