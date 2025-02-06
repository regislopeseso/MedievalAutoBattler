using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedievalAutoBattler.Migrations
{
    /// <inheritdoc />
    public partial class BattleTableimproved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_battles_npcs_NpcId",
                table: "battles");

            migrationBuilder.AlterColumn<int>(
                name: "NpcId",
                table: "battles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SaveId",
                table: "battles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_battles_SaveId",
                table: "battles",
                column: "SaveId");

            migrationBuilder.AddForeignKey(
                name: "FK_battles_npcs_NpcId",
                table: "battles",
                column: "NpcId",
                principalTable: "npcs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_battles_saves_SaveId",
                table: "battles",
                column: "SaveId",
                principalTable: "saves",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_battles_npcs_NpcId",
                table: "battles");

            migrationBuilder.DropForeignKey(
                name: "FK_battles_saves_SaveId",
                table: "battles");

            migrationBuilder.DropIndex(
                name: "IX_battles_SaveId",
                table: "battles");

            migrationBuilder.DropColumn(
                name: "SaveId",
                table: "battles");

            migrationBuilder.AlterColumn<int>(
                name: "NpcId",
                table: "battles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_battles_npcs_NpcId",
                table: "battles",
                column: "NpcId",
                principalTable: "npcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
