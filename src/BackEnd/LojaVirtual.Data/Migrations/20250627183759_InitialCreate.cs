using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LojaVirtual.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorito");

            migrationBuilder.CreateTable(
                name: "Favoritos",
                columns: table => new
                {
                    ClienteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClienteId1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favoritos", x => new { x.ClienteId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_Favoritos_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favoritos_Cliente_ClienteId1",
                        column: x => x.ClienteId1,
                        principalTable: "Cliente",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favoritos_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favoritos_ClienteId1",
                table: "Favoritos",
                column: "ClienteId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritos_ProdutoId",
                table: "Favoritos",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favoritos");

            migrationBuilder.CreateTable(
                name: "Favorito",
                columns: table => new
                {
                    ClienteId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorito", x => new { x.ClienteId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_Favorito_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorito_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorito_ProdutoId",
                table: "Favorito",
                column: "ProdutoId");
        }
    }
}
