using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolfV12.Server.Data.Migrations
{
    public partial class cuatro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_G110Organizacion_OrganizacionId",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_G110Organizacion",
                table: "G110Organizacion");

            migrationBuilder.RenameTable(
                name: "G110Organizacion",
                newName: "Organizaciones");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizaciones",
                table: "Organizaciones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Organizaciones_OrganizacionId",
                table: "Players",
                column: "OrganizacionId",
                principalTable: "Organizaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Organizaciones_OrganizacionId",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizaciones",
                table: "Organizaciones");

            migrationBuilder.RenameTable(
                name: "Organizaciones",
                newName: "G110Organizacion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_G110Organizacion",
                table: "G110Organizacion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_G110Organizacion_OrganizacionId",
                table: "Players",
                column: "OrganizacionId",
                principalTable: "G110Organizacion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
