using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrameworkYConexionBD.Migrations
{
    /// <inheritdoc />
    public partial class User1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SystemUser",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password" },
                values: new object[] { 1, "fajnfajk@gmail.com", "Jose", "Pat", "12345" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemUser",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
