using Microsoft.EntityFrameworkCore;

namespace Alyio.Extensions.Configuration.EntityFrameworkCore;

internal sealed class KeyValuePairDbContext : DbContext
{
    private readonly EntityOptions _entityOptions;

    public KeyValuePairDbContext(DbContextOptions<KeyValuePairDbContext> dbContextOptions, EntityOptions entityOptions)
        : base(dbContextOptions) => _entityOptions = entityOptions;

    public IQueryable<KeyValuePair> KeyValuePairs => Set<KeyValuePair>().AsNoTracking();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<KeyValuePair>();
        entity.ToTable(_entityOptions.TableName).HasNoKey();
        entity.Property(kv => kv.Key).HasColumnName(_entityOptions.KeyColumnName);
        entity.Property(kv => kv.Value).HasColumnName(_entityOptions.ValueColumeName);
    }
}
