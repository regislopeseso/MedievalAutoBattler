using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectThreeAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixCard_Table_attempt5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CountLosses",
                table: "saves",
                newName: "PlayerLevel");

            migrationBuilder.AddColumn<int>(
                name: "CountDefeats",
                table: "saves",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountDefeats",
                table: "saves");

            migrationBuilder.RenameColumn(
                name: "PlayerLevel",
                table: "saves",
                newName: "CountLosses");
        }
    }
}
