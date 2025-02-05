using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectThreeAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixSaveDeckEntries_Table_Attempt3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaveDeckEntries_Decks_DeckId",
                table: "SaveDeckEntries");

            migrationBuilder.AlterColumn<int>(
                name: "DeckId",
                table: "SaveDeckEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SaveDeckEntries_Decks_DeckId",
                table: "SaveDeckEntries",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaveDeckEntries_Decks_DeckId",
                table: "SaveDeckEntries");

            migrationBuilder.AlterColumn<int>(
                name: "DeckId",
                table: "SaveDeckEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SaveDeckEntries_Decks_DeckId",
                table: "SaveDeckEntries",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "Id");
        }
    }
}
