using Kedu.Domain.Entities;
using Kedu.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kedu.Infra.Data.Configurations;

public class CobrancaConfiguration : IEntityTypeConfiguration<Cobranca>
{
    public void Configure(EntityTypeBuilder<Cobranca> builder)
    {
        builder.ToTable("Cobrancas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PlanoDePagamentoId)
            .IsRequired();

        builder.Property(x => x.Valor)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.DataVencimento)
            .IsRequired();

        builder.Property(x => x.MetodoPagamento)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(StatusCobranca.Emitida);

        builder.Property(x => x.CodigoPagamento)
            .IsRequired()
            .HasMaxLength(50);

        builder.Ignore(x => x.IsVencida);

        builder.HasIndex(x => x.PlanoDePagamentoId)
            .HasDatabaseName("IX_Cobrancas_PlanoDePagamentoId");

        builder.HasIndex(x => x.CodigoPagamento)
            .IsUnique()
            .HasDatabaseName("IX_Cobrancas_CodigoPagamento");

        builder.HasIndex(x => x.DataVencimento)
            .HasDatabaseName("IX_Cobrancas_DataVencimento");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_Cobrancas_Status");

        builder.HasOne(x => x.PlanoDePagamento)
            .WithMany(x => x.Cobrancas)
            .HasForeignKey(x => x.PlanoDePagamentoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Pagamentos)
            .WithOne(x => x.Cobranca)
            .HasForeignKey(x => x.CobrancaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}