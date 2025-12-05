using Microsoft.EntityFrameworkCore;
using Poststore.Models;

namespace Poststore.Data
{
    public class CategoryMapping: IEntityTypeConfiguration<Category>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(x => x.Id)
                .HasName("pk_category");

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .UseIdentityAlwaysColumn()
                .UseSerialColumn();

            builder.Property(x => x.Title)
                .HasColumnType("VARCHAR(160)")
                .HasColumnName("title")
                .IsRequired(true);

            builder.HasMany(x => x.Products);
        }
    }
}