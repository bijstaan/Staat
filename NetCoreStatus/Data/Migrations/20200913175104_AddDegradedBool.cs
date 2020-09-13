using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreStatus.Data.Migrations
{
    public partial class AddDegradedBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDegraded",
                table: "Statuses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDegraded",
                table: "Statuses");
        }
    }
}
