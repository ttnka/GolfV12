using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolfV12.Server.Data.Migrations
{
    public partial class Six : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banderas_Campos_CampoId",
                table: "Banderas");

            migrationBuilder.DropForeignKey(
                name: "FK_Bitacoras_Players_UsuarioId",
                table: "Bitacoras");

            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Players_CreadorId",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Distancias_Banderas_BanderaId",
                table: "Distancias");

            migrationBuilder.DropForeignKey(
                name: "FK_Fotos_Players_PlayerId",
                table: "Fotos");

            migrationBuilder.DropForeignKey(
                name: "FK_Hcps_Banderas_BanderaId",
                table: "Hcps");

            migrationBuilder.DropForeignKey(
                name: "FK_Hcps_Players_PlayerId",
                table: "Hcps");

            migrationBuilder.DropForeignKey(
                name: "FK_Hoyos_Campos_CampoId",
                table: "Hoyos");

            migrationBuilder.DropForeignKey(
                name: "FK_Players_Organizaciones_OrganizacionId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_OrganizacionId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Hoyos_CampoId",
                table: "Hoyos");

            migrationBuilder.DropIndex(
                name: "IX_Hcps_BanderaId",
                table: "Hcps");

            migrationBuilder.DropIndex(
                name: "IX_Hcps_PlayerId",
                table: "Hcps");

            migrationBuilder.DropIndex(
                name: "IX_Fotos_PlayerId",
                table: "Fotos");

            migrationBuilder.DropIndex(
                name: "IX_Distancias_BanderaId",
                table: "Distancias");

            migrationBuilder.DropIndex(
                name: "IX_Citas_CreadorId",
                table: "Citas");

            migrationBuilder.DropIndex(
                name: "IX_Bitacoras_UsuarioId",
                table: "Bitacoras");

            migrationBuilder.DropIndex(
                name: "IX_Banderas_CampoId",
                table: "Banderas");

            migrationBuilder.DropColumn(
                name: "User",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "CreadorId",
                table: "Citas",
                newName: "Creador");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Creador",
                table: "Citas",
                newName: "CreadorId");

            migrationBuilder.AddColumn<int>(
                name: "User",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Players_OrganizacionId",
                table: "Players",
                column: "OrganizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Hoyos_CampoId",
                table: "Hoyos",
                column: "CampoId");

            migrationBuilder.CreateIndex(
                name: "IX_Hcps_BanderaId",
                table: "Hcps",
                column: "BanderaId");

            migrationBuilder.CreateIndex(
                name: "IX_Hcps_PlayerId",
                table: "Hcps",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_PlayerId",
                table: "Fotos",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Distancias_BanderaId",
                table: "Distancias",
                column: "BanderaId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_CreadorId",
                table: "Citas",
                column: "CreadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bitacoras_UsuarioId",
                table: "Bitacoras",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Banderas_CampoId",
                table: "Banderas",
                column: "CampoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banderas_Campos_CampoId",
                table: "Banderas",
                column: "CampoId",
                principalTable: "Campos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bitacoras_Players_UsuarioId",
                table: "Bitacoras",
                column: "UsuarioId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Players_CreadorId",
                table: "Citas",
                column: "CreadorId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Distancias_Banderas_BanderaId",
                table: "Distancias",
                column: "BanderaId",
                principalTable: "Banderas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fotos_Players_PlayerId",
                table: "Fotos",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hcps_Banderas_BanderaId",
                table: "Hcps",
                column: "BanderaId",
                principalTable: "Banderas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hcps_Players_PlayerId",
                table: "Hcps",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Hoyos_Campos_CampoId",
                table: "Hoyos",
                column: "CampoId",
                principalTable: "Campos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Organizaciones_OrganizacionId",
                table: "Players",
                column: "OrganizacionId",
                principalTable: "Organizaciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
