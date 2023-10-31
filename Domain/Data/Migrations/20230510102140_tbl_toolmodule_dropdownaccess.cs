using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_toolmodule_dropdownaccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                schema: "mobile",
                table: "Tool");

            migrationBuilder.CreateTable(
                name: "DropdownMenu",
                schema: "mobile",
                columns: table => new
                {
                    DropdownMenuName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropdownMenu", x => x.DropdownMenuName);
                });

            migrationBuilder.CreateTable(
                name: "ToolModule",
                schema: "mobile",
                columns: table => new
                {
                    ToolModuleName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolModule", x => x.ToolModuleName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DropdownMenu",
                schema: "mobile");

            migrationBuilder.DropTable(
                name: "ToolModule",
                schema: "mobile");

            migrationBuilder.AddColumn<string>(
                name: "District",
                schema: "mobile",
                table: "Tool",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
