using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_dropdownaccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DropdownenuAccess",
                schema: "mobile",
                columns: table => new
                {
                    DropdownenuAccessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToolId = table.Column<int>(type: "int", nullable: false),
                    DropdownMenuName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToolModuleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropdownenuAccess", x => x.DropdownenuAccessId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DropdownenuAccess",
                schema: "mobile");
        }
    }
}
