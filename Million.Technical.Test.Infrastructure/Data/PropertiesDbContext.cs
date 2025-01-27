using Microsoft.EntityFrameworkCore;
using Million.Technical.Test.Domain.Entities;

namespace Million.Technical.Test.Infrastructure.Data
{
    public class PropertiesDbContext : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<PropertyTrace> PropertyTraces { get; set; }

        public PropertiesDbContext(DbContextOptions<PropertiesDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Owner>(entity =>
            {
                entity.HasKey(e => e.IdOwner);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Photo);
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.IdProperty);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Price);
                entity.Property(e => e.CodeInternal).IsRequired().HasMaxLength(50);

                entity.HasOne(e => e.Owner)
                      .WithMany(o => o.Properties)
                      .HasForeignKey(e => e.IdOwner)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(e => e.IdPropertyImage);
                entity.Property(e => e.ImageData);

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.PropertyImages)
                      .HasForeignKey(e => e.IdProperty)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PropertyTrace>(entity =>
            {
                entity.HasKey(e => e.IdPropertyTrace);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Value);
                entity.Property(e => e.Tax);

                entity.HasOne(e => e.Property)
                      .WithMany(p => p.PropertyTraces)
                      .HasForeignKey(e => e.IdProperty)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}