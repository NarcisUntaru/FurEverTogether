using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FurEver_Together.Migrations
{
    /// <inheritdoc />
    public partial class VolunteerModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AgreementToTerms",
                table: "Volunteers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HoursPerWeek",
                table: "Volunteers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsOver18",
                table: "Volunteers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "PreviousExperience",
                table: "Volunteers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TransportationAvailable",
                table: "Volunteers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgreementToTerms",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "HoursPerWeek",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "IsOver18",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "PreviousExperience",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "TransportationAvailable",
                table: "Volunteers");
        }
    }
}
