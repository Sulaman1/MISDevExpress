using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_toolInfoDetail_key_alter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToolControlDetailPostId",
                schema: "mobile",
                table: "ToolInfoDetailPost",
                newName: "ToolInfoDetailPostId");

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToolInfoDetailPostId",
                schema: "mobile",
                table: "ToolInfoDetailPost",
                newName: "ToolControlDetailPostId");

           
        }
    }
}
