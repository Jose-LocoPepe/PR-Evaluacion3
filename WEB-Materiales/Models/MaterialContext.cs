using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WEB_Materiales.Models;

public partial class MaterialContext : DbContext
{
    public MaterialContext()
    {
    }

    public MaterialContext(DbContextOptions<MaterialContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Material> Materiales { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    public virtual DbSet<Obra> Obras { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_MATERIALES");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DB_CONNECTION_STRING_MATERIALES environment variable is not set.");
            }

            optionsBuilder.UseMySQL(connectionString);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while configuring the database: {ex.Message}");
            throw;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("material");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CantidadTotal).HasColumnName("cantidad_total");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.HasKey(e => new { e.IdMaterial, e.IdObra }).HasName("PRIMARY");

            entity.ToTable("movimiento");

            entity.HasIndex(e => e.IdObra, "id_obra");

            entity.Property(e => e.IdMaterial).HasColumnName("id_material");
            entity.Property(e => e.IdObra).HasColumnName("id_obra");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");

            entity.HasOne(d => d.IdMaterialNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdMaterial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimiento_ibfk_2");

            entity.HasOne(d => d.IdObraNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdObra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimiento_ibfk_1");
        });

        modelBuilder.Entity<Obra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("obra");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
