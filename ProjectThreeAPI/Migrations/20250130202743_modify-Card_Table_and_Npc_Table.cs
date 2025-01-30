using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectThreeAPI.Migrations
{
    /// <inheritdoc />
    public partial class modifyCard_Table_and_Npc_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_npcs_cards_CardId",
                table: "npcs");

            migrationBuilder.DropIndex(
                name: "IX_npcs_CardId",
                table: "npcs");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "npcs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "npcs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_npcs_CardId",
                table: "npcs",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_npcs_cards_CardId",
                table: "npcs",
                column: "CardId",
                principalTable: "cards",
                principalColumn: "Id");
        }
    }
}
