using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Netgo.Domain;

namespace Netgo.Persistence.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(x => x.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Category
                {
                    Id = Guid.Parse("0D2EE86D-81B7-429F-B91F-BAB837D4BDCC"),
                    Name = "Not categorized",
                    DateCreated = DateTime.UtcNow,
                });

        }
    }
}
