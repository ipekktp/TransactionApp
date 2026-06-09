using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniBanking.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIbanToAccount2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "Accounts");
        }
    }
}
