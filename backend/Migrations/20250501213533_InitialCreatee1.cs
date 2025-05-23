using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatee1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "DossiersMedicaux");

            migrationBuilder.DropColumn(
                name: "HistoriqueMedical",
                table: "DossiersMedicaux");

            migrationBuilder.DropColumn(
                name: "IdentifiantFiscale",
                table: "DossiersMedicaux");

            migrationBuilder.DropColumn(
                name: "NumeroDossier",
                table: "DossiersMedicaux");

            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "DossiersMedicaux",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Antecedents",
                table: "DossiersMedicaux",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupeSanguin",
                table: "DossiersMedicaux",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "DossiersMedicaux",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Poids",
                table: "DossiersMedicaux",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Taille",
                table: "DossiersMedicaux",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Traitements",
                table: "DossiersMedicaux",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "DossiersMedicaux");

            migrationBuilder.DropColumn(
                name: "Antecedents",
                table: "DossiersMedicaux");

            migrationBuilder.DropColumn(
                name: "GroupeSanguin",
                table: "DossiersMedicaux");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "DossiersMedicaux");

            migrationBuilder.DropColumn(
                name: "Poids",
                table: "DossiersMedicaux");

            migrationBuilder.DropColumn(
                name: "Taille",
                table: "DossiersMedicaux");

            migrationBuilder.DropColumn(
                name: "Traitements",
                table: "DossiersMedicaux");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "DossiersMedicaux",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "HistoriqueMedical",
                table: "DossiersMedicaux",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentifiantFiscale",
                table: "DossiersMedicaux",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroDossier",
                table: "DossiersMedicaux",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
