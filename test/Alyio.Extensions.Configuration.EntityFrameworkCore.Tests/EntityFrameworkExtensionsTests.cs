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
        using (var context = new MemoryDbContext(new DbContextOptionsBuilder<MemoryDbContext>().UseSqlite(databaseName).Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.AddRange(
                new KeyValuePair { Key = "Blog1", Value = "http://blog1.com" },
                new KeyValuePair { Key = "Blog2", Value = "http://blog2.com" });
            context.SaveChanges();
        }
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddEntityFrameworkCore(opt => opt.UseSqlite(databaseName));
        var configuration = configurationBuilder.Build();

        // Act and Assert
        Assert.Equal("http://blog1.com", configuration.GetSection("Blog1").Value);
        Assert.Equal("http://blog2.com", configuration.GetSection("Blog2").Value);
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
            entity.Property(kv => kv.Key).HasColumnName("pName");
            entity.Property(kv => kv.Value).HasColumnName("pValue");
            entity.HasKey(x => x.Key);
        }
    }
}