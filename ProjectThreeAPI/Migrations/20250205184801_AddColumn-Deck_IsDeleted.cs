using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnDeck_IsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Decks",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Decks");
        }
    }
}
