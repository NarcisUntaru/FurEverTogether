using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurEver_Together.Migrations
{
    /// <inheritdoc />
    public partial class rerere : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Dogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Dogs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cats");
        }
    }
}
