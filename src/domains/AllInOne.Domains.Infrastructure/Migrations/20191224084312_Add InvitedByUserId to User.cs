using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AllInOne.Domains.Infrastructure.Migrations
{
    public partial class AddInvitedByUserIdtoUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InvitedByUserId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InvitedByUserId",
                table: "AspNetUsers",
                column: "InvitedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_InvitedByUserId",
                table: "AspNetUsers",
                column: "InvitedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_InvitedByUserId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_InvitedByUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InvitedByUserId",
                table: "AspNetUsers");
        }
    }
}
