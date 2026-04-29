using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDproject.Migrations
{
    /// <inheritdoc />
    public partial class addNewColumnInJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Jobs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Jobs");
        }
    }
}
