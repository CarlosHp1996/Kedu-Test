using Kedu.Domain.Entities;
using Kedu.Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Kedu.Infra.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ResponsavelFinanceiro> ResponsaveisFinanceiros { get; set; }
    public DbSet<CentroDeCusto> CentrosDeCusto { get; set; }
    public DbSet<PlanoDePagamento> PlanosDePagamento { get; set; }
    public DbSet<Cobranca> Cobrancas { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ResponsavelFinanceiroConfiguration());
        modelBuilder.ApplyConfiguration(new CentroDeCustoConfiguration());
        modelBuilder.ApplyConfiguration(new PlanoDePagamentoConfiguration());
        modelBuilder.ApplyConfiguration(new CobrancaConfiguration());
        modelBuilder.ApplyConfiguration(new PagamentoConfiguration());
    }
}
