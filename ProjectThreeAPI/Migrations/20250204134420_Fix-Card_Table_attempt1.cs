using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectThreeAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixCard_Table_attempt1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeckEntries_Saves_SaveId",
                table: "DeckEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Saves",
                table: "Saves");

            migrationBuilder.RenameTable(
                name: "Saves",
                newName: "saves");

            migrationBuilder.AlterColumn<int>(
                name: "SaveId",
                table: "DeckEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_saves",
                table: "saves",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeckEntries_saves_SaveId",
                table: "DeckEntries",
                column: "SaveId",
                principalTable: "saves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeckEntries_saves_SaveId",
                table: "DeckEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_saves",
                table: "saves");

            migrationBuilder.RenameTable(
                name: "saves",
                newName: "Saves");

            migrationBuilder.AlterColumn<int>(
                name: "SaveId",
                table: "DeckEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Saves",
                table: "Saves",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeckEntries_Saves_SaveId",
                table: "DeckEntries",
                column: "SaveId",
                principalTable: "Saves",
                principalColumn: "Id");
        }
    }
}
