using Kedu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kedu.Infra.Data.Configurations;

public class PlanoDePagamentoConfiguration : IEntityTypeConfiguration<PlanoDePagamento>
{
    public void Configure(EntityTypeBuilder<PlanoDePagamento> builder)
    {
        builder.ToTable("PlanosDePagamento");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ResponsavelFinanceiroId)
            .IsRequired();

        builder.Property(x => x.CentroDeCustoId)
            .IsRequired();

        builder.Ignore(x => x.ValorTotal);

        builder.HasIndex(x => x.ResponsavelFinanceiroId)
            .HasDatabaseName("IX_PlanosDePagamento_ResponsavelFinanceiroId");

        builder.HasIndex(x => x.CentroDeCustoId)
            .HasDatabaseName("IX_PlanosDePagamento_CentroDeCustoId");

        builder.HasOne(x => x.ResponsavelFinanceiro)
            .WithMany(x => x.PlanosDePagamento)
            .HasForeignKey(x => x.ResponsavelFinanceiroId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CentroDeCusto)
            .WithMany(x => x.PlanosDePagamento)
            .HasForeignKey(x => x.CentroDeCustoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Cobrancas)
            .WithOne(x => x.PlanoDePagamento)
            .HasForeignKey(x => x.PlanoDePagamentoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}