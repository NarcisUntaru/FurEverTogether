using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurEver_Together.Migrations
{
    /// <inheritdoc />
    public partial class AdoptionRequestDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "Adoptions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "Adoptions");
        }
    }
}
