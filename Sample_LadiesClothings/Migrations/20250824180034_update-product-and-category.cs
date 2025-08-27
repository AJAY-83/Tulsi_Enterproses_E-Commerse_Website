using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample_LadiesClothings.Migrations
{
    public partial class updateproductandcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Category_Id",
                table: "tbl_products",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_products_Category_Id",
                table: "tbl_products",
                column: "Category_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_products_tbl_category_Category_Id",
                table: "tbl_products",
                column: "Category_Id",
                principalTable: "tbl_category",
                principalColumn: "Category_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_products_tbl_category_Category_Id",
                table: "tbl_products");

            migrationBuilder.DropIndex(
                name: "IX_tbl_products_Category_Id",
                table: "tbl_products");

            migrationBuilder.AlterColumn<string>(
                name: "Category_Id",
                table: "tbl_products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
