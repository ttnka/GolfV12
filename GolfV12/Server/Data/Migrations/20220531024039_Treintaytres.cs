using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolfV12.Server.Data.Migrations
{
    public partial class Treintaytres : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyScore522_Tarjetas502_G502TarjetasId",
                table: "MyScore522");

            migrationBuilder.DropIndex(
                name: "IX_MyScore522_G502TarjetasId",
                table: "MyScore522");

            migrationBuilder.DropColumn(
                name: "G502TarjetasId",
                table: "MyScore522");

            migrationBuilder.AddColumn<string>(
                name: "Tarjeta",
                table: "Extras",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tarjeta",
                table: "Extras");

            migrationBuilder.AddColumn<string>(
                name: "G502TarjetasId",
                table: "MyScore522",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyScore522_G502TarjetasId",
                table: "MyScore522",
                column: "G502TarjetasId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyScore522_Tarjetas502_G502TarjetasId",
                table: "MyScore522",
                column: "G502TarjetasId",
                principalTable: "Tarjetas502",
                principalColumn: "Id");
        }
    }
}
