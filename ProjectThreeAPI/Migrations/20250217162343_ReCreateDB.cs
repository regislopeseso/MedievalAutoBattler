using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    /// <inheritdoc />
    public partial class ReCreateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Power = table.Column<int>(type: "int", nullable: false),
                    UpperHand = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cards", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "npcs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_npcs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlayersSaves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlayerLevel = table.Column<int>(type: "int", nullable: false),
                    Gold = table.Column<int>(type: "int", nullable: false),
                    CountMatches = table.Column<int>(type: "int", nullable: false),
                    CountVictories = table.Column<int>(type: "int", nullable: false),
                    CountDefeats = table.Column<int>(type: "int", nullable: false),
                    CountBoosters = table.Column<int>(type: "int", nullable: false),
                    AllCardsCollectedTrophy = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AllNpcsDefeatedTrophy = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersSaves", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NpcDeckEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    NpcId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "battles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SaveId = table.Column<int>(type: "int", nullable: false),
                    NpcId = table.Column<int>(type: "int", nullable: false),
                    Winner = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Results = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsFinished = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_battles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_battles_PlayersSaves_SaveId",
                        column: x => x.SaveId,
                        principalTable: "PlayersSaves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_battles_npcs_NpcId",
                        column: x => x.NpcId,
                        principalTable: "npcs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SaveId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decks_PlayersSaves_SaveId",
                        column: x => x.SaveId,
                        principalTable: "PlayersSaves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlayerCardEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SaveId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCardEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerCardEntries_PlayersSaves_SaveId",
                        column: x => x.SaveId,
                        principalTable: "PlayersSaves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerCardEntries_cards_CardId",
                        column: x => x.CardId,
                        principalTable: "cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlayerDeckEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SaveCardEntryId = table.Column<int>(type: "int", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDeckEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerDeckEntries_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerDeckEntries_PlayerCardEntries_SaveCardEntryId",
                        column: x => x.SaveCardEntryId,
                        principalTable: "PlayerCardEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_battles_NpcId",
                table: "battles",
                column: "NpcId");

            migrationBuilder.CreateIndex(
                name: "IX_battles_SaveId",
                table: "battles",
                column: "SaveId");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_SaveId",
                table: "Decks",
                column: "SaveId");

            migrationBuilder.CreateIndex(
                name: "IX_NpcDeckEntries_CardId",
                table: "NpcDeckEntries",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_NpcDeckEntries_NpcId",
                table: "NpcDeckEntries",
                column: "NpcId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCardEntries_CardId",
                table: "PlayerCardEntries",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCardEntries_SaveId",
                table: "PlayerCardEntries",
                column: "SaveId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDeckEntries_DeckId",
                table: "PlayerDeckEntries",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDeckEntries_SaveCardEntryId",
                table: "PlayerDeckEntries",
                column: "SaveCardEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "battles");

            migrationBuilder.DropTable(
                name: "NpcDeckEntries");

            migrationBuilder.DropTable(
                name: "PlayerDeckEntries");

            migrationBuilder.DropTable(
                name: "npcs");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "PlayerCardEntries");

            migrationBuilder.DropTable(
                name: "PlayersSaves");

            migrationBuilder.DropTable(
                name: "cards");
        }
    }
}
