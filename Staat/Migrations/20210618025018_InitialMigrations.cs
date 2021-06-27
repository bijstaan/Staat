using System;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Staat.Services;

namespace Staat.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            switch (migrationBuilder.ActiveProvider)
            {
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    migrationBuilder.CreateTable(
                        name: "Status",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                            Name = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                            Description = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                            Color = table.Column<string>(type: "VARCHAR(25)", nullable: false, maxLength: 25),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Status", x => x.Id);
                        });
                    migrationBuilder.CreateTable(
                        name: "Settings",
                        columns: table => new
                        {
                            Id = table.Column<int>(nullable: false)
                                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                            Key = table.Column<string>(type: "TEXT", nullable: false),
                            Value = table.Column<string>(type: "TEXT", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Settings", x => x.Id);
                        });
                    migrationBuilder.CreateTable(name: "User", columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                            FirstName = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                            LastName = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                            Email = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                            PasswordHash = table.Column<byte[]>(type: "LONGBLOB", nullable: false),
                            PasswordSalt = table.Column<byte[]>(type: "LONGBLOB", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        }, 
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_User", x => x.Id);
                        });
                    migrationBuilder.CreateTable(
                        name: "ServiceGroup",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                            Name = table.Column<string>(type: "VARCHAR(100)", nullable: false, maxLength: 100),
                            Description = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                            DefaultOpen = table.Column<bool>(type: "INT", defaultValue: false, nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_ServiceGroup", x => x.Id);
                        });
                    migrationBuilder.CreateTable(
                        name: "Service",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                            Name = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                            Description = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                            Url = table.Column<string>(type: "TEXT", nullable: true),
                            StatusId = table.Column<int>(type: "INTEGER", nullable: false),
                            GroupId = table.Column<int>(type: "INTEGER", nullable: false),
                            ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Service", x => x.Id);
                            table.ForeignKey(
                                name: "FK_Service_Service_ParentId",
                                column: x => x.ParentId,
                                principalTable: "Service",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_Service_ServiceGroup_GroupId",
                                column: x => x.GroupId,
                                principalTable: "ServiceGroup",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_Service_Status_StatusId",
                                column: x => x.StatusId,
                                principalTable: "Status",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });
                    migrationBuilder.CreateTable(
                        name: "Incident",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                            Title = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                            Description = table.Column<string>(type: "TEXT", nullable: true),
                            DescriptionHtml = table.Column<string>(type: "TEXT", nullable: true),
                            AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                            ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                            StartedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            EndedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
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
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });
                    migrationBuilder.CreateTable(
                        name: "IncidentMessage",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                            Message = table.Column<string>(type: "TEXT", nullable: false),
                            MessageHtml = table.Column<string>(type: "TEXT", nullable: false),
                            AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                            StatusId = table.Column<int>(type: "INTEGER", nullable: true),
                            IncidentId = table.Column<int>(type: "INTEGER", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
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
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_IncidentMessage_User_AuthorId",
                                column: x => x.AuthorId,
                                principalTable: "User",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });
                    migrationBuilder.CreateTable(
                        name: "Monitor",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                            Type = table.Column<int>(type: "INTEGER", nullable: false),
                            Host = table.Column<string>(type: "TEXT", nullable: false),
                            Port = table.Column<int>(type: "INTEGER", nullable: true),
                            MonitorTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                            ValidateSsl = table.Column<bool>(type: "INTEGER", nullable: true),
                            CurrentIncidentId = table.Column<int>(type: "INTEGER", nullable: true),
                            ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                            MonitorCron = table.Column<string>(type: "VARCHAR(14)", nullable: false, defaultValue: "00:01:00"),
                            LastRunTime = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                            NextRunTime = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Monitor", x => x.Id);
                            table.ForeignKey(
                                name: "FK_Monitor_Incident_CurrentIncidentId",
                                column: x => x.CurrentIncidentId,
                                principalTable: "Incident",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_Monitor_Service_ServiceId",
                                column: x => x.ServiceId,
                                principalTable: "Service",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade);
                        });
                    migrationBuilder.CreateTable(
                        name: "MonitorData",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                            PingTime = table.Column<int>(type: "INTEGER", nullable: false),
                            Available = table.Column<bool>(type: "INTEGER", nullable: false),
                            SslValid = table.Column<bool>(type: "INTEGER", nullable: true),
                            FailureReason = table.Column<string>(type: "TEXT", nullable: true),
                            MonitorId = table.Column<int>(type: "INTEGER", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
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
                    break;
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    migrationBuilder.CreateTable(
                        name: "Status",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                            Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                            Description = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: true),
                            Color = table.Column<string>(type: "NVARCHAR(25)", nullable: false, maxLength: 25),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Status", x => x.Id);
                        });
                    migrationBuilder.CreateTable(
                        name: "Settings",
                        columns: table => new
                        {
                            Id = table.Column<int>(nullable: false)
                                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                            Key = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                            Value = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Settings", x => x.Id);
                        });
                    migrationBuilder.CreateTable(name: "User", columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                            FirstName = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                            LastName = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                            Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                            PasswordHash = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                            PasswordSalt = table.Column<byte[]>(type: "VARBINARY(MAX)", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        }, 
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_User", x => x.Id);
                        });
                    migrationBuilder.CreateTable(
                        name: "ServiceGroup",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                            Name = table.Column<string>(type: "NVARCHAR(100)", nullable: false, maxLength: 100),
                            Description = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: true),
                            DefaultOpen = table.Column<bool>(type: "INT", defaultValue: false, nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_ServiceGroup", x => x.Id);
                        });
                    migrationBuilder.CreateTable(
                        name: "Service",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                            Name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                            Description = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: true),
                            Url = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                            StatusId = table.Column<int>(type: "INTEGER", nullable: false),
                            GroupId = table.Column<int>(type: "INTEGER", nullable: false),
                            ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Service", x => x.Id);
                            table.ForeignKey(
                                name: "FK_Service_Service_ParentId",
                                column: x => x.ParentId,
                                principalTable: "Service",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_Service_ServiceGroup_GroupId",
                                column: x => x.GroupId,
                                principalTable: "ServiceGroup",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_Service_Status_StatusId",
                                column: x => x.StatusId,
                                principalTable: "Status",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });
                    migrationBuilder.CreateTable(
                        name: "Incident",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                            Title = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                            Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                            DescriptionHtml = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                            AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                            ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                            StartedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            EndedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
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
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });
                    migrationBuilder.CreateTable(
                        name: "IncidentMessage",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                            Message = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                            MessageHtml = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                            AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                            StatusId = table.Column<int>(type: "INTEGER", nullable: true),
                            IncidentId = table.Column<int>(type: "INTEGER", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
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
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_IncidentMessage_User_AuthorId",
                                column: x => x.AuthorId,
                                principalTable: "User",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });
                    migrationBuilder.CreateTable(
                        name: "Monitor",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                            Type = table.Column<int>(type: "INTEGER", nullable: false),
                            Host = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                            Port = table.Column<int>(type: "INTEGER", nullable: true),
                            MonitorTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                            ValidateSsl = table.Column<bool>(type: "INTEGER", nullable: true),
                            CurrentIncidentId = table.Column<int>(type: "INTEGER", nullable: true),
                            ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                            MonitorCron = table.Column<string>(type: "VARCHAR(14)", nullable: false, defaultValue: "00:01:00"),
                            LastRunTime = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                            NextRunTime = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Monitor", x => x.Id);
                            table.ForeignKey(
                                name: "FK_Monitor_Incident_CurrentIncidentId",
                                column: x => x.CurrentIncidentId,
                                principalTable: "Incident",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_Monitor_Service_ServiceId",
                                column: x => x.ServiceId,
                                principalTable: "Service",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade);
                        });
                    migrationBuilder.CreateTable(
                        name: "MonitorData",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                            PingTime = table.Column<int>(type: "INTEGER", nullable: false),
                            Available = table.Column<bool>(type: "INTEGER", nullable: false),
                            SslValid = table.Column<bool>(type: "INTEGER", nullable: true),
                            FailureReason = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                            MonitorId = table.Column<int>(type: "INTEGER", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
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
                    break;
                case "Pomelo.EntityFrameworkCore.MySQL":
                    migrationBuilder.CreateTable(
                        name: "Status",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("MySQL:AutoIncrement", true),
                            Name = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                            Description = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                            Color = table.Column<string>(type: "VARCHAR(25)", nullable: false, maxLength: 25),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Status", x => x.Id);
                        });
                    migrationBuilder.CreateTable(
                        name: "Settings",
                        columns: table => new
                        {
                            Id = table.Column<int>(nullable: false)
                                .Annotation("MySQL:AutoIncrement", true),
                            Key = table.Column<string>(type: "TEXT", nullable: false),
                            Value = table.Column<string>(type: "TEXT", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Settings", x => x.Id);
                        });
                    migrationBuilder.CreateTable(name: "User", columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("MySQL:AutoIncrement", true),
                            FirstName = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                            LastName = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                            Email = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                            PasswordHash = table.Column<byte[]>(type: "LONGBLOB", nullable: false),
                            PasswordSalt = table.Column<byte[]>(type: "LONGBLOB", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        }, 
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_User", x => x.Id);
                        });
                    migrationBuilder.CreateTable(
                        name: "ServiceGroup",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("MySQL:AutoIncrement", true),
                            Name = table.Column<string>(type: "VARCHAR(100)", nullable: false, maxLength: 100),
                            Description = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                            DefaultOpen = table.Column<bool>(type: "INT", defaultValue: false, nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_ServiceGroup", x => x.Id);
                        });
                    migrationBuilder.CreateTable(
                        name: "Service",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("MySQL:AutoIncrement", true),
                            Name = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                            Description = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                            Url = table.Column<string>(type: "TEXT", nullable: true),
                            StatusId = table.Column<int>(type: "INTEGER", nullable: false),
                            GroupId = table.Column<int>(type: "INTEGER", nullable: false),
                            ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Service", x => x.Id);
                            table.ForeignKey(
                                name: "FK_Service_Service_ParentId",
                                column: x => x.ParentId,
                                principalTable: "Service",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_Service_ServiceGroup_GroupId",
                                column: x => x.GroupId,
                                principalTable: "ServiceGroup",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_Service_Status_StatusId",
                                column: x => x.StatusId,
                                principalTable: "Status",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });
                    migrationBuilder.CreateTable(
                        name: "Incident",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("MySQL:AutoIncrement", true),
                            Title = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                            Description = table.Column<string>(type: "TEXT", nullable: true),
                            DescriptionHtml = table.Column<string>(type: "TEXT", nullable: true),
                            AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                            ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                            StartedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            EndedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
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
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });
                    migrationBuilder.CreateTable(
                        name: "IncidentMessage",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("MySQL:AutoIncrement", true),
                            Message = table.Column<string>(type: "TEXT", nullable: false),
                            MessageHtml = table.Column<string>(type: "TEXT", nullable: false),
                            AuthorId = table.Column<int>(type: "INTEGER", nullable: false),
                            StatusId = table.Column<int>(type: "INTEGER", nullable: true),
                            IncidentId = table.Column<int>(type: "INTEGER", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
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
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_IncidentMessage_User_AuthorId",
                                column: x => x.AuthorId,
                                principalTable: "User",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                        });
                    migrationBuilder.CreateTable(
                        name: "Monitor",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("MySQL:AutoIncrement", true),
                            Type = table.Column<int>(type: "INTEGER", nullable: false),
                            Host = table.Column<string>(type: "TEXT", nullable: false),
                            Port = table.Column<int>(type: "INTEGER", nullable: true),
                            MonitorTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                            ValidateSsl = table.Column<bool>(type: "INTEGER", nullable: true),
                            CurrentIncidentId = table.Column<int>(type: "INTEGER", nullable: true),
                            ServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                            MonitorCron = table.Column<string>(type: "VARCHAR(14)", nullable: false, defaultValue: "00:01:00"),
                            LastRunTime = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                            NextRunTime = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
                        },
                        constraints: table =>
                        {
                            table.PrimaryKey("PK_Monitor", x => x.Id);
                            table.ForeignKey(
                                name: "FK_Monitor_Incident_CurrentIncidentId",
                                column: x => x.CurrentIncidentId,
                                principalTable: "Incident",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Restrict);
                            table.ForeignKey(
                                name: "FK_Monitor_Service_ServiceId",
                                column: x => x.ServiceId,
                                principalTable: "Service",
                                principalColumn: "Id",
                                onDelete: ReferentialAction.Cascade);
                        });
                    migrationBuilder.CreateTable(
                        name: "MonitorData",
                        columns: table => new
                        {
                            Id = table.Column<int>(type: "INTEGER", nullable: false)
                                .Annotation("MySQL:AutoIncrement", true),
                            PingTime = table.Column<int>(type: "INTEGER", nullable: false),
                            Available = table.Column<bool>(type: "INTEGER", nullable: false),
                            SslValid = table.Column<bool>(type: "INTEGER", nullable: true),
                            FailureReason = table.Column<string>(type: "TEXT", nullable: true),
                            MonitorId = table.Column<int>(type: "INTEGER", nullable: false),
                            CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                            UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false)
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
                    break;
            }
            
            migrationBuilder.CreateIndex(
                name: "IX_Incident_ServiceId",
                table: "Incident",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentMessage_IncidentId",
                table: "IncidentMessage",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentMessage_StatusId",
                table: "IncidentMessage",
                column: "StatusId");

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
                name: "IX_IncidentMessage_AuthorId",
                table: "IncidentMessage",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_AuthorId",
                table: "Incident",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncidentMessage");

            migrationBuilder.DropTable(
                name: "MonitorData");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Monitor");

            migrationBuilder.DropTable(
                name: "Incident");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "ServiceGroup");

            migrationBuilder.DropTable(
                name: "Status");
        }
    }
}
