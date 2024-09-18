using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ChartAPI.Models;

public partial class ChartProjectContext : DbContext
{
    public ChartProjectContext()
    {
    }

    public ChartProjectContext(DbContextOptions<ChartProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-C715D1H\\SQLEXPRESS;Database=ChartProject;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Staff>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Surname).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
