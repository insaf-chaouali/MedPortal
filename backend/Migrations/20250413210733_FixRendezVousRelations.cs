using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_1.Migrations
{
    /// <inheritdoc />
    public partial class FixRendezVousRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Traitements_Consultations_ConsultationId",
                table: "Traitements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Traitements",
                table: "Traitements");

            migrationBuilder.RenameTable(
                name: "Traitements",
                newName: "Traitement");

            migrationBuilder.RenameColumn(
                name: "Prenom",
                table: "Utilisateurs",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "Utilisateurs",
                newName: "Phone");

            migrationBuilder.RenameIndex(
                name: "IX_Traitements_ConsultationId",
                table: "Traitement",
                newName: "IX_Traitement_ConsultationId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Utilisateurs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Traitement",
                table: "Traitement",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Traitement_Consultations_ConsultationId",
                table: "Traitement",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Traitement_Consultations_ConsultationId",
                table: "Traitement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Traitement",
                table: "Traitement");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Utilisateurs");

            migrationBuilder.RenameTable(
                name: "Traitement",
                newName: "Traitements");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Utilisateurs",
                newName: "Prenom");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Utilisateurs",
                newName: "Nom");

            migrationBuilder.RenameIndex(
                name: "IX_Traitement_ConsultationId",
                table: "Traitements",
                newName: "IX_Traitements_ConsultationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Traitements",
                table: "Traitements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Traitements_Consultations_ConsultationId",
                table: "Traitements",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
