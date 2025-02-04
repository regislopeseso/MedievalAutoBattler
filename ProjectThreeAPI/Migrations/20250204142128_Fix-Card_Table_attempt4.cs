using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectThreeAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixCard_Table_attempt4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NpcDeckEntries_npcs_NpcId",
                table: "NpcDeckEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_SaveDeckEntries_saves_SaveId",
                table: "SaveDeckEntries");

            migrationBuilder.AlterColumn<int>(
                name: "SaveId",
                table: "SaveDeckEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NpcId",
                table: "NpcDeckEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NpcDeckEntries_npcs_NpcId",
                table: "NpcDeckEntries",
                column: "NpcId",
                principalTable: "npcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaveDeckEntries_saves_SaveId",
                table: "SaveDeckEntries",
                column: "SaveId",
                principalTable: "saves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NpcDeckEntries_npcs_NpcId",
                table: "NpcDeckEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_SaveDeckEntries_saves_SaveId",
                table: "SaveDeckEntries");

            migrationBuilder.AlterColumn<int>(
                name: "SaveId",
                table: "SaveDeckEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "NpcId",
                table: "NpcDeckEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_NpcDeckEntries_npcs_NpcId",
                table: "NpcDeckEntries",
                column: "NpcId",
                principalTable: "npcs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaveDeckEntries_saves_SaveId",
                table: "SaveDeckEntries",
                column: "SaveId",
                principalTable: "saves",
                principalColumn: "Id");
        }
    }
}
