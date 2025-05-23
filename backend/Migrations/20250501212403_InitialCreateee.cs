using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Utilisateurs_UtilisateurId1",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UtilisateurId1",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "UtilisateurId1",
                table: "Notifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UtilisateurId1",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UtilisateurId1",
                table: "Notifications",
                column: "UtilisateurId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Utilisateurs_UtilisateurId1",
                table: "Notifications",
                column: "UtilisateurId1",
                principalTable: "Utilisateurs",
                principalColumn: "Id");
        }
    }
}
