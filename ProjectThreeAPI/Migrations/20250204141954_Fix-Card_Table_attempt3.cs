using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    /// <inheritdoc />
    public partial class FixCard_Table_attempt3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckEntries");

            migrationBuilder.CreateTable(
                name: "NpcDeckEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    NpcId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NpcDeckEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NpcDeckEntries_cards_CardId",
                        column: x => x.CardId,
                        principalTable: "cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NpcDeckEntries_npcs_NpcId",
                        column: x => x.NpcId,
                        principalTable: "npcs",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SaveDeckEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    SaveId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveDeckEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaveDeckEntries_cards_CardId",
                        column: x => x.CardId,
                        principalTable: "cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaveDeckEntries_saves_SaveId",
                        column: x => x.SaveId,
                        principalTable: "saves",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_NpcDeckEntries_CardId",
                table: "NpcDeckEntries",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_NpcDeckEntries_NpcId",
                table: "NpcDeckEntries",
                column: "NpcId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveDeckEntries_CardId",
                table: "SaveDeckEntries",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveDeckEntries_SaveId",
                table: "SaveDeckEntries",
                column: "SaveId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NpcDeckEntries");

            migrationBuilder.DropTable(
                name: "SaveDeckEntries");

            migrationBuilder.CreateTable(
                name: "DeckEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    NpcId = table.Column<int>(type: "int", nullable: true),
                    SaveId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_DeckEntries_saves_SaveId",
                        column: x => x.SaveId,
                        principalTable: "saves",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DeckEntries_CardId",
                table: "DeckEntries",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckEntries_NpcId",
                table: "DeckEntries",
                column: "NpcId");

            migrationBuilder.CreateIndex(
                name: "IX_DeckEntries_SaveId",
                table: "DeckEntries",
                column: "SaveId");
        }
    }
}
