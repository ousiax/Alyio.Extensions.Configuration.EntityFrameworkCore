using Microsoft.EntityFrameworkCore;

namespace Alyio.Extensions.Configuration.EntityFrameworkCore;

internal sealed class KeyValuePairDbContext : DbContext
{
    public KeyValuePairDbContext(DbContextOptions<KeyValuePairDbContext> options) : base(options)
    {
    }

    public IQueryable<KeyValuePair> KeyValuePairs => Set<KeyValuePair>().AsNoTracking();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<KeyValuePair>();
        entity.ToTable("KeyValuePairs");
        entity.HasNoKey();
        entity.Property(kv => kv.Key).HasColumnName("pName");
        entity.Property(kv => kv.Value).HasColumnName("pValue");
    }
}
