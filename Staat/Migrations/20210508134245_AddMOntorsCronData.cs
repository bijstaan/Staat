using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Staat.Migrations
{
    public partial class AddMOntorsCronData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastRunTime",
                table: "Monitor",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MonitorCron",
                table: "Monitor",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NextRunTime",
                table: "Monitor",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastRunTime",
                table: "Monitor");

            migrationBuilder.DropColumn(
                name: "MonitorCron",
                table: "Monitor");

            migrationBuilder.DropColumn(
                name: "NextRunTime",
                table: "Monitor");
        }
    }
}
