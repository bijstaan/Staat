using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreStatus.Data.Migrations
{
    public partial class AddIncidentActiveColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Incidents",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Incidents");
        }
    }
}
