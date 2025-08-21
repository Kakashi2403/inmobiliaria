using System;
using System.Collections.Generic;
using inmobiliaria.infrastructure.Persistencia.entidades;
using Microsoft.EntityFrameworkCore;

namespace inmobiliaria.infrastructure.Persistencia;

public partial class RealEstateDbContext : DbContext
{
    public RealEstateDbContext()
    {
    }

    public RealEstateDbContext(DbContextOptions<RealEstateDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<PropertyImage> PropertyImages { get; set; }

    public virtual DbSet<PropertyTrace> PropertyTraces { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-6997H3G\\SQLEXPRESS;Database=RealEstateDb;User Id=sa;Password=Kakashi2403*;Encrypt=False;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.IdOwner);

            entity.ToTable("Owner");

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Photo).HasMaxLength(500);
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.IdProperty);

            entity.ToTable("Property");

            entity.HasIndex(e => e.IdOwner, "IX_Property_IdOwner");

            entity.HasIndex(e => e.CodeInternal, "UQ_Property_CodeInternal").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.CodeInternal).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdOwnerNavigation).WithMany(p => p.Properties)
                .HasForeignKey(d => d.IdOwner)
                .HasConstraintName("FK_Property_Owner");
        });

        modelBuilder.Entity<PropertyImage>(entity =>
        {
            entity.HasKey(e => e.IdPropertyImage);

            entity.ToTable("PropertyImage");

            entity.HasIndex(e => e.IdProperty, "IX_PropertyImage_IdProperty");

            entity.Property(e => e.Enabled).HasDefaultValue(true);
            entity.Property(e => e.File).HasMaxLength(500);

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.PropertyImages)
                .HasForeignKey(d => d.IdProperty)
                .HasConstraintName("FK_PropertyImage_Property");
        });

        modelBuilder.Entity<PropertyTrace>(entity =>
        {
            entity.HasKey(e => e.IdPropertyTrace);

            entity.ToTable("PropertyTrace");

            entity.HasIndex(e => e.IdProperty, "IX_PropertyTrace_IdProperty");

            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.PropertyTraces)
                .HasForeignKey(d => d.IdProperty)
                .HasConstraintName("FK_PropertyTrace_Property");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
