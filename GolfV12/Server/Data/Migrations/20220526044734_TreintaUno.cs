using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolfV12.Server.Data.Migrations
{
    public partial class TreintaUno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tarjetas502",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Creador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Campo = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Captura = table.Column<int>(type: "int", nullable: false),
                    Consulta = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarjetas502", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MyScore522",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tarjeta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<int>(type: "int", nullable: false),
                    Teams = table.Column<int>(type: "int", nullable: false),
                    Player = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hcp = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Renglon = table.Column<int>(type: "int", nullable: false),
                    HcpB9 = table.Column<bool>(type: "bit", nullable: false),
                    H1 = table.Column<int>(type: "int", nullable: false),
                    H2 = table.Column<int>(type: "int", nullable: false),
                    H3 = table.Column<int>(type: "int", nullable: false),
                    H4 = table.Column<int>(type: "int", nullable: false),
                    H5 = table.Column<int>(type: "int", nullable: false),
                    H6 = table.Column<int>(type: "int", nullable: false),
                    H7 = table.Column<int>(type: "int", nullable: false),
                    H8 = table.Column<int>(type: "int", nullable: false),
                    H9 = table.Column<int>(type: "int", nullable: false),
                    H10 = table.Column<int>(type: "int", nullable: false),
                    H11 = table.Column<int>(type: "int", nullable: false),
                    H12 = table.Column<int>(type: "int", nullable: false),
                    H13 = table.Column<int>(type: "int", nullable: false),
                    H14 = table.Column<int>(type: "int", nullable: false),
                    H15 = table.Column<int>(type: "int", nullable: false),
                    H16 = table.Column<int>(type: "int", nullable: false),
                    H17 = table.Column<int>(type: "int", nullable: false),
                    H18 = table.Column<int>(type: "int", nullable: false),
                    G502TarjetasId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyScore522", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyScore522_Tarjetas502_G502TarjetasId",
                        column: x => x.G502TarjetasId,
                        principalTable: "Tarjetas502",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyScore522_G502TarjetasId",
                table: "MyScore522",
                column: "G502TarjetasId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyScore522");

            migrationBuilder.DropTable(
                name: "Tarjetas502");
        }
    }
}
