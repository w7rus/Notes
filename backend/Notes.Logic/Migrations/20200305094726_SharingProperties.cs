using Microsoft.EntityFrameworkCore.Migrations;

namespace NotesWebAPI.Migrations
{
    public partial class SharingProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Sharing",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    NoteId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sharing", x => new { x.NoteId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Sharing_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sharing_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsSystem", "Password", "Username" },
                values: new object[] { 1, true, null, "public" });

            migrationBuilder.CreateIndex(
                name: "IX_Sharing_UserId",
                table: "Sharing",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sharing");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "Users");
        }
    }
}
