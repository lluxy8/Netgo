using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netgo.Domain;

namespace Netgo.Persistence.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Tradable)
                .IsRequired();

            builder.Property(x => x.NormalizedTitle)
                .HasMaxLength(100);

            builder.Property(x => x.Price)
                .HasPrecision(18, 6);

            builder.Property(x => x.Images)
                .HasConversion(
                    v => string.Join(';', v), 
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList() 
                )
                .HasColumnName("ImageURLs")
                .HasMaxLength(1000);

            builder.HasMany(x => x.Details)
                .WithOne(x => x.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Product
                {
                    Id = Guid.Parse("c2468aa5-9364-42d3-98e7-cea1ea708453"),
                    CategoryId = Guid.Parse("0D2EE86D-81B7-429F-B91F-BAB837D4BDCC"),
                    UserId = Guid.Parse("432fb0ee-4eeb-4a7a-92d7-cf19b702669d"),
                    Title = "Örnek Ürün",
                    Price = 200,
                    NormalizedTitle = "ORNEK-URUN",
                    Description = "Bu bir örnek üründür.",
                    Tradable = true,
                    DateCreated = DateTime.UtcNow,
                    DateSold = null,
                    Images =
                    [
                        "images/c2468aa5-9364-42d3-98e7-cea1ea708453/sample-image1.jpg",
                    ],
                }
            );
        }
    }
}