using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Staat.Migrations.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "ServiceGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DefaultOpen = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerificationString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Service_Service_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Service",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Service_ServiceGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ServiceGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Service_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maintenance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maintenance_User_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Incident",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incident", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incident_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incident_User_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "User",
                        principalColumn: "Id");
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
                name: "MaintenanceMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaintenanceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceMessage_Maintenance_MaintenanceId",
                        column: x => x.MaintenanceId,
                        principalTable: "Maintenance",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceMessage_User_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceService",
                columns: table => new
                {
                    MaintenanceId = table.Column<int>(type: "int", nullable: false),
                    ServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceService", x => new { x.MaintenanceId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_MaintenanceService_Maintenance_MaintenanceId",
                        column: x => x.MaintenanceId,
                        principalTable: "Maintenance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceService_Service_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IncidentMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MessageHtml = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    IncidentId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentMessage_Incident_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentMessage_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IncidentMessage_User_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Monitor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonitorTypeId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: true),
                    ValidateSsl = table.Column<bool>(type: "bit", nullable: true),
                    MonitorCron = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    NextRunTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastRunTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentIncidentId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Monitor_Incident_CurrentIncidentId",
                        column: x => x.CurrentIncidentId,
                        principalTable: "Incident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Monitor_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id");
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
                name: "MonitorData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PingTime = table.Column<long>(type: "bigint", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    SslValid = table.Column<bool>(type: "bit", nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonitorId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitorData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonitorData_Monitor_MonitorId",
                        column: x => x.MonitorId,
                        principalTable: "Monitor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Incident_AuthorId",
                table: "Incident",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_ServiceId",
                table: "Incident",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentMessage_AuthorId",
                table: "IncidentMessage",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentMessage_IncidentId",
                table: "IncidentMessage",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentMessage_StatusId",
                table: "IncidentMessage",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_AuthorId",
                table: "Maintenance",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMessage_AuthorId",
                table: "MaintenanceMessage",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceMessage_MaintenanceId",
                table: "MaintenanceMessage",
                column: "MaintenanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceService_ServicesId",
                table: "MaintenanceService",
                column: "ServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_Monitor_CurrentIncidentId",
                table: "Monitor",
                column: "CurrentIncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Monitor_ServiceId",
                table: "Monitor",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MonitorData_MonitorId",
                table: "MonitorData",
                column: "MonitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_GroupId",
                table: "Service",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_ParentId",
                table: "Service",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_StatusId",
                table: "Service",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Key",
                table: "Settings",
                column: "Key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "MonitorData");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Subscriber");

            migrationBuilder.DropTable(
                name: "IncidentMessage");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "MaintenanceMessage");

            migrationBuilder.DropTable(
                name: "Monitor");

            migrationBuilder.DropTable(
                name: "Maintenance");

            migrationBuilder.DropTable(
                name: "Incident");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ServiceGroup");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
