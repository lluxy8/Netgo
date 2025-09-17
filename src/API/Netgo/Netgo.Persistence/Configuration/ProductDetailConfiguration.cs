using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netgo.Domain;

namespace Netgo.Persistence.Configuration
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(x => x.Product)
                   .WithMany(p => p.Details)
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new ProductDetail
                {
                    Id = Guid.Parse("482a2c41-8b89-4ba8-bb9d-aefbf879ea68"),
                    ProductId = Guid.Parse("c2468aa5-9364-42d3-98e7-cea1ea708453"),
                    Title = "Renk",
                    Value = "Kırmızı",
                    DateCreated = DateTime.UtcNow
                },
                new ProductDetail
                {
                    Id = Guid.Parse("18b00abe-86b0-4d76-8686-7983ab2f0f34"),
                    ProductId = Guid.Parse("c2468aa5-9364-42d3-98e7-cea1ea708453"),
                    Title = "Boyut",
                    Value = "Orta",
                    DateCreated = DateTime.UtcNow
                }
            );
        }
    }
}
