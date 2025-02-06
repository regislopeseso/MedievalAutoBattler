using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    /// <inheritdoc />
    public partial class NPC_table_corrected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Npcs_NpcId",
                table: "Cards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Npcs",
                table: "Npcs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cards",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_NpcId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "NpcId",
                table: "Cards");

            migrationBuilder.RenameTable(
                name: "Npcs",
                newName: "npcs");

            migrationBuilder.RenameTable(
                name: "Cards",
                newName: "cards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_npcs",
                table: "npcs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cards",
                table: "cards",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CardNpc",
                columns: table => new
                {
                    HandId = table.Column<int>(type: "int", nullable: false),
                    NpcsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardNpc", x => new { x.HandId, x.NpcsId });
                    table.ForeignKey(
                        name: "FK_CardNpc_cards_HandId",
                        column: x => x.HandId,
                        principalTable: "cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardNpc_npcs_NpcsId",
                        column: x => x.NpcsId,
                        principalTable: "npcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CardNpc_NpcsId",
                table: "CardNpc",
                column: "NpcsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardNpc");

            migrationBuilder.DropPrimaryKey(
                name: "PK_npcs",
                table: "npcs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cards",
                table: "cards");

            migrationBuilder.RenameTable(
                name: "npcs",
                newName: "Npcs");

            migrationBuilder.RenameTable(
                name: "cards",
                newName: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "NpcId",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Npcs",
                table: "Npcs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cards",
                table: "Cards",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_NpcId",
                table: "Cards",
                column: "NpcId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Npcs_NpcId",
                table: "Cards",
                column: "NpcId",
                principalTable: "Npcs",
                principalColumn: "Id");
        }
    }
}
