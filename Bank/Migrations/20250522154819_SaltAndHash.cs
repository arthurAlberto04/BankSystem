using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Migrations
{
    /// <inheritdoc />
    public partial class SaltAndHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContaNumero",
                table: "Transacaos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Contas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Transacaos_ContaNumero",
                table: "Transacaos",
                column: "ContaNumero");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacaos_Contas_ContaNumero",
                table: "Transacaos",
                column: "ContaNumero",
                principalTable: "Contas",
                principalColumn: "Numero");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacaos_Contas_ContaNumero",
                table: "Transacaos");

            migrationBuilder.DropIndex(
                name: "IX_Transacaos_ContaNumero",
                table: "Transacaos");

            migrationBuilder.DropColumn(
                name: "ContaNumero",
                table: "Transacaos");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Contas");
        }
    }
}
