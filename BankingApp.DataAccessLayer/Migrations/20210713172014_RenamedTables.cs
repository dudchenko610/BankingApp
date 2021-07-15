using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApp.DataAccessLayer.Migrations
{
    public partial class RenamedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositeHistoryItems_DepositeHistories_DepositeHistoryId",
                table: "DepositeHistoryItems");

            migrationBuilder.RenameColumn(
                name: "DepositeHistoryId",
                table: "DepositeHistoryItems",
                newName: "DepositId");

            migrationBuilder.RenameIndex(
                name: "IX_DepositeHistoryItems_DepositeHistoryId",
                table: "DepositeHistoryItems",
                newName: "IX_DepositeHistoryItems_DepositId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositeHistoryItems_DepositeHistories_DepositId",
                table: "DepositeHistoryItems",
                column: "DepositId",
                principalTable: "DepositeHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositeHistoryItems_DepositeHistories_DepositId",
                table: "DepositeHistoryItems");

            migrationBuilder.RenameColumn(
                name: "DepositId",
                table: "DepositeHistoryItems",
                newName: "DepositeHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_DepositeHistoryItems_DepositId",
                table: "DepositeHistoryItems",
                newName: "IX_DepositeHistoryItems_DepositeHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositeHistoryItems_DepositeHistories_DepositeHistoryId",
                table: "DepositeHistoryItems",
                column: "DepositeHistoryId",
                principalTable: "DepositeHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
