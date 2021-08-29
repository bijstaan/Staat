using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Staat.Helpers;

namespace Staat.Migrations
{
    public partial class InsertBaseData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            byte[] passwordSalt;
            byte[] passwordHash;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("StaatIsAwesome!"));
            }
            migrationBuilder.InsertData(table: "User", 
                columns: new[] {"FirstName", "LastName", "Email", "PasswordHash", "PasswordSalt", "CreatedAt", "UpdatedAt"}, 
                values: new object[,]
                {
                    {
                        "Admin",
                        "User",
                        "admin@staat.local",
                        passwordHash,
                        passwordSalt,
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString()
                    }
                });
            
            migrationBuilder.InsertData(table: "Status", 
                columns: new[] {"Name", "Description", "Color", "CreatedAt", "UpdatedAt"}, 
                values: new object[,]
                {
                    {
                        "Available",
                        "Service is operating normally",
                        "#50C878",
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString()
                    },
                    {
                        "Partial Outage",
                        "Some parts of the service may not be working properly",
                        "#FF5733",
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString()
                    },
                    {
                        "Major Outage",
                        "Service is completly offline",
                        "#C70039",
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString()
                    },
                });

            migrationBuilder.InsertData(table: "ServiceGroup",
                columns: new[] {"Name", "Description", "DefaultOpen", "CreatedAt", "UpdatedAt"},
                values: new object[,]
                {
                    {
                        "Default Group",
                        "This is a default service group",
                        true,
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString(),
                    }
                });

            migrationBuilder.InsertData(table: "Service",
                columns: new[]
                    {"Name", "Description", "Url", "StatusId", "GroupId", "ParentId", "CreatedAt", "UpdatedAt"},
                values: new object[,]
                {
                    {
                        "Good Service",
                        "This service is normal",
                        "https://example.com",
                        1,
                        1,
                        null,
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString(),
                    },
                    {
                        "Warning Service",
                        "This service is in a warning state",
                        "https://example.com",
                        2,
                        1,
                        null,
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString(),
                    },
                    {
                        "Bad Service",
                        "This service is out",
                        "https://example.com",
                        3,
                        1,
                        null,
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString(),
                    },
                    {
                        "Child Service",
                        "This service is a child of another service",
                        "https://example.com",
                        1,
                        1,
                        1,
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString(),
                    }
                });

            migrationBuilder.InsertData(table: "Incident",
                columns: new[] {"Title", "Description", "DescriptionHtml", "AuthorId", "ServiceId", "StartedAt", "EndedAt", "CreatedAt", "UpdatedAt"},
                values: new object[,]
                {
                    {
                        "First incident",
                        "Descriptions of incidents support **markdown**",
                        MarkdownHelper.ToHtml("Descriptions of incidents support **markdown**"),
                        1,
                        3,
                        DateTime.UtcNow.ToString(),
                        null,
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString(),
                    }
                });
            
            migrationBuilder.InsertData(table: "IncidentMessage",
                columns: new[] {"Message", "MessageHtml", "AuthorId", "StatusId", "IncidentId", "CreatedAt", "UpdatedAt"},
                values: new object[,]
                {
                    {
                        "Incident messages support **markdown** too!",
                        MarkdownHelper.ToHtml("Incident messages support **markdown** too!"),
                        1,
                        3,
                        1,
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString(),
                    }
                });
            
            migrationBuilder.InsertData(table: "Settings", 
                columns: new[] {"Key", "Value", "CreatedAt", "UpdatedAt"}, 
                values: new object[,]
                {
                    {
                        "backend.status.success",
                        "1",
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString(),
                    },
                    {
                        "backend.status.warning",
                        "2",
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString()
                    },
                    {
                        "backend.status.error",
                        "3",
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString(),
                    },
                    {
                        "backend.email.template.maintenance",
                        "This is some temp text",
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString()                    
                    },
                    {
                        "backend.email.template.maintenancemessage",
                        "This is some temp text",
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString()                    
                    },
                    {
                        "backend.email.template.incident",
                        "This is some temp text",
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString()                    
                    },
                    {
                        "backend.email.template.incidentmessage",
                        "This is some temp text",
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString()                    
                    },
                    {
                        "backend.monitor.cleanup",
                        "30",
                        DateTime.UtcNow.ToString(),
                        DateTime.UtcNow.ToString()
                    }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
