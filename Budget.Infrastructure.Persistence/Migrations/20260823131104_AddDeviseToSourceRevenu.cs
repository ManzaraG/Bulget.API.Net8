using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviseToSourceRevenu : Migration
    {
        private const string FCfaId = "66666666-6666-6666-6666-666666666666";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeviseId",
                table: "SourcesRevenu",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid(FCfaId));

            migrationBuilder.CreateIndex(
                name: "IX_SourcesRevenu_DeviseId",
                table: "SourcesRevenu",
                column: "DeviseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SourcesRevenu_Devises_DeviseId",
                table: "SourcesRevenu",
                column: "DeviseId",
                principalTable: "Devises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourcesRevenu_Devises_DeviseId",
                table: "SourcesRevenu");

            migrationBuilder.DropIndex(
                name: "IX_SourcesRevenu_DeviseId",
                table: "SourcesRevenu");

            migrationBuilder.DropColumn(
                name: "DeviseId",
                table: "SourcesRevenu");
        }
    }
}
