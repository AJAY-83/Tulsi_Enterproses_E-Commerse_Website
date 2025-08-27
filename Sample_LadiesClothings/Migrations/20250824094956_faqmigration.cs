using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sample_LadiesClothings.Migrations
{
    public partial class faqmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_faq",
                columns: table => new
                {
                    FAQ_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FAQ_Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FAQ_Answer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_faq", x => x.FAQ_Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_faq");
        }
    }
}
