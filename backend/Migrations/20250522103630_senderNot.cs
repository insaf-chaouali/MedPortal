using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_1.Migrations
{
    /// <inheritdoc />
    public partial class senderNot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Utilisateurs_MedecinId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Utilisateurs_PatientId",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Notification",
                newName: "Sender");

            migrationBuilder.RenameColumn(
                name: "MedecinId",
                table: "Notification",
                newName: "Reciver");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_PatientId",
                table: "Notification",
                newName: "IX_Notification_Sender");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_MedecinId",
                table: "Notification",
                newName: "IX_Notification_Reciver");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Utilisateurs_Reciver",
                table: "Notification",
                column: "Reciver",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Utilisateurs_Sender",
                table: "Notification",
                column: "Sender",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Utilisateurs_Reciver",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Utilisateurs_Sender",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "Sender",
                table: "Notification",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "Reciver",
                table: "Notification",
                newName: "MedecinId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_Sender",
                table: "Notification",
                newName: "IX_Notification_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_Reciver",
                table: "Notification",
                newName: "IX_Notification_MedecinId");

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
        }
    }
}
