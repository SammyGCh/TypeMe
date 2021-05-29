using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MSMultimedia.Models
{
    public partial class MultimediasContext : DbContext
    {
        public MultimediasContext()
        {
        }

        public MultimediasContext(DbContextOptions<MultimediasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Multimedia> Multimedias { get; set; }
        public virtual DbSet<TipoMultimedium> TipoMultimedia { get; set; }

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
            modelBuilder.Entity<Multimedia>(entity =>
            {
                entity.HasKey(e => e.IdMultimedia)
                    .HasName("PRIMARY");

                entity.HasIndex(e => e.IdTipoMultimedia, "fk_Multimedia_TipoMultimedia_idx");

                entity.Property(e => e.IdMultimedia)
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Ruta)
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.IdTipoMultimediaNavigation)
                    .WithMany(p => p.Multimedia)
                    .HasForeignKey(d => d.IdTipoMultimedia)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Multimedia_TipoMultimedia");
            });

            modelBuilder.Entity<TipoMultimedium>(entity =>
            {
                entity.HasKey(e => e.IdTipoMultimedia)
                    .HasName("PRIMARY");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnType("varchar(20)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
