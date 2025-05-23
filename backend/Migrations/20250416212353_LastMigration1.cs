using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_1.Migrations
{
    /// <inheritdoc />
    public partial class LastMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Traitement_Consultations_ConsultationId",
                table: "Traitement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Traitement",
                table: "Traitement");

            migrationBuilder.RenameTable(
                name: "Traitement",
                newName: "Traitements");

            migrationBuilder.RenameIndex(
                name: "IX_Traitement_ConsultationId",
                table: "Traitements",
                newName: "IX_Traitements_ConsultationId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RendezVous",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Etat",
                table: "RendezVous",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Traitements_Consultations_ConsultationId",
                table: "Traitements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Traitements",
                table: "Traitements");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RendezVous");

            migrationBuilder.DropColumn(
                name: "Etat",
                table: "RendezVous");

            migrationBuilder.RenameTable(
                name: "Traitements",
                newName: "Traitement");

            migrationBuilder.RenameIndex(
                name: "IX_Traitements_ConsultationId",
                table: "Traitement",
                newName: "IX_Traitement_ConsultationId");

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
    }
}
