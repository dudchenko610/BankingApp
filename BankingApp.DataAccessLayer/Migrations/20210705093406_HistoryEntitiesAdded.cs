using Microsoft.EntityFrameworkCore.Migrations;

namespace BankingApp.DataAccessLayer.Migrations
{
    public partial class HistoryEntitiesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DepositeHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalculationFormula = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepositeSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthsCount = table.Column<int>(type: "int", nullable: false),
                    Percents = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositeHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepositeHistoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepositeHistoryId = table.Column<int>(type: "int", nullable: false),
                    MonthNumber = table.Column<int>(type: "int", nullable: false),
                    TotalMonthSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Percents = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositeHistoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositeHistoryItems_DepositeHistories_DepositeHistoryId",
                        column: x => x.DepositeHistoryId,
                        principalTable: "DepositeHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepositeHistoryItems_DepositeHistoryId",
                table: "DepositeHistoryItems",
                column: "DepositeHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepositeHistoryItems");

            migrationBuilder.DropTable(
                name: "DepositeHistories");
        }
    }
}
