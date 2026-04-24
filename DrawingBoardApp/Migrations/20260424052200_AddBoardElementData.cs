using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrawingBoardApp.Migrations
{
    /// <inheritdoc />
    public partial class AddBoardElementData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "BoardElements",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "BoardElements");
        }
    }
}
