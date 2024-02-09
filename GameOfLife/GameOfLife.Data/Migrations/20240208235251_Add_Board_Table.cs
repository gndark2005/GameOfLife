using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameOfLife.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_Board_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Board",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rows = table.Column<int>(type: "integer", nullable: false),
                    Columns = table.Column<int>(type: "integer", nullable: false),
                    AliveCellsJson = table.Column<string>(type: "jsonb", nullable: false),
                    CreationDatetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdateDatetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CurrentGeneration = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Board");
        }
    }
}
