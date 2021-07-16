using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApp.DataAccessLayer.Migrations
{
    public partial class UserWiredWithDeposit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Deposits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_UserId",
                table: "Deposits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_AspNetUsers_UserId",
                table: "Deposits",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_AspNetUsers_UserId",
                table: "Deposits");

            migrationBuilder.DropIndex(
                name: "IX_Deposits_UserId",
                table: "Deposits");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Deposits");
        }
    }
}
