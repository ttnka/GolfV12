using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolfV12.Server.Data.Migrations
{
    public partial class Catorce : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Hcp",
                table: "Jugadores",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hcp",
                table: "Jugadores");
        }
    }
}
