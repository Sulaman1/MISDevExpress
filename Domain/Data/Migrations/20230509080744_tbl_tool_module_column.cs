using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_tool_module_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<string>(
                name: "District",
                schema: "mobile",
                table: "Tool",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ToolModule",
                schema: "mobile",
                table: "Tool",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.DropColumn(
                name: "District",
                schema: "mobile",
                table: "Tool");

            migrationBuilder.DropColumn(
                name: "ToolModule",
                schema: "mobile",
                table: "Tool");

          
        }
    }
}
