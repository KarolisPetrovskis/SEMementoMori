using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MementoMori.Server.Migrations
{
    /// <inheritdoc />
    public partial class nullableuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_CreatorId",
                table: "Decks");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "Decks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Users_CreatorId",
                table: "Decks",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Users_CreatorId",
                table: "Decks");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "Decks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Users_CreatorId",
                table: "Decks",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
