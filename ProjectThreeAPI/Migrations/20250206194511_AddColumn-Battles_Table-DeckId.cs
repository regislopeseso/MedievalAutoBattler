using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnBattles_TableDeckId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerDeckId",
                table: "battles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_battles_PlayerDeckId",
                table: "battles",
                column: "PlayerDeckId");

            migrationBuilder.AddForeignKey(
                name: "FK_battles_Decks_PlayerDeckId",
                table: "battles",
                column: "PlayerDeckId",
                principalTable: "Decks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_battles_Decks_PlayerDeckId",
                table: "battles");

            migrationBuilder.DropIndex(
                name: "IX_battles_PlayerDeckId",
                table: "battles");

            migrationBuilder.DropColumn(
                name: "PlayerDeckId",
                table: "battles");
        }
    }
}
