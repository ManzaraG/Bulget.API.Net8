using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TypeSourceRevenuAsReference : Migration
    {
        private const string SalaireId = "11111111-1111-1111-1111-111111111111";
        private const string FreelanceId = "22222222-2222-2222-2222-222222222222";
        private const string InvestissementId = "33333333-3333-3333-3333-333333333333";
        private const string LocationId = "44444444-4444-4444-4444-444444444444";
        private const string AutreId = "55555555-5555-5555-5555-555555555555";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TypesSourceRevenu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesSourceRevenu", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TypesSourceRevenu",
                columns: new[] { "Id", "Nom" },
                values: new object[,]
                {
                    { new Guid(SalaireId), "Salaire" },
                    { new Guid(FreelanceId), "Freelance" },
                    { new Guid(InvestissementId), "Investissement" },
                    { new Guid(LocationId), "Location" },
                    { new Guid(AutreId), "Autre" },
                });

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "SourcesRevenu",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql($"""
                UPDATE SourcesRevenu
                SET TypeId = CASE Type
                    WHEN 'Salaire' THEN '{SalaireId}'
                    WHEN 'Freelance' THEN '{FreelanceId}'
                    WHEN 'Investissement' THEN '{InvestissementId}'
                    WHEN 'Location' THEN '{LocationId}'
                    ELSE '{AutreId}'
                END;
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "TypeId",
                table: "SourcesRevenu",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "Type",
                table: "SourcesRevenu");

            migrationBuilder.CreateIndex(
                name: "IX_SourcesRevenu_TypeId",
                table: "SourcesRevenu",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SourcesRevenu_TypesSourceRevenu_TypeId",
                table: "SourcesRevenu",
                column: "TypeId",
                principalTable: "TypesSourceRevenu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SourcesRevenu_TypesSourceRevenu_TypeId",
                table: "SourcesRevenu");

            migrationBuilder.DropIndex(
                name: "IX_SourcesRevenu_TypeId",
                table: "SourcesRevenu");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "SourcesRevenu",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE s
                SET s.Type = t.Nom
                FROM SourcesRevenu s
                INNER JOIN TypesSourceRevenu t ON t.Id = s.TypeId;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "SourcesRevenu",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Autre",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "SourcesRevenu");

            migrationBuilder.DropTable(
                name: "TypesSourceRevenu");
        }
    }
}
