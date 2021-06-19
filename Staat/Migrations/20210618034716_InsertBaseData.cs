using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
            
            migrationBuilder.InsertData(table: "Settings", 
                columns: new[] {"Key", "Value", "CreatedAt", "UpdatedAt"}, 
                values: new object[,]
                {
                    {
                        "status.warning",
                        "2",
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
