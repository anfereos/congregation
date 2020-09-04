using Microsoft.EntityFrameworkCore.Migrations;

namespace Congregation.Web.Migrations
{
    public partial class DeleteCascadeForCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Churches_Districts_DistrictId",
                table: "Churches");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Countries_CountryId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_Name",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Churches_Name",
                table: "Churches");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Name_CountryId",
                table: "Districts",
                columns: new[] { "Name", "CountryId" },
                unique: true,
                filter: "[CountryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Churches_Name_DistrictId",
                table: "Churches",
                columns: new[] { "Name", "DistrictId" },
                unique: true,
                filter: "[DistrictId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Churches_Districts_DistrictId",
                table: "Churches",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Countries_CountryId",
                table: "Districts",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Churches_Districts_DistrictId",
                table: "Churches");

            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Countries_CountryId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Districts_Name_CountryId",
                table: "Districts");

            migrationBuilder.DropIndex(
                name: "IX_Churches_Name_DistrictId",
                table: "Churches");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_Name",
                table: "Districts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Churches_Name",
                table: "Churches",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Churches_Districts_DistrictId",
                table: "Churches",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Countries_CountryId",
                table: "Districts",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
