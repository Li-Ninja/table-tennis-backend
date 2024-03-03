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

    public virtual DbSet<PlayerScoreHistory> PlayerScoreHistory { get; set; }

    public virtual DbSet<Result> Result { get; set; }

    public virtual DbSet<ResultItem> ResultItem { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasOne(d => d.Event).WithMany(p => p.Result)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Result_Event");

            entity.HasOne(d => d.PlayerA1)
                .WithMany()
                .HasForeignKey(d => d.Player_Id_A_1)
                .HasConstraintName("FK_Result_PlayerA1");

            entity.HasOne(d => d.PlayerA2)
                .WithMany()
                .HasForeignKey(d => d.Player_Id_A_2)
                .HasConstraintName("FK_Result_PlayerA2");

            entity.HasOne(d => d.PlayerB1)
                .WithMany()
                .HasForeignKey(d => d.Player_Id_B_1)
                .HasConstraintName("FK_Result_PlayerB1");

            entity.HasOne(d => d.PlayerB2)
                .WithMany()
                .HasForeignKey(d => d.Player_Id_B_2)
                .HasConstraintName("FK_Result_PlayerB2");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsSingleMatch).HasDefaultValueSql("((1))");
            entity.Property(e => e.Type).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
