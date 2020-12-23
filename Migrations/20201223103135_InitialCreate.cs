using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "IsReturnable",
                table: "Shops",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReturnDiscount",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReturnQuantity",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReturnedProductId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReturnedProducts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnedProducts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReturnedProductId",
                table: "Orders",
                column: "ReturnedProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ReturnedProducts_ReturnedProductId",
                table: "Orders",
                column: "ReturnedProductId",
                principalTable: "ReturnedProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ReturnedProducts_ReturnedProductId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ReturnedProducts");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ReturnedProductId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsReturnable",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ReturnDiscount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReturnQuantity",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReturnedProductId",
                table: "Orders");
        }
    }
}
