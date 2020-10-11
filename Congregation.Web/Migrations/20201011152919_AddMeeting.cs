using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Congregation.Web.Migrations
{
    public partial class AddMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_Meetings_MeetingId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_AspNetUsers_UserId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Assistances");

            migrationBuilder.AlterColumn<int>(
                name: "ChurchId",
                table: "Meetings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Assistances",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MeetingId",
                table: "Assistances",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_Meetings_MeetingId",
                table: "Assistances",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_AspNetUsers_UserId",
                table: "Assistances",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings",
                column: "ChurchId",
                principalTable: "Churches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_Meetings_MeetingId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_AspNetUsers_UserId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings");

            migrationBuilder.AlterColumn<int>(
                name: "ChurchId",
                table: "Meetings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Assistances",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "MeetingId",
                table: "Assistances",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Assistances",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_Meetings_MeetingId",
                table: "Assistances",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_AspNetUsers_UserId",
                table: "Assistances",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_Churches_ChurchId",
                table: "Meetings",
                column: "ChurchId",
                principalTable: "Churches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
