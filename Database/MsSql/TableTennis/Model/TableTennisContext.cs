using Microsoft.EntityFrameworkCore;

namespace table_tennis_backend.Database.MsSql.TableTennis.Model;

public partial class TableTennisContext : DbContext
{
        public TableTennisContext(DbContextOptions<TableTennisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Player> Player { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
