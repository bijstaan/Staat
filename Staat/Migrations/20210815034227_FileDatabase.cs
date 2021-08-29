using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Staat.Migrations
{
    public partial class FileDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_ServiceGroup_GroupId",
                table: "Service");
            switch (migrationBuilder.ActiveProvider)
            {
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    migrationBuilder.DropForeignKey(
                        name: "FK_Service_ServiceGroup_GroupId",
                        table: "Service");
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    migrationBuilder.CreateTable(
                        name: "File",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "int", nullable: false)
                                .Annotation("SqlServer:Identity", "1, 1"), 
                            Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                            Namespace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                            Hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                            MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_File", x => x.Id);
                        });

                    migrationBuilder.CreateTable(
                        name: "FileIncident",
                        columns: table => new
                        {   
                            FilesId = table.Column<int>(type: "int", nullable: false),
                            IncidentsId = table.Column<int>(type: "int", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_FileIncident", x => new { x.FilesId, x.IncidentsId });
                            table.ForeignKey(
                                name: "FK_FileIncident_File_FilesId",
                                column: x => x.FilesId,
                                principalTable: "File",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade);
                            table.ForeignKey(
                                name: "FK_FileIncident_Incident_IncidentsId",
                                column: x => x.IncidentsId,
                                principalTable: "Incident",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade);
                        });

                    migrationBuilder.CreateTable(
                        name: "FileIncidentMessage",
                        columns: table => new
                        {
                            AttachmentsId = table.Column<int>(type: "int", nullable: false),
                            IncidentMessagesId = table.Column<int>(type: "int", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_FileIncidentMessage", x => new { x.AttachmentsId, x.IncidentMessagesId });
                            table.ForeignKey(
                                name: "FK_FileIncidentMessage_File_AttachmentsId",
                                column: x => x.AttachmentsId,
                                principalTable: "File",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade);
                            table.ForeignKey(
                                name: "FK_FileIncidentMessage_IncidentMessage_IncidentMessagesId",
                                column: x => x.IncidentMessagesId,
                                principalTable: "IncidentMessage",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade);
                        });

                    migrationBuilder.CreateTable(
                        name: "FileMaintenance",
                        columns: table => new
                        { 
                            AttachmentsId = table.Column<int>(type: "int", nullable: false),
                            MaintenancesId = table.Column<int>(type: "int", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_FileMaintenance", x => new { x.AttachmentsId, x.MaintenancesId });
                            table.ForeignKey(
                                name: "FK_FileMaintenance_File_AttachmentsId",
                                column: x => x.AttachmentsId,
                                principalTable: "File",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade);
                            table.ForeignKey(
                                name: "FK_FileMaintenance_Maintenance_MaintenancesId",
                                column: x => x.MaintenancesId,
                                principalTable: "Maintenance",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade);
                        });

                    migrationBuilder.CreateTable(
                        name: "FileMaintenanceMessage",
                        columns: table => new
                        {
                            AttachmentsId = table.Column<int>(type: "int", nullable: false),
                            MaintenanceMessagesId = table.Column<int>(type: "int", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_FileMaintenanceMessage", x => new { x.AttachmentsId, x.MaintenanceMessagesId });
                            table.ForeignKey(
                                name: "FK_FileMaintenanceMessage_File_AttachmentsId",
                                column: x => x.AttachmentsId,
                                principalTable: "File",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade);
                            table.ForeignKey(
                                name: "FK_FileMaintenanceMessage_MaintenanceMessage_MaintenanceMessagesId",
                                column: x => x.MaintenanceMessagesId,
                                principalTable: "MaintenanceMessage",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade);
                        });
                    
                    break;
                case "Pomelo.EntityFrameworkCore.MySQL":
                    migrationBuilder.DropForeignKey(
                        name: "FK_Service_ServiceGroup_GroupId",
                        table: "Service");
                    break;
            }

            /*migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Status",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Status",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Settings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ServiceGroup",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ServiceGroup",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Service",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Service",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MonitorCron",
                table: "Monitor",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Incident",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndedAt",
                table: "Incident",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionHtml",
                table: "Incident",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Incident",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");*/

            migrationBuilder.CreateIndex(
                name: "IX_FileIncident_IncidentsId",
                table: "FileIncident",
                column: "IncidentsId");

            migrationBuilder.CreateIndex(
                name: "IX_FileIncidentMessage_IncidentMessagesId",
                table: "FileIncidentMessage",
                column: "IncidentMessagesId");

            migrationBuilder.CreateIndex(
                name: "IX_FileMaintenance_MaintenancesId",
                table: "FileMaintenance",
                column: "MaintenancesId");

            migrationBuilder.CreateIndex(
                name: "IX_FileMaintenanceMessage_MaintenanceMessagesId",
                table: "FileMaintenanceMessage",
                column: "MaintenanceMessagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_ServiceGroup_GroupId",
                table: "Service",
                column: "GroupId",
                principalTable: "ServiceGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_ServiceGroup_GroupId",
                table: "Service");

            migrationBuilder.DropTable(
                name: "FileIncident");

            migrationBuilder.DropTable(
                name: "FileIncidentMessage");

            migrationBuilder.DropTable(
                name: "FileMaintenance");

            migrationBuilder.DropTable(
                name: "FileMaintenanceMessage");

            migrationBuilder.DropTable(
                name: "MaintenanceService");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "MaintenanceMessage");

            migrationBuilder.DropTable(
                name: "Maintenance");

            /*migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Status",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Status",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Settings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ServiceGroup",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ServiceGroup",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Service",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Service",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "MonitorCron",
                table: "Monitor",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Incident",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndedAt",
                table: "Incident",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionHtml",
                table: "Incident",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Incident",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);*/

            migrationBuilder.AddForeignKey(
                name: "FK_Service_ServiceGroup_GroupId",
                table: "Service",
                column: "GroupId",
                principalTable: "ServiceGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
