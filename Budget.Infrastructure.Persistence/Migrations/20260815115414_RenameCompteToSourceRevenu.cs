using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameCompteToSourceRevenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Comptes_CompteId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Comptes_Utilisateurs_UtilisateurId",
                table: "Comptes");

            migrationBuilder.RenameTable(
                name: "Comptes",
                newName: "SourcesRevenu");

            migrationBuilder.RenameColumn(
                name: "CompteId",
                table: "Transactions",
                newName: "SourceRevenuId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CompteId_Date",
                table: "Transactions",
                newName: "IX_Transactions_SourceRevenuId_Date");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CompteId",
                table: "Transactions",
                newName: "IX_Transactions_SourceRevenuId");

            migrationBuilder.RenameIndex(
                name: "IX_Comptes_UtilisateurId",
                table: "SourcesRevenu",
                newName: "IX_SourcesRevenu_UtilisateurId");

            migrationBuilder.Sql("EXEC sp_rename N'dbo.PK_Comptes', N'PK_SourcesRevenu', N'OBJECT';");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "SourcesRevenu",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Autre");

            migrationBuilder.AddColumn<bool>(
                name: "EstActif",
                table: "SourcesRevenu",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SourcesRevenu_Utilisateurs_UtilisateurId",
                table: "SourcesRevenu",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_SourcesRevenu_SourceRevenuId",
                table: "Transactions",
                column: "SourceRevenuId",
                principalTable: "SourcesRevenu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_SourcesRevenu_SourceRevenuId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SourcesRevenu_Utilisateurs_UtilisateurId",
                table: "SourcesRevenu");

            migrationBuilder.DropColumn(
                name: "EstActif",
                table: "SourcesRevenu");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "SourcesRevenu");

            migrationBuilder.RenameIndex(
                name: "IX_SourcesRevenu_UtilisateurId",
                table: "SourcesRevenu",
                newName: "IX_Comptes_UtilisateurId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SourceRevenuId",
                table: "Transactions",
                newName: "IX_Transactions_CompteId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SourceRevenuId_Date",
                table: "Transactions",
                newName: "IX_Transactions_CompteId_Date");

            migrationBuilder.RenameColumn(
                name: "SourceRevenuId",
                table: "Transactions",
                newName: "CompteId");

            migrationBuilder.Sql("EXEC sp_rename N'dbo.PK_SourcesRevenu', N'PK_Comptes', N'OBJECT';");

            migrationBuilder.RenameTable(
                name: "SourcesRevenu",
                newName: "Comptes");

            migrationBuilder.AddForeignKey(
                name: "FK_Comptes_Utilisateurs_UtilisateurId",
                table: "Comptes",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Comptes_CompteId",
                table: "Transactions",
                column: "CompteId",
                principalTable: "Comptes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
