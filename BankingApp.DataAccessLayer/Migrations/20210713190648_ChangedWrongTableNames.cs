using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApp.DataAccessLayer.Migrations
{
    public partial class ChangedWrongTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepositeHistoryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepositeHistories",
                table: "DepositeHistories");

            migrationBuilder.RenameTable(
                name: "DepositeHistories",
                newName: "Deposits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deposits",
                table: "Deposits",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MonthlyPayments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositId = table.Column<int>(type: "int", nullable: false),
                    MonthNumber = table.Column<int>(type: "int", nullable: false),
                    TotalMonthSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Percents = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyPayments_Deposits_DepositId",
                        column: x => x.DepositId,
                        principalTable: "Deposits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyPayments_DepositId",
                table: "MonthlyPayments",
                column: "DepositId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyPayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deposits",
                table: "Deposits");

            migrationBuilder.RenameTable(
                name: "Deposits",
                newName: "DepositeHistories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepositeHistories",
                table: "DepositeHistories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DepositeHistoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositId = table.Column<int>(type: "int", nullable: false),
                    MonthNumber = table.Column<int>(type: "int", nullable: false),
                    Percents = table.Column<float>(type: "real", nullable: false),
                    TotalMonthSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositeHistoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositeHistoryItems_DepositeHistories_DepositId",
                        column: x => x.DepositId,
                        principalTable: "DepositeHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepositeHistoryItems_DepositId",
                table: "DepositeHistoryItems",
                column: "DepositId");
        }
    }
}
