using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOfLife.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Board_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Board",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Board");
        }
    }
}
