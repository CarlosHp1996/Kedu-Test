using Kedu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kedu.Infra.Data.Configurations;

public class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("Pagamentos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.CobrancaId)
            .IsRequired();

        builder.Property(x => x.Valor)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DataPagamento)
            .IsRequired();

        builder.HasIndex(x => x.CobrancaId)
            .HasDatabaseName("IX_Pagamentos_CobrancaId");

        builder.HasOne(x => x.Cobranca)
            .WithMany(x => x.Pagamentos)
            .HasForeignKey(x => x.CobrancaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}