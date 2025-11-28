using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kedu.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AjusteTipoCentroDeCusto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CentrosDeCusto_Nome_Tipo",
                table: "CentrosDeCusto");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "CentrosDeCusto");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "CentrosDeCusto",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CentrosDeCusto",
                keyColumn: "Id",
                keyValue: 1,
                column: "Tipo",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CentrosDeCusto",
                keyColumn: "Id",
                keyValue: 2,
                column: "Tipo",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CentrosDeCusto",
                keyColumn: "Id",
                keyValue: 3,
                column: "Tipo",
                value: 3);

            migrationBuilder.CreateIndex(
                name: "IX_CentrosDeCusto_Nome_Tipo",
                table: "CentrosDeCusto",
                columns: new[] { "Nome", "Tipo" },
                unique: true);
        }
    }
}
