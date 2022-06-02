using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolfV12.Server.Data.Migrations
{
    public partial class Treintaycuatro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Azar",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tarjeta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoAzar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Azar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bolitas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tarjeta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Azar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    J1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    J2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<int>(type: "int", nullable: false),
                    H1V = table.Column<int>(type: "int", nullable: false),
                    H2V = table.Column<int>(type: "int", nullable: false),
                    H3V = table.Column<int>(type: "int", nullable: false),
                    H4V = table.Column<int>(type: "int", nullable: false),
                    H5V = table.Column<int>(type: "int", nullable: false),
                    H6V = table.Column<int>(type: "int", nullable: false),
                    H7V = table.Column<int>(type: "int", nullable: false),
                    H8V = table.Column<int>(type: "int", nullable: false),
                    H9V = table.Column<int>(type: "int", nullable: false),
                    H10V = table.Column<int>(type: "int", nullable: false),
                    H11V = table.Column<int>(type: "int", nullable: false),
                    H12V = table.Column<int>(type: "int", nullable: false),
                    H13V = table.Column<int>(type: "int", nullable: false),
                    H14V = table.Column<int>(type: "int", nullable: false),
                    H15V = table.Column<int>(type: "int", nullable: false),
                    H16V = table.Column<int>(type: "int", nullable: false),
                    H17V = table.Column<int>(type: "int", nullable: false),
                    H18V = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bolitas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loba",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tarjeta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Azar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<int>(type: "int", nullable: false),
                    H1V = table.Column<int>(type: "int", nullable: false),
                    H2V = table.Column<int>(type: "int", nullable: false),
                    H3V = table.Column<int>(type: "int", nullable: false),
                    H4V = table.Column<int>(type: "int", nullable: false),
                    H5V = table.Column<int>(type: "int", nullable: false),
                    H6V = table.Column<int>(type: "int", nullable: false),
                    H7V = table.Column<int>(type: "int", nullable: false),
                    H8V = table.Column<int>(type: "int", nullable: false),
                    H9V = table.Column<int>(type: "int", nullable: false),
                    H10V = table.Column<int>(type: "int", nullable: false),
                    H11V = table.Column<int>(type: "int", nullable: false),
                    H12V = table.Column<int>(type: "int", nullable: false),
                    H13V = table.Column<int>(type: "int", nullable: false),
                    H14V = table.Column<int>(type: "int", nullable: false),
                    H15V = table.Column<int>(type: "int", nullable: false),
                    H16V = table.Column<int>(type: "int", nullable: false),
                    H17V = table.Column<int>(type: "int", nullable: false),
                    H18V = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loba", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LobaDet",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Loba = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hoyo = table.Column<int>(type: "int", nullable: false),
                    J1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    J2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    J3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    J4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LobaDet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parejas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tarjeta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Azar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    J1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    J2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    J3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    J4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precio = table.Column<int>(type: "int", nullable: false),
                    H1V = table.Column<int>(type: "int", nullable: false),
                    H2V = table.Column<int>(type: "int", nullable: false),
                    H3V = table.Column<int>(type: "int", nullable: false),
                    H4V = table.Column<int>(type: "int", nullable: false),
                    H5V = table.Column<int>(type: "int", nullable: false),
                    H6V = table.Column<int>(type: "int", nullable: false),
                    H7V = table.Column<int>(type: "int", nullable: false),
                    H8V = table.Column<int>(type: "int", nullable: false),
                    H9V = table.Column<int>(type: "int", nullable: false),
                    H10V = table.Column<int>(type: "int", nullable: false),
                    H11V = table.Column<int>(type: "int", nullable: false),
                    H12V = table.Column<int>(type: "int", nullable: false),
                    H13V = table.Column<int>(type: "int", nullable: false),
                    H14V = table.Column<int>(type: "int", nullable: false),
                    H15V = table.Column<int>(type: "int", nullable: false),
                    H16V = table.Column<int>(type: "int", nullable: false),
                    H17V = table.Column<int>(type: "int", nullable: false),
                    H18V = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parejas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participantes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tarjeta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Azar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    J1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    J2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participantes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposAzar",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Creador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Indivual = table.Column<bool>(type: "bit", nullable: false),
                    Publico = table.Column<bool>(type: "bit", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposAzar", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Azar");

            migrationBuilder.DropTable(
                name: "Bolitas");

            migrationBuilder.DropTable(
                name: "Loba");

            migrationBuilder.DropTable(
                name: "LobaDet");

            migrationBuilder.DropTable(
                name: "Parejas");

            migrationBuilder.DropTable(
                name: "Participantes");

            migrationBuilder.DropTable(
                name: "TiposAzar");
        }
    }
}
