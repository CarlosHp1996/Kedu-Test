using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kedu.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CentrosDeCusto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentrosDeCusto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResponsaveisFinanceiros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsaveisFinanceiros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanosDePagamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResponsavelFinanceiroId = table.Column<int>(type: "integer", nullable: false),
                    CentroDeCustoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanosDePagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanosDePagamento_CentrosDeCusto_CentroDeCustoId",
                        column: x => x.CentroDeCustoId,
                        principalTable: "CentrosDeCusto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanosDePagamento_ResponsaveisFinanceiros_ResponsavelFinanc~",
                        column: x => x.ResponsavelFinanceiroId,
                        principalTable: "ResponsaveisFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cobrancas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlanoDePagamentoId = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MetodoPagamento = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    CodigoPagamento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cobrancas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cobrancas_PlanosDePagamento_PlanoDePagamentoId",
                        column: x => x.PlanoDePagamentoId,
                        principalTable: "PlanosDePagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CobrancaId = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamentos_Cobrancas_CobrancaId",
                        column: x => x.CobrancaId,
                        principalTable: "Cobrancas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CentrosDeCusto",
                columns: new[] { "Id", "Nome", "Tipo" },
                values: new object[,]
                {
                    { 1, "Matrícula", 1 },
                    { 2, "Mensalidade", 2 },
                    { 3, "Material Escolar", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CentrosDeCusto_Nome_Tipo",
                table: "CentrosDeCusto",
                columns: new[] { "Nome", "Tipo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cobrancas_CodigoPagamento",
                table: "Cobrancas",
                column: "CodigoPagamento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cobrancas_DataVencimento",
                table: "Cobrancas",
                column: "DataVencimento");

            migrationBuilder.CreateIndex(
                name: "IX_Cobrancas_PlanoDePagamentoId",
                table: "Cobrancas",
                column: "PlanoDePagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cobrancas_Status",
                table: "Cobrancas",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_CobrancaId",
                table: "Pagamentos",
                column: "CobrancaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanosDePagamento_CentroDeCustoId",
                table: "PlanosDePagamento",
                column: "CentroDeCustoId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanosDePagamento_ResponsavelFinanceiroId",
                table: "PlanosDePagamento",
                column: "ResponsavelFinanceiroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagamentos");

            migrationBuilder.DropTable(
                name: "Cobrancas");

            migrationBuilder.DropTable(
                name: "PlanosDePagamento");

            migrationBuilder.DropTable(
                name: "CentrosDeCusto");

            migrationBuilder.DropTable(
                name: "ResponsaveisFinanceiros");
        }
    }
}
