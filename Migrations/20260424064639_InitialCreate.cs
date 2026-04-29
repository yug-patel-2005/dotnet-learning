using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDproject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolId = table.Column<int>(type: "int", nullable: true),
                    PhotographerId = table.Column<int>(type: "int", nullable: true),
                    ShootDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShootCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShootId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");
        }
    }
}
