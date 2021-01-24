using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreStatus.Data.Migrations
{
    public partial class AddStatusIsErrorDefaultColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsErrorDefault",
                table: "Statuses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsErrorDefault",
                table: "Statuses");
        }
    }
}
