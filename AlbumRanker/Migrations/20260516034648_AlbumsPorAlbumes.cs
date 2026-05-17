using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlbumRanker.Migrations
{
    /// <inheritdoc />
    public partial class AlbumsPorAlbumes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Artistas_ArtistaId",
                table: "Albums");

            migrationBuilder.DropForeignKey(
                name: "FK_Canciones_Albums_AlbumId",
                table: "Canciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Albums",
                table: "Albums");

            migrationBuilder.RenameTable(
                name: "Albums",
                newName: "Albumes");

            migrationBuilder.RenameIndex(
                name: "IX_Albums_ArtistaId",
                table: "Albumes",
                newName: "IX_Albumes_ArtistaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Albumes",
                table: "Albumes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Albumes_Artistas_ArtistaId",
                table: "Albumes",
                column: "ArtistaId",
                principalTable: "Artistas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Canciones_Albumes_AlbumId",
                table: "Canciones",
                column: "AlbumId",
                principalTable: "Albumes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albumes_Artistas_ArtistaId",
                table: "Albumes");

            migrationBuilder.DropForeignKey(
                name: "FK_Canciones_Albumes_AlbumId",
                table: "Canciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Albumes",
                table: "Albumes");

            migrationBuilder.RenameTable(
                name: "Albumes",
                newName: "Albums");

            migrationBuilder.RenameIndex(
                name: "IX_Albumes_ArtistaId",
                table: "Albums",
                newName: "IX_Albums_ArtistaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Albums",
                table: "Albums",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Artistas_ArtistaId",
                table: "Albums",
                column: "ArtistaId",
                principalTable: "Artistas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Canciones_Albums_AlbumId",
                table: "Canciones",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
