using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurEver_Together.Migrations
{
    /// <inheritdoc />
    public partial class UserAddedToAdoptionn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adoptions_Pets_PetId",
                table: "Adoptions");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Adoptions_AdoptionId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AdoptionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdoptionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AdoptionStory",
                table: "Adoptions");

            migrationBuilder.DropColumn(
                name: "FreeTransportation",
                table: "Adoptions");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Adoptions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Adoptions_UserId",
                table: "Adoptions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adoptions_AspNetUsers_UserId",
                table: "Adoptions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Adoptions_Pets_PetId",
                table: "Adoptions",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adoptions_AspNetUsers_UserId",
                table: "Adoptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Adoptions_Pets_PetId",
                table: "Adoptions");

            migrationBuilder.DropIndex(
                name: "IX_Adoptions_UserId",
                table: "Adoptions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Adoptions");

            migrationBuilder.AddColumn<int>(
                name: "AdoptionId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdoptionStory",
                table: "Adoptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "FreeTransportation",
                table: "Adoptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AdoptionId",
                table: "AspNetUsers",
                column: "AdoptionId",
                unique: true,
                filter: "[AdoptionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Adoptions_Pets_PetId",
                table: "Adoptions",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Adoptions_AdoptionId",
                table: "AspNetUsers",
                column: "AdoptionId",
                principalTable: "Adoptions",
                principalColumn: "Id");
        }
    }
}
