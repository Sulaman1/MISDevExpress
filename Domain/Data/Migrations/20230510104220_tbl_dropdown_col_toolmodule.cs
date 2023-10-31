using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_dropdown_col_toolmodule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ToolModuleName",
                schema: "mobile",
                table: "DropdownMenu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToolModuleName",
                schema: "mobile",
                table: "DropdownMenu");
        }
    }
}
