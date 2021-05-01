using Microsoft.EntityFrameworkCore.Migrations;

namespace Staat.Migrations
{
    public partial class AddDefaultOpenServiceGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DefaultOpen",
                table: "ServiceGroup",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultOpen",
                table: "ServiceGroup");
        }
    }
}
