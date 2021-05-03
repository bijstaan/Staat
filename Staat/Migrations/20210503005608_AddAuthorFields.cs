using Microsoft.EntityFrameworkCore.Migrations;

namespace Staat.Migrations
{
    public partial class AddAuthorFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "IncidentMessage",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Incident",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncidentMessage_AuthorId",
                table: "IncidentMessage",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_AuthorId",
                table: "Incident",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incident_User_AuthorId",
                table: "Incident",
                column: "AuthorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IncidentMessage_User_AuthorId",
                table: "IncidentMessage",
                column: "AuthorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incident_User_AuthorId",
                table: "Incident");

            migrationBuilder.DropForeignKey(
                name: "FK_IncidentMessage_User_AuthorId",
                table: "IncidentMessage");

            migrationBuilder.DropIndex(
                name: "IX_IncidentMessage_AuthorId",
                table: "IncidentMessage");

            migrationBuilder.DropIndex(
                name: "IX_Incident_AuthorId",
                table: "Incident");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "IncidentMessage");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Incident");
        }
    }
}
