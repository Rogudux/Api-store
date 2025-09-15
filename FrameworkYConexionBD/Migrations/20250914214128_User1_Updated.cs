using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrameworkYConexionBD.Migrations
{
    /// <inheritdoc />
    public partial class User1_Updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SystemUser",
                keyColumn: "Id",
                keyValue: 1,
                column: "FirstName",
                value: "Jose Juan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SystemUser",
                keyColumn: "Id",
                keyValue: 1,
                column: "FirstName",
                value: "Jose");
        }
    }
}
