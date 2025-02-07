using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    /// <inheritdoc />
    public partial class RecreateBattles_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "battles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SaveId = table.Column<int>(type: "int", nullable: false),
                    PlayerDeckId = table.Column<int>(type: "int", nullable: true),
                    NpcId = table.Column<int>(type: "int", nullable: true),
                    Winner = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_battles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_battles_Decks_PlayerDeckId",
                        column: x => x.PlayerDeckId,
                        principalTable: "Decks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_battles_npcs_NpcId",
                        column: x => x.NpcId,
                        principalTable: "npcs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_battles_saves_SaveId",
                        column: x => x.SaveId,
                        principalTable: "saves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_battles_NpcId",
                table: "battles",
                column: "NpcId");

            migrationBuilder.CreateIndex(
                name: "IX_battles_PlayerDeckId",
                table: "battles",
                column: "PlayerDeckId");

            migrationBuilder.CreateIndex(
                name: "IX_battles_SaveId",
                table: "battles",
                column: "SaveId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "battles");
        }
    }
}
