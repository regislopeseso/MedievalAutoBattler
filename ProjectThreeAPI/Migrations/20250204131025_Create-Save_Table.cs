using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectThreeAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateSave_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaveId",
                table: "DeckEntries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Saves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gold = table.Column<int>(type: "int", nullable: false),
                    CountMatches = table.Column<int>(type: "int", nullable: false),
                    CountVictories = table.Column<int>(type: "int", nullable: false),
                    CountLosses = table.Column<int>(type: "int", nullable: false),
                    CountBoosters = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Saves", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DeckEntries_SaveId",
                table: "DeckEntries",
                column: "SaveId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeckEntries_Saves_SaveId",
                table: "DeckEntries",
                column: "SaveId",
                principalTable: "Saves",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeckEntries_Saves_SaveId",
                table: "DeckEntries");

            migrationBuilder.DropTable(
                name: "Saves");

            migrationBuilder.DropIndex(
                name: "IX_DeckEntries_SaveId",
                table: "DeckEntries");

            migrationBuilder.DropColumn(
                name: "SaveId",
                table: "DeckEntries");
        }
    }
}
