using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watchlist.Data.Migrations
{
    public partial class DbClassesUpdates20240321 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmsUtilisateur_AspNetUsers_UserId",
                table: "FilmsUtilisateur");

            migrationBuilder.DropForeignKey(
                name: "FK_FilmsUtilisateur_Films_FilmId",
                table: "FilmsUtilisateur");

            migrationBuilder.DropIndex(
                name: "IX_FilmsUtilisateur_FilmId",
                table: "FilmsUtilisateur");

            migrationBuilder.DropIndex(
                name: "IX_FilmsUtilisateur_UserId",
                table: "FilmsUtilisateur");

            migrationBuilder.DropColumn(
                name: "FilmId",
                table: "FilmsUtilisateur");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FilmsUtilisateur");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_FilmsUtilisateur_IdFilm",
                table: "FilmsUtilisateur",
                column: "IdFilm");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmsUtilisateur_AspNetUsers_IdUtilisateur",
                table: "FilmsUtilisateur",
                column: "IdUtilisateur",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FilmsUtilisateur_Films_IdFilm",
                table: "FilmsUtilisateur",
                column: "IdFilm",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmsUtilisateur_AspNetUsers_IdUtilisateur",
                table: "FilmsUtilisateur");

            migrationBuilder.DropForeignKey(
                name: "FK_FilmsUtilisateur_Films_IdFilm",
                table: "FilmsUtilisateur");

            migrationBuilder.DropIndex(
                name: "IX_FilmsUtilisateur_IdFilm",
                table: "FilmsUtilisateur");

            migrationBuilder.AddColumn<int>(
                name: "FilmId",
                table: "FilmsUtilisateur",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FilmsUtilisateur",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateIndex(
                name: "IX_FilmsUtilisateur_FilmId",
                table: "FilmsUtilisateur",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmsUtilisateur_UserId",
                table: "FilmsUtilisateur",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmsUtilisateur_AspNetUsers_UserId",
                table: "FilmsUtilisateur",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FilmsUtilisateur_Films_FilmId",
                table: "FilmsUtilisateur",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
