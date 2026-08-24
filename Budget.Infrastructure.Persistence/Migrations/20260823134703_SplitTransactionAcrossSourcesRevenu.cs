using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SplitTransactionAcrossSourcesRevenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RepartitionsSourceRevenu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceRevenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepartitionsSourceRevenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepartitionsSourceRevenu_SourcesRevenu_SourceRevenuId",
                        column: x => x.SourceRevenuId,
                        principalTable: "SourcesRevenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RepartitionsSourceRevenu_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("""
                INSERT INTO RepartitionsSourceRevenu (Id, TransactionId, SourceRevenuId, Montant)
                SELECT NEWID(), Id, SourceRevenuId, Montant
                FROM Transactions;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_SourcesRevenu_SourceRevenuId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SourceRevenuId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SourceRevenuId_Date",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Montant",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SourceRevenuId",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Date",
                table: "Transactions",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_RepartitionsSourceRevenu_SourceRevenuId",
                table: "RepartitionsSourceRevenu",
                column: "SourceRevenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RepartitionsSourceRevenu_TransactionId",
                table: "RepartitionsSourceRevenu",
                column: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_Date",
                table: "Transactions");

            migrationBuilder.AddColumn<decimal>(
                name: "Montant",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceRevenuId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.Sql("""
                UPDATE t
                SET t.Montant = agg.MontantTotal,
                    t.SourceRevenuId = agg.SourceRevenuPrincipale
                FROM Transactions t
                CROSS APPLY (
                    SELECT
                        SUM(r.Montant) AS MontantTotal,
                        (SELECT TOP 1 r2.SourceRevenuId
                         FROM RepartitionsSourceRevenu r2
                         WHERE r2.TransactionId = t.Id
                         ORDER BY r2.Montant DESC) AS SourceRevenuPrincipale
                    FROM RepartitionsSourceRevenu r
                    WHERE r.TransactionId = t.Id
                ) agg;
                """);

            migrationBuilder.DropTable(
                name: "RepartitionsSourceRevenu");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceRevenuId",
                table: "Transactions",
                column: "SourceRevenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SourceRevenuId_Date",
                table: "Transactions",
                columns: new[] { "SourceRevenuId", "Date" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_SourcesRevenu_SourceRevenuId",
                table: "Transactions",
                column: "SourceRevenuId",
                principalTable: "SourcesRevenu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
