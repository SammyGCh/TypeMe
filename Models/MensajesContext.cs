using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MSMensajes.Models
{
    public partial class MensajesContext : DbContext
    {
        public MensajesContext()
        {
        }

        public MensajesContext(DbContextOptions<MensajesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Grupo> Grupos { get; set; }
        public virtual DbSet<Mensaje> Mensajes { get; set; }
        public virtual DbSet<Pertenece> Perteneces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                optionsBuilder.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.23-mysql"),
                    builder => {
                        builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    });
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grupo>(entity =>
            {
                entity.HasKey(e => e.IdGrupo)
                    .HasName("PRIMARY");

                entity.Property(e => e.Descripcion)
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.FechaCreacion).HasColumnType("date");

                entity.Property(e => e.Nombre)
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<Mensaje>(entity =>
            {
                entity.HasKey(e => e.IdMensaje)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdGrupo, "fk_Mensaje_Grupo");

                entity.Property(e => e.Contenido)
                    .IsRequired()
                    .HasColumnType("mediumtext")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.Property(e => e.Hora).HasColumnType("time");

                entity.Property(e => e.IdMultimedia)
                    .IsRequired()
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.IdTyper)
                    .IsRequired()
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.IdGrupoNavigation)
                    .WithMany(p => p.Mensajes)
                    .HasForeignKey(d => d.IdGrupo)
                    .HasConstraintName("fk_Mensaje_Grupo");
            });

            modelBuilder.Entity<Pertenece>(entity =>
            {
                entity.HasKey(e => new { e.IdGrupo, e.IdTyper })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("Pertenece");

                entity.Property(e => e.IdTyper)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.IdGrupoNavigation)
                    .WithMany(p => p.Perteneces)
                    .HasForeignKey(d => d.IdGrupo)
                    .HasConstraintName("fk_Pertenece_Grupo1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
