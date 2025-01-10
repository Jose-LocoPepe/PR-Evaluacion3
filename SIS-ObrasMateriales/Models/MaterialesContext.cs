using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SIS_ObrasMateriales;

public partial class MaterialesContext : DbContext
{
    public MaterialesContext()
    {
    }

    public MaterialesContext(DbContextOptions<MaterialesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    public virtual DbSet<Obra> Obras { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        try
        {
            DotNetEnv.Env.Load();
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set.");
            }

            optionsBuilder.UseMySQL(connectionString);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while configuring the database: {ex.Message}");
            throw; // Re-throw the exception to ensure the application is aware of the failure
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

            entity.HasIndex(e => e.IdObra, "asigna2");

            entity.Property(e => e.IdMaterial).HasColumnName("id_material");
            entity.Property(e => e.IdObra).HasColumnName("id_obra");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");

            entity.HasOne(d => d.IdMaterialNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdMaterial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("asigna1");

            entity.HasOne(d => d.IdObraNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdObra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("asigna2");
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
