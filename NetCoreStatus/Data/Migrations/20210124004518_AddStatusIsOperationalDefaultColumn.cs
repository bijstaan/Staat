using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreStatus.Data.Migrations
{
    public partial class AddStatusIsOperationalDefaultColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOperationalDefault",
                table: "Statuses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOperationalDefault",
                table: "Statuses");
        }
    }
}
