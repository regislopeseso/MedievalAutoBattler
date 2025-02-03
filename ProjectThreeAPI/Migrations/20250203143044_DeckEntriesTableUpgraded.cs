using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectThreeAPI.Migrations
{
    /// <inheritdoc />
    public partial class DeckEntriesTableUpgraded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeckEntries_npcs_NpcId",
                table: "DeckEntries");

            migrationBuilder.AlterColumn<int>(
                name: "NpcId",
                table: "DeckEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DeckEntries",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_DeckEntries_npcs_NpcId",
                table: "DeckEntries",
                column: "NpcId",
                principalTable: "npcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeckEntries_npcs_NpcId",
                table: "DeckEntries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DeckEntries");

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
        }
    }
}
