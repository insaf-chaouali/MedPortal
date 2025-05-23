using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_1.Migrations
{
    /// <inheritdoc />
    public partial class dossiermedical : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "DossierMedical",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Antecedents",
                table: "DossierMedical",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "DossierMedical",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Traitements",
                table: "DossierMedical",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "DossierMedical");

            migrationBuilder.DropColumn(
                name: "Antecedents",
                table: "DossierMedical");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "DossierMedical");

            migrationBuilder.DropColumn(
                name: "Traitements",
                table: "DossierMedical");
        }
    }
}
