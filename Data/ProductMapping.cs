using Microsoft.EntityFrameworkCore;
using Poststore.Models;

namespace Poststore.Data
{
    public class ProductMapping: IEntityTypeConfiguration<Product>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasKey(x => x.Id)
                .HasName("pk_product");

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn()
                .UseSerialColumn();

            builder.Property(x => x.Title)
                .HasColumnType("VARCHAR")
                .HasColumnName("title")
                .HasMaxLength(160)
                .IsRequired(true);

            builder.Property(x => x.Slug)
                .HasColumnType("VARCHAR")
                .HasColumnName("slug")
                .HasMaxLength(160)
                .IsRequired(true);
            
            builder.Property(x => x.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .IsRequired(true);
            
            builder.Property(x => x.UpdatedAtUtc)
                .HasColumnName("updated_at_utc")
                .IsRequired(true);

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .IsRequired(true);

            builder.Property(x => x.CategoryId)
                .HasColumnName("category_id");

            builder.HasOne(x => x.Category);

            builder.HasIndex(x => x.Slug)
                .IsUnique()
                .HasDatabaseName("ix_product_slug");
        }
    }
}