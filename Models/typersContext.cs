using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MSTypers.Models
{
    public partial class TypersContext : DbContext
    {
        public TypersContext()
        {
        }

        public TypersContext(DbContextOptions<TypersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contrasenia> Contrasenias { get; set; }
        public virtual DbSet<Correo> Correos { get; set; }
        public virtual DbSet<Typer> Typers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                optionsBuilder.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.17-mysql"),
                builder => {
                            builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                        }
                );
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contrasenia>(entity =>
            {
                entity.HasKey(e => e.IdContrasenia)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdTyper, "fk_Contrasenias_Typers1_idx");

                entity.Property(e => e.Contrasenia1)
                    .IsRequired()
                    .HasColumnType("varchar(120)")
                    .HasColumnName("Contrasenia")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.IdTyper)
                    .IsRequired()
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.IdTyperNavigation)
                    .WithMany(p => p.Contrasenia)
                    .HasForeignKey(d => d.IdTyper)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Contrasenias_Typers1");
            });

            modelBuilder.Entity<Correo>(entity =>
            {
                entity.HasKey(e => e.IdCorreo)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdTyper, "fk_Correos_Typers_idx");

                entity.Property(e => e.Direccion)
                    .IsRequired()
                    .HasColumnType("varchar(80)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.IdTyper)
                    .IsRequired()
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.IdTyperNavigation)
                    .WithMany(p => p.Correos)
                    .HasForeignKey(d => d.IdTyper)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Correos_Typers");
            });

            modelBuilder.Entity<Typer>(entity =>
            {
                entity.HasKey(e => e.IdTyper)
                    .HasName("PRIMARY");

                entity.Property(e => e.IdTyper)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Estado)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FotoDePerfil)
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
