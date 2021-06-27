using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Staat.Migrations
{
    public partial class AddMaintenance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            switch (migrationBuilder.ActiveProvider)
            {
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    migrationBuilder.CreateTable(
                        name: "Maintenance",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                            Title = table.Column<string>(type: "varchar(100)", nullable: false),
                            Description = table.Column<string>(type: "TEXT", nullable: false),
                            StartedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            EndedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                            AuthorId = table.Column<int>(type: "int", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Maintenance", x => x.Id);
                            table.ForeignKey(
                                name: "FK_Maintenance_User_AuthorId",
                                column: x => x.AuthorId,
                                principalTable: "User",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });

                    migrationBuilder.CreateTable(
                        name: "MaintenanceMessage",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                            Message = table.Column<string>(type: "TEXT", nullable: false),
                            AuthorId = table.Column<int>(type: "int", nullable: false),
                            MaintenanceId = table.Column<int>(type: "int", nullable: true),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_MaintenanceMessage", x => x.Id);
                            table.ForeignKey(
                                name: "FK_MaintenanceMessage_Maintenance_MaintenanceId",
                                column: x => x.MaintenanceId,
                                principalTable: "Maintenance",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_MaintenanceMessage_User_AuthorId",
                                column: x => x.AuthorId,
                                principalTable: "User",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
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
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    migrationBuilder.CreateTable(
                        name: "Maintenance",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "int", nullable: false)
                                .Annotation("SqlServer:Identity", "1, 1"),
                            Title = table.Column<string>(type: "nvarchar(100)", nullable: false),
                            Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                            StartedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            EndedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                            AuthorId = table.Column<int>(type: "int", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Maintenance", x => x.Id);
                            table.ForeignKey(
                                name: "FK_Maintenance_User_AuthorId",
                                column: x => x.AuthorId,
                                principalTable: "User",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });

                    migrationBuilder.CreateTable(
                        name: "MaintenanceMessage",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "int", nullable: false)
                                .Annotation("SqlServer:Identity", "1, 1"),
                            Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                            AuthorId = table.Column<int>(type: "int", nullable: false),
                            MaintenanceId = table.Column<int>(type: "int", nullable: true),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_MaintenanceMessage", x => x.Id);
                            table.ForeignKey(
                                name: "FK_MaintenanceMessage_Maintenance_MaintenanceId",
                                column: x => x.MaintenanceId,
                                principalTable: "Maintenance",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_MaintenanceMessage_User_AuthorId",
                                column: x => x.AuthorId,
                                principalTable: "User",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
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
                    break;
                case "Pomelo.EntityFrameworkCore.MySQL":
                    migrationBuilder.CreateTable(
                        name: "Maintenance",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "int", nullable: false)
                                .Annotation("MySQL:AutoIncrement", true),
                            Title = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                            Description = table.Column<string>(type: "TEXT", nullable: false),
                            StartedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            EndedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                            AuthorId = table.Column<int>(type: "int", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Maintenance", x => x.Id);
                            table.ForeignKey(
                                name: "FK_Maintenance_User_AuthorId",
                                column: x => x.AuthorId,
                                principalTable: "User",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });

                    migrationBuilder.CreateTable(
                        name: "MaintenanceMessage",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "int", nullable: false)
                                .Annotation("MySQL:AutoIncrement", true),
                            Message = table.Column<string>(type: "TEXT", nullable: false),
                            AuthorId = table.Column<int>(type: "int", nullable: false),
                            MaintenanceId = table.Column<int>(type: "int", nullable: true),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_MaintenanceMessage", x => x.Id);
                            table.ForeignKey(
                                name: "FK_MaintenanceMessage_Maintenance_MaintenanceId",
                                column: x => x.MaintenanceId,
                                principalTable: "Maintenance",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_MaintenanceMessage_User_AuthorId",
                                column: x => x.AuthorId,
                                principalTable: "User",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
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
                    break;
            }

            

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceMessage");

            migrationBuilder.DropTable(
                name: "MaintenanceService");

            migrationBuilder.DropTable(
                name: "Maintenance");
        }
    }
}
