using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_1.Migrations
{
    /// <inheritdoc />
    public partial class dossiermed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "DossierMedicals");

            migrationBuilder.DropColumn(
                name: "Antecedents",
                table: "DossierMedicals");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "DossierMedicals");

            migrationBuilder.DropColumn(
                name: "Traitements",
                table: "DossierMedicals");

            migrationBuilder.RenameTable(
                name: "DossierMedicals",
                newName: "DossierMedical");

            migrationBuilder.RenameIndex(
                name: "IX_DossierMedicals_UtilisateurId",
                table: "DossierMedical",
                newName: "IX_DossierMedical_UtilisateurId");

            migrationBuilder.AlterColumn<int>(
                name: "UtilisateurId",
                table: "DossierMedical",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Taille",
                table: "DossierMedical",
                type: "real",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "Poids",
                table: "DossierMedical",
                type: "real",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MedecinId",
                table: "DossierMedical",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "DossierMedical",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DossierMedical",
                table: "DossierMedical",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DossierMedical_MedecinId",
                table: "DossierMedical",
                column: "MedecinId");

            migrationBuilder.CreateIndex(
                name: "IX_DossierMedical_PatientId",
                table: "DossierMedical",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_DossierMedical_DossierMedicalId",
                table: "Consultations",
                column: "DossierMedicalId",
                principalTable: "DossierMedical",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DossierMedical_Utilisateurs_MedecinId",
                table: "DossierMedical",
                column: "MedecinId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DossierMedical_Utilisateurs_PatientId",
                table: "DossierMedical",
                column: "PatientId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DossierMedical_Utilisateurs_UtilisateurId",
                table: "DossierMedical",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_DossierMedical_DossierMedicalId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_DossierMedical_Utilisateurs_MedecinId",
                table: "DossierMedical");

            migrationBuilder.DropForeignKey(
                name: "FK_DossierMedical_Utilisateurs_PatientId",
                table: "DossierMedical");

            migrationBuilder.DropForeignKey(
                name: "FK_DossierMedical_Utilisateurs_UtilisateurId",
                table: "DossierMedical");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DossierMedical",
                table: "DossierMedical");

            migrationBuilder.DropIndex(
                name: "IX_DossierMedical_MedecinId",
                table: "DossierMedical");

            migrationBuilder.DropIndex(
                name: "IX_DossierMedical_PatientId",
                table: "DossierMedical");

            migrationBuilder.DropColumn(
                name: "MedecinId",
                table: "DossierMedical");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "DossierMedical");

            migrationBuilder.RenameTable(
                name: "DossierMedical",
                newName: "DossierMedicals");

            migrationBuilder.RenameIndex(
                name: "IX_DossierMedical_UtilisateurId",
                table: "DossierMedicals",
                newName: "IX_DossierMedicals_UtilisateurId");

            migrationBuilder.AlterColumn<int>(
                name: "UtilisateurId",
                table: "DossierMedicals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Taille",
                table: "DossierMedicals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Poids",
                table: "DossierMedicals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "DossierMedicals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Antecedents",
                table: "DossierMedicals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "DossierMedicals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Traitements",
                table: "DossierMedicals",
                type: "nvarchar(max)",
                nullable: true);

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
    }
}
