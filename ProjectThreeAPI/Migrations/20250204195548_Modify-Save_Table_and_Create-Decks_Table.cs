using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    /// <inheritdoc />
    public partial class ModifySave_Table_and_CreateDecks_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeckId",
                table: "SaveDeckEntries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SaveId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decks_saves_SaveId",
                        column: x => x.SaveId,
                        principalTable: "saves",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SaveDeckEntries_DeckId",
                table: "SaveDeckEntries",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_SaveId",
                table: "Decks",
                column: "SaveId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaveDeckEntries_Decks_DeckId",
                table: "SaveDeckEntries",
                column: "DeckId",
                principalTable: "Decks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaveDeckEntries_Decks_DeckId",
                table: "SaveDeckEntries");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropIndex(
                name: "IX_SaveDeckEntries_DeckId",
                table: "SaveDeckEntries");

            migrationBuilder.DropColumn(
                name: "DeckId",
                table: "SaveDeckEntries");
        }
    }
}
