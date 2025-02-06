using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    /// <inheritdoc />
    public partial class Create_DeckEntryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardNpc");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "npcs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeckEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    NpcId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeckEntries_cards_CardId",
                        column: x => x.CardId,
                        principalTable: "cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckEntries_npcs_NpcId",
                        column: x => x.NpcId,
                        principalTable: "npcs",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_npcs_CardId",
                table: "npcs",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckEntries_CardId",
                table: "DeckEntries",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckEntries_NpcId",
                table: "DeckEntries",
                column: "NpcId");

            migrationBuilder.AddForeignKey(
                name: "FK_npcs_cards_CardId",
                table: "npcs",
                column: "CardId",
                principalTable: "cards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_npcs_cards_CardId",
                table: "npcs");

            migrationBuilder.DropTable(
                name: "DeckEntries");

            migrationBuilder.DropIndex(
                name: "IX_npcs_CardId",
                table: "npcs");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "npcs");

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
    }
}
