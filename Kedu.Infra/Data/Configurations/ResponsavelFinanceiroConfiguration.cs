using Kedu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kedu.Infra.Data.Configurations;

public class ResponsavelFinanceiroConfiguration : IEntityTypeConfiguration<ResponsavelFinanceiro>
{
    public void Configure(EntityTypeBuilder<ResponsavelFinanceiro> builder)
    {
        builder.ToTable("ResponsaveisFinanceiros");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(x => x.PlanosDePagamento)
            .WithOne(x => x.ResponsavelFinanceiro)
            .HasForeignKey(x => x.ResponsavelFinanceiroId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}