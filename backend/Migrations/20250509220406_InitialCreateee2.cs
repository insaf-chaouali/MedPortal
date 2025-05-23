using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateee2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_DossiersMedicaux_DossierMedicalId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_DossiersMedicaux_Utilisateurs_UtilisateurId",
                table: "DossiersMedicaux");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DossiersMedicaux",
                table: "DossiersMedicaux");

            migrationBuilder.RenameTable(
                name: "DossiersMedicaux",
                newName: "DossierMedicals");

            migrationBuilder.RenameIndex(
                name: "IX_DossiersMedicaux_UtilisateurId",
                table: "DossierMedicals",
                newName: "IX_DossierMedicals_UtilisateurId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DossierMedicals",
                table: "DossierMedicals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_DossierMedicals_DossierMedicalId",
                table: "Consultations",
                column: "DossierMedicalId",
                principalTable: "DossierMedicals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DossierMedicals_Utilisateurs_UtilisateurId",
                table: "DossierMedicals",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_DossierMedicals_DossierMedicalId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_DossierMedicals_Utilisateurs_UtilisateurId",
                table: "DossierMedicals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DossierMedicals",
                table: "DossierMedicals");

            migrationBuilder.RenameTable(
                name: "DossierMedicals",
                newName: "DossiersMedicaux");

            migrationBuilder.RenameIndex(
                name: "IX_DossierMedicals_UtilisateurId",
                table: "DossiersMedicaux",
                newName: "IX_DossiersMedicaux_UtilisateurId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DossiersMedicaux",
                table: "DossiersMedicaux",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_DossiersMedicaux_DossierMedicalId",
                table: "Consultations",
                column: "DossierMedicalId",
                principalTable: "DossiersMedicaux",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DossiersMedicaux_Utilisateurs_UtilisateurId",
                table: "DossiersMedicaux",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
