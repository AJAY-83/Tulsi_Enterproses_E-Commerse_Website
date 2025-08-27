using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample_LadiesClothings.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Customer_Password",
                table: "tbl_customer");

            migrationBuilder.AlterColumn<string>(
                name: "Customer_Name",
                table: "tbl_customer",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Customer_Email",
                table: "tbl_customer",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Customer_PasswordHash",
                table: "tbl_customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "tbl_orders",
                columns: table => new
                {
                    Order_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Customer_Id = table.Column<int>(type: "int", nullable: false),
                    Order_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_orders", x => x.Order_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_order_items",
                columns: table => new
                {
                    OrderItem_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_Id = table.Column<int>(type: "int", nullable: false),
                    Product_Id = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_order_items", x => x.OrderItem_Id);
                    table.ForeignKey(
                        name: "FK_tbl_order_items_tbl_orders_Order_Id",
                        column: x => x.Order_Id,
                        principalTable: "tbl_orders",
                        principalColumn: "Order_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_customer_Customer_Email",
                table: "tbl_customer",
                column: "Customer_Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_items_Order_Id",
                table: "tbl_order_items",
                column: "Order_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_order_items");

            migrationBuilder.DropTable(
                name: "tbl_orders");

            migrationBuilder.DropIndex(
                name: "IX_tbl_customer_Customer_Email",
                table: "tbl_customer");

            migrationBuilder.DropColumn(
                name: "Customer_PasswordHash",
                table: "tbl_customer");

            migrationBuilder.AlterColumn<string>(
                name: "Customer_Name",
                table: "tbl_customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Customer_Email",
                table: "tbl_customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120);

            migrationBuilder.AddColumn<string>(
                name: "Customer_Password",
                table: "tbl_customer",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
