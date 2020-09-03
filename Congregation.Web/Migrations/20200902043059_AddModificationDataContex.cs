using Microsoft.EntityFrameworkCore.Migrations;

namespace Congregation.Web.Migrations
{
    public partial class AddModificationDataContex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Professions_Name",
                table: "Professions",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Professions_Name",
                table: "Professions");
        }
    }
}
