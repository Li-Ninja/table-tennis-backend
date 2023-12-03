using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace table_tennis_backend.Database.MsSql.TableTennis.Model;

public partial class TableTennisContext : DbContext
{
    public TableTennisContext()
    {
    }

    public TableTennisContext(DbContextOptions<TableTennisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Event { get; set; }

    public virtual DbSet<Player> Player { get; set; }

    public virtual DbSet<Result> Result { get; set; }

    public virtual DbSet<ResultItem> ResultItem { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasOne(d => d.Event).WithMany(p => p.Result)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Result_Event");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
