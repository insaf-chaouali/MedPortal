using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_1.Migrations
{
    /// <inheritdoc />
    public partial class NOTIFICATION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Utilisateurs_UtilisateurId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UtilisateurId",
                table: "Notification",
                newName: "IX_Notification_UtilisateurId");

            migrationBuilder.AlterColumn<int>(
                name: "UtilisateurId",
                table: "Notification",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MedecinId",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Notification",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_MedecinId",
                table: "Notification",
                column: "MedecinId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_PatientId",
                table: "Notification",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Utilisateurs_MedecinId",
                table: "Notification",
                column: "MedecinId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Utilisateurs_PatientId",
                table: "Notification",
                column: "PatientId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Utilisateurs_UtilisateurId",
                table: "Notification",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Utilisateurs_MedecinId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Utilisateurs_PatientId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Utilisateurs_UtilisateurId",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_MedecinId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_PatientId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "MedecinId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Notification");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UtilisateurId",
                table: "Notifications",
                newName: "IX_Notifications_UtilisateurId");

            migrationBuilder.AlterColumn<int>(
                name: "UtilisateurId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Utilisateurs_UtilisateurId",
                table: "Notifications",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
