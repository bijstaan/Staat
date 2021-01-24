using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreStatus.Data.Migrations
{
    public partial class AddMonitors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Monitors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(nullable: false),
                    Host = table.Column<string>(nullable: false),
                    Port = table.Column<int>(nullable: false),
                    ValidateSsl = table.Column<bool>(nullable: false),
                    CurrentIncidentId = table.Column<int>(nullable: true),
                    ServiceId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Monitors_Incidents_CurrentIncidentId",
                        column: x => x.CurrentIncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Monitors_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Monitors_CurrentIncidentId",
                table: "Monitors",
                column: "CurrentIncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Monitors_ServiceId",
                table: "Monitors",
                column: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Monitors");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Statuses",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Statuses",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
