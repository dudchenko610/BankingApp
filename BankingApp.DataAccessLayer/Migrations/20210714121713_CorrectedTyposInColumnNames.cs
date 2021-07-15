using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApp.DataAccessLayer.Migrations
{
    public partial class CorrectedTyposInColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepositeSum",
                table: "Deposits",
                newName: "DepositSum");

            migrationBuilder.RenameColumn(
                name: "CalulationDateTime",
                table: "Deposits",
                newName: "CalсulationDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DepositSum",
                table: "Deposits",
                newName: "DepositeSum");

            migrationBuilder.RenameColumn(
                name: "CalсulationDateTime",
                table: "Deposits",
                newName: "CalulationDateTime");
        }
    }
}
