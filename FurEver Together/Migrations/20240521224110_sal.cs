using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurEver_Together.Migrations
{
    /// <inheritdoc />
    public partial class sal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Adoptions_AdoptionId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AdoptionId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "AdoptionId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AdoptionId",
                table: "AspNetUsers",
                column: "AdoptionId",
                unique: true,
                filter: "[AdoptionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Adoptions_AdoptionId",
                table: "AspNetUsers",
                column: "AdoptionId",
                principalTable: "Adoptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Adoptions_AdoptionId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AdoptionId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "AdoptionId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AdoptionId",
                table: "AspNetUsers",
                column: "AdoptionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Adoptions_AdoptionId",
                table: "AspNetUsers",
                column: "AdoptionId",
                principalTable: "Adoptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
