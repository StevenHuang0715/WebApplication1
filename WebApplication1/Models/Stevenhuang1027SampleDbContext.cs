using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class Stevenhuang1027SampleDbContext : DbContext
{
    public Stevenhuang1027SampleDbContext()
    {
    }

    public Stevenhuang1027SampleDbContext(DbContextOptions<Stevenhuang1027SampleDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Gallery> Galleries { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Gallery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Gallerie__3214EC0762899F4B");

            entity.Property(e => e.ImageUrl).HasMaxLength(200);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__Groups__149AF36AC6533827");

            entity.Property(e => e.GroupName).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07D1764C05");

            entity.Property(e => e.Desc).HasMaxLength(100);
            entity.Property(e => e.ImageUrl).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
