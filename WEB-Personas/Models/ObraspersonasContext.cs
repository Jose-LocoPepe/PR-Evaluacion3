﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WEB_Personas;

public partial class ObraspersonasContext : DbContext
{
    public ObraspersonasContext()
    {
    }

    public ObraspersonasContext(DbContextOptions<ObraspersonasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contrato> Contratos { get; set; }

    public virtual DbSet<Obra> Obras { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

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
        modelBuilder.Entity<Contrato>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("contrato");

            entity.HasIndex(e => e.IdPersona, "tiene2");

            entity.HasIndex(e => e.IdObra, "tiene3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaFin)
                .HasColumnType("date")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("date")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.IdObra).HasColumnName("id_obra");
            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.NumContrato)
                .HasMaxLength(50)
                .HasColumnName("num_contrato");
            entity.Property(e => e.Vigencia).HasColumnName("vigencia");

            entity.HasOne(d => d.IdObraNavigation).WithMany(p => p.Contratos)
                .HasForeignKey(d => d.IdObra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tiene3");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Contratos)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tiene2");
        });

        modelBuilder.Entity<Obra>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("obra");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AvanceGeneral).HasColumnName("avance_general");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("persona");

            entity.HasIndex(e => e.Rut, "rut").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Rut)
                .HasMaxLength(50)
                .HasColumnName("rut");
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tarea");

            entity.HasIndex(e => e.IdObra, "tiene1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdObra).HasColumnName("id_obra");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.PorcentajeAvance).HasColumnName("porcentaje_avance");

            entity.HasOne(d => d.IdObraNavigation).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.IdObra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tiene");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
