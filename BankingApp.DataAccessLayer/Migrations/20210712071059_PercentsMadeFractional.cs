using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApp.DataAccessLayer.Migrations
{
    public partial class PercentsMadeFractional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Percents",
                table: "DepositeHistories",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Percents",
                table: "DepositeHistories",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
