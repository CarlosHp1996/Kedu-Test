using Kedu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kedu.Infra.Data.Configurations;

public class CentroDeCustoConfiguration : IEntityTypeConfiguration<CentroDeCusto>
{
    public void Configure(EntityTypeBuilder<CentroDeCusto> builder)
    {
        builder.ToTable("CentrosDeCusto");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(x => x.PlanosDePagamento)
            .WithOne(x => x.CentroDeCusto)
            .HasForeignKey(x => x.CentroDeCustoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}