using Microsoft.EntityFrameworkCore.Migrations;

namespace Notes.Logic.Migrations
{
    public partial class ChangeDeleteBehaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Notes_NoteId",
                table: "Shares");

            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Users_UserId",
                table: "Shares");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Notes_NoteId",
                table: "Shares",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Users_UserId",
                table: "Shares",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Notes_NoteId",
                table: "Shares");

            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Users_UserId",
                table: "Shares");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Notes_NoteId",
                table: "Shares",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Users_UserId",
                table: "Shares",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
