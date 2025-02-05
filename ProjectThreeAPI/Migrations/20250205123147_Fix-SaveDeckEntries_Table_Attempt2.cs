using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectThreeAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixSaveDeckEntries_Table_Attempt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_saves_SaveId",
                table: "Decks");

            migrationBuilder.DropForeignKey(
                name: "FK_SaveDeckEntries_saves_SaveId",
                table: "SaveDeckEntries");

            migrationBuilder.DropIndex(
                name: "IX_SaveDeckEntries_SaveId",
                table: "SaveDeckEntries");

            migrationBuilder.DropColumn(
                name: "SaveId",
                table: "SaveDeckEntries");

            migrationBuilder.AlterColumn<int>(
                name: "SaveId",
                table: "Decks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_saves_SaveId",
                table: "Decks",
                column: "SaveId",
                principalTable: "saves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_saves_SaveId",
                table: "Decks");

            migrationBuilder.AddColumn<int>(
                name: "SaveId",
                table: "SaveDeckEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SaveId",
                table: "Decks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_SaveDeckEntries_SaveId",
                table: "SaveDeckEntries",
                column: "SaveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_saves_SaveId",
                table: "Decks",
                column: "SaveId",
                principalTable: "saves",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaveDeckEntries_saves_SaveId",
                table: "SaveDeckEntries",
                column: "SaveId",
                principalTable: "saves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
