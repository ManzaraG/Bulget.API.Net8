using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDevisesReference : Migration
    {
        private const string FCfaId = "66666666-6666-6666-6666-666666666666";
        private const string EuroId = "77777777-7777-7777-7777-777777777777";
        private const string DollarUsId = "88888888-8888-8888-8888-888888888888";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devises", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Devises",
                columns: new[] { "Id", "Nom" },
                values: new object[,]
                {
                    { new Guid(FCfaId), "Franc CFA" },
                    { new Guid(EuroId), "Euro" },
                    { new Guid(DollarUsId), "Dollar US" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devises");
        }
    }
}
