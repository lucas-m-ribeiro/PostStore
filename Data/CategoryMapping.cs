using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poststore.Models;

namespace Poststore.Data.Mappings;

public class CategoryMapping : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Define o nome da tabela no banco de dados para a entidade Category
        builder.ToTable("categories");

        // Define a chave primária da entidade e nomeia a constraint
        builder
            .HasKey(x => x.Id)
            .HasName("pk_category");
        
        // Mapeia a propriedade Id para a coluna 'id' e configura comportamento de geração de valor
        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityAlwaysColumn()
            .UseSerialColumn();

        // Mapeia o objeto complexo Heading como colunas na mesma tabela (owned type)
        builder.OwnsOne(x => x.Heading, heading =>
        {
            // Mapeia Heading.Title para a coluna 'title' com tipo, tamanho e obrigatoriedade
            heading.Property(x => x.Title)
                .HasColumnName("title")
                .HasColumnType("varchar")
                .HasMaxLength(160)
                .IsRequired(true);

            // Mapeia Heading.Slug para a coluna 'slug' com tipo, tamanho e obrigatoriedade
            heading.Property(x => x.Slug)
                .HasColumnName("slug")
                .HasColumnType("varchar")
                .HasMaxLength(160)
                .IsRequired(true);
            
            // Cria um índice único na coluna 'slug' para garantir unicidade e melhorar consultas
            heading.HasIndex(x => x.Slug)
                .IsUnique()
                .HasDatabaseName("idx_categories_slug");
        });

        // Configura a relação 1:N indicando que Category tem muitos Products
        builder.HasMany(x => x.Products);
    }
}