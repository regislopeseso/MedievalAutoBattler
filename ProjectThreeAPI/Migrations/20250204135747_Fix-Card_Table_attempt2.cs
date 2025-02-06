using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    /// <inheritdoc />
    public partial class FixCard_Table_attempt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeckEntries_npcs_NpcId",
                table: "DeckEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_DeckEntries_saves_SaveId",
                table: "DeckEntries");

            migrationBuilder.AlterColumn<int>(
                name: "SaveId",
                table: "DeckEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "NpcId",
                table: "DeckEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DeckEntries_npcs_NpcId",
                table: "DeckEntries",
                column: "NpcId",
                principalTable: "npcs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeckEntries_saves_SaveId",
                table: "DeckEntries",
                column: "SaveId",
                principalTable: "saves",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeckEntries_npcs_NpcId",
                table: "DeckEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_DeckEntries_saves_SaveId",
                table: "DeckEntries");

            migrationBuilder.AlterColumn<int>(
                name: "SaveId",
                table: "DeckEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NpcId",
                table: "DeckEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeckEntries_npcs_NpcId",
                table: "DeckEntries",
                column: "NpcId",
                principalTable: "npcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DeckEntries_saves_SaveId",
                table: "DeckEntries",
                column: "SaveId",
                principalTable: "saves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
