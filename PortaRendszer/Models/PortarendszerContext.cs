﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PortaRendszer.Models;

public partial class PortarendszerContext : DbContext
{
    public PortarendszerContext()
    {
    }

    public PortarendszerContext(DbContextOptions<PortarendszerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Belepes> Belepesek { get; set; }

    public virtual DbSet<Felhasznalo> Felhasznalok { get; set; }

    public virtual DbSet<Osztaly> Osztalyok { get; set; }

    public virtual DbSet<PortaUzenet> PortaUzenetek { get; set; }

    public virtual DbSet<SpecNap> SpecNapok { get; set; }

    public virtual DbSet<Tanterem> Tantermek { get; set; }

    public virtual DbSet<TanteremHasznalat> TanteremHasznalatok { get; set; }

    public virtual DbSet<Tanulo> Tanulok { get; set; }

    public virtual DbSet<TanuloArchiv> TanuloArchivum { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("MySql");
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_hungarian_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Belepes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("belepes");

            entity.HasIndex(e => e.FelhasznaloId, "felhasznalo_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.BelepesiIdo)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("belepesi_ido");
            entity.Property(e => e.FelhasznaloId)
                .HasColumnType("int(11)")
                .HasColumnName("felhasznalo_id");
            entity.Property(e => e.KilepesiIdo)
                .HasColumnType("timestamp")
                .HasColumnName("kilepesi_ido");
            entity.Property(e => e.UtolsoAktivitas)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("utolso_aktivitas");

            entity.HasOne(d => d.Felhasznalo).WithMany(p => p.Belepes)
                .HasForeignKey(d => d.FelhasznaloId)
                .HasConstraintName("belepes_ibfk_1");
        });

        modelBuilder.Entity<Felhasznalo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("felhasznalo");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.Felhasznalonev, "felhasznalonev").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Beosztas)
                .HasColumnType("enum('igazgato','igazgatohelyettes','osztalyfonok','tanar','napkozis_nevelo','pedagogiai_asszisztens','portas')")
                .HasColumnName("beosztas");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Felhasznalonev)
                .HasMaxLength(20)
                .HasColumnName("felhasznalonev");
            entity.Property(e => e.Jelszo)
                .HasMaxLength(255)
                .HasColumnName("jelszo");
            entity.Property(e => e.Nev)
                .HasMaxLength(100)
                .HasColumnName("nev");
        });

        modelBuilder.Entity<Osztaly>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("osztaly");

            entity.HasIndex(e => e.EgyediAzonosito, "egyedi_azonosito").IsUnique();

            entity.HasIndex(e => e.OsztalyfonokId, "osztalyfonok_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.EgyediAzonosito)
                .HasMaxLength(20)
                .HasColumnName("egyedi_azonosito");
            entity.Property(e => e.Nev)
                .HasMaxLength(10)
                .HasColumnName("nev");
            entity.Property(e => e.OsztalyfonokId)
                .HasColumnType("int(11)")
                .HasColumnName("osztalyfonok_id");

            entity.HasOne(d => d.Osztalyfonok).WithMany(p => p.Osztalyok)
            .HasForeignKey(d => d.OsztalyfonokId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("osztaly_ibfk_1");

        });

        modelBuilder.Entity<PortaUzenet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("porta_uzenet");

            entity.HasIndex(e => e.TanuloId, "tanulo_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Idopont)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("idopont");
            entity.Property(e => e.Statusz)
                .HasDefaultValueSql("'Jelen_van'")
                .HasColumnType("enum('Jelen_van','Hianyzo','Kulon_foglalkozas','Hazament')")
                .HasColumnName("statusz");
            entity.Property(e => e.TanuloId)
                .HasColumnType("int(11)")
                .HasColumnName("tanulo_id");
            entity.Property(e => e.Uzenet)
                .HasColumnType("text")
                .HasColumnName("uzenet");

            entity.HasOne(d => d.Tanulo).WithMany(p => p.PortaUzenets)
                .HasForeignKey(d => d.TanuloId)
                .HasConstraintName("porta_uzenet_ibfk_1");
        });

        modelBuilder.Entity<SpecNap>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("spec_nap");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Datum).HasColumnName("datum");
            entity.Property(e => e.FelugyeletOpcio)
                .HasDefaultValueSql("'0'")
                .HasColumnName("felugyelet_opcio");
            entity.Property(e => e.Megnevezes)
                .HasMaxLength(100)
                .HasColumnName("megnevezes");
        });

        modelBuilder.Entity<Tanterem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tanterem");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Aktiv)
                .HasDefaultValueSql("'1'")
                .HasColumnName("aktiv");
            entity.Property(e => e.Nev)
                .HasMaxLength(50)
                .HasColumnName("nev");
        });

        modelBuilder.Entity<TanteremHasznalat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tanterem_hasznalat");

            entity.HasIndex(e => e.FelhasznaloId, "felhasznalo_id");

            entity.HasIndex(e => e.OsztalyId, "osztaly_id");

            entity.HasIndex(e => e.TanteremId, "tanterem_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.FelhasznaloId)
                .HasColumnType("int(11)")
                .HasColumnName("felhasznalo_id");
            entity.Property(e => e.Idopont)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("idopont");
            entity.Property(e => e.OsztalyId)
                .HasColumnType("int(11)")
                .HasColumnName("osztaly_id");
            entity.Property(e => e.TanteremId)
                .HasColumnType("int(11)")
                .HasColumnName("tanterem_id");

            entity.HasOne(d => d.Felhasznalo).WithMany(p => p.TanteremHasznalats)
                .HasForeignKey(d => d.FelhasznaloId)
                .HasConstraintName("tanterem_hasznalat_ibfk_1");

            entity.HasOne(d => d.Osztaly).WithMany(p => p.TanteremHasznalats)
                .HasForeignKey(d => d.OsztalyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("tanterem_hasznalat_ibfk_3");

            entity.HasOne(d => d.Tanterem).WithMany(p => p.TanteremHasznalats)
                .HasForeignKey(d => d.TanteremId)
                .HasConstraintName("tanterem_hasznalat_ibfk_2");
        });

        modelBuilder.Entity<Tanulo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tanulo");

            entity.HasIndex(e => e.OktAzonosito, "idx_okt_azonosito").IsUnique();

            entity.HasIndex(e => e.OsztalyId, "osztaly_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.GondviseloNev)
                .HasMaxLength(100)
                .HasColumnName("gondviselo_nev");
            entity.Property(e => e.GondviseloStatusz)
                .HasMaxLength(50)
                .HasColumnName("gondviselo_statusz");
            entity.Property(e => e.Nev)
                .HasMaxLength(100)
                .HasColumnName("nev");
            entity.Property(e => e.OktAzonosito)
                .HasMaxLength(11)
                .HasColumnName("okt_azonosito");
            entity.Property(e => e.OsztalyId)
                .HasColumnType("int(11)")
                .HasColumnName("osztaly_id");
            entity.Property(e => e.SpecHazavitel)
                .HasDefaultValueSql("'0'")
                .HasColumnName("spec_hazavitel");
            entity.Property(e => e.Tanszobas)
                .HasDefaultValueSql("'1'")
                .HasColumnName("tanszobas");

            entity.HasOne(d => d.Osztaly).WithMany(p => p.Tanulos)
                .HasForeignKey(d => d.OsztalyId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("tanulo_ibfk_1");
        });

        modelBuilder.Entity<TanuloArchiv>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tanulo_archiv");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Nev)
                .HasMaxLength(100)
                .HasColumnName("nev");
            entity.Property(e => e.OktAzonosito)
                .HasMaxLength(11)
                .HasColumnName("okt_azonosito");
            entity.Property(e => e.OsztalyNev)
                .HasMaxLength(10)
                .HasColumnName("osztaly_nev");
            entity.Property(e => e.TorlesIdopont)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("torles_idopont");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
