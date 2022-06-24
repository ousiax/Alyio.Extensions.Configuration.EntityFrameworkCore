using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Alyio.Extensions.Configuration.EntityFrameworkCore.Tests;

public class EntityFrameworkExtensionsTests
{
    [Fact]
    public void TestEntityFrameworkExtensions()
    {
        // Arrange
        const string databaseName = "Data Source=kv.db";
        const string tableName = "KeyValuePairs";
        const string keyColumnName = "pName";
        const string valueColumeName = "pValue";
        using (var context = new KVDbContext(
            new DbContextOptionsBuilder<KVDbContext>().UseSqlite(databaseName).Options,
            tableName,
            keyColumnName,
            valueColumeName
            ))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.AddRange(
                new Configuration { Key = "Blog1", Value = "http://blog1.com" },
                new Configuration { Key = "Blog2", Value = "http://blog2.com" });
            context.SaveChanges();
        }
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddEntityFrameworkCore(opt => opt.UseSqlite(databaseName), tableName, keyColumnName, valueColumeName);
        var configuration = configurationBuilder.Build();

        // Act and Assert
        Assert.Equal("http://blog1.com", configuration.GetSection("Blog1").Value);
        Assert.Equal("http://blog2.com", configuration.GetSection("Blog2").Value);
    }

    private sealed class KVDbContext : DbContext
    {
        private readonly string _tableName;
        private readonly string _keyColumnName;
        private readonly string _valueColumeName;

        public KVDbContext(
            DbContextOptions<KVDbContext> dbContextOptions,
            string tableName,
            string keyColumnName,
            string valueColumeName) : base(dbContextOptions)
        {
            (_tableName, _keyColumnName, _valueColumeName) = (tableName, keyColumnName, valueColumeName);
        }

        public IQueryable<Configuration> Configurations => Set<Configuration>().AsNoTracking();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Configuration>();
            entity.ToTable(_tableName).HasKey(e => e.Key);
            entity.Property(kv => kv.Key).HasColumnName(_keyColumnName);
            entity.Property(kv => kv.Value).HasColumnName(_valueColumeName);
        }
    }

    private sealed class Configuration
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}