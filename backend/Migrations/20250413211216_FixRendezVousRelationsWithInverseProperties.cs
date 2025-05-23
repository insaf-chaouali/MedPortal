using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_1.Migrations
{
    /// <inheritdoc />
    public partial class FixRendezVousRelationsWithInverseProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RendezVous_Utilisateurs_UtilisateurId",
                table: "RendezVous");

            migrationBuilder.DropForeignKey(
                name: "FK_RendezVous_Utilisateurs_UtilisateurId1",
                table: "RendezVous");

            migrationBuilder.DropIndex(
                name: "IX_RendezVous_UtilisateurId",
                table: "RendezVous");

            migrationBuilder.DropIndex(
                name: "IX_RendezVous_UtilisateurId1",
                table: "RendezVous");

            migrationBuilder.DropColumn(
                name: "UtilisateurId",
                table: "RendezVous");

            migrationBuilder.DropColumn(
                name: "UtilisateurId1",
                table: "RendezVous");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UtilisateurId",
                table: "RendezVous",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UtilisateurId1",
                table: "RendezVous",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RendezVous_UtilisateurId",
                table: "RendezVous",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_RendezVous_UtilisateurId1",
                table: "RendezVous",
                column: "UtilisateurId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RendezVous_Utilisateurs_UtilisateurId",
                table: "RendezVous",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RendezVous_Utilisateurs_UtilisateurId1",
                table: "RendezVous",
                column: "UtilisateurId1",
                principalTable: "Utilisateurs",
                principalColumn: "Id");
        }
    }
}
