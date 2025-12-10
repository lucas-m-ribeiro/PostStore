using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poststore.Models;

namespace Poststore.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Define o nome da tabela no banco de dados para a entidade Product
        builder.ToTable("products");

        // Define a chave primária da entidade e nomeia a constraint
        builder
            .HasKey(x => x.Id)
            .HasName("pk_product");

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
                .HasDatabaseName("idx_products_slug");
        });

        // Mapeia CreatedAtUtc para 'created_at_utc', define valor padrão e obrigatoriedade
        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .HasDefaultValueSql("now()")
            .IsRequired(true);

        // Mapeia UpdatedAtUtc para 'updated_at_utc', define valor padrão e obrigatoriedade
        builder.Property(x => x.UpdatedAtUtc)
            .HasColumnName("updated_at_utc")
            .HasDefaultValueSql("now()")
            .IsRequired(true);

        // Mapeia IsActive para 'is_active' e marca como obrigatório
        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired(true);

        // Mapeia DefaultLanguage para 'default_language', define tamanho, valor padrão e obrigatoriedade
        builder.Property(e => e.DefaultLanguage)
            .HasColumnName("default_language")
            .HasMaxLength(8)
            .HasDefaultValue("en-us")
            .IsRequired();

        // Mapeia Translations para 'translations' usando tipo jsonb e define conversão para serializar/deserializar
        builder.Property(x => x.Translations)
            .HasColumnName("translations")
            .HasColumnType("jsonb")
            .HasConversion(
                x => JsonSerializer.Serialize(x, _jsonOptions),
                x => string.IsNullOrEmpty(x)
                    ? new List<Translation>()
                    : JsonSerializer.Deserialize<List<Translation>>(x, _jsonOptions)
                      ?? new List<Translation>()
            );

        // Mapeia CategoryId para a coluna 'category_id' (chave estrangeira)
        builder
            .Property(x => x.CategoryId)
            .HasColumnName("category_id");

        // Configura a relação entre Product e Category (produto referencia uma categoria)
        builder.HasOne(x => x.Category);
    }
}