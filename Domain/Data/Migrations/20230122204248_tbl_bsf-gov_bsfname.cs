using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_bsfgov_bsfname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.AddColumn<string>(
                name: "BSFName",
                schema: "master",
                table: "BSFGov",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {           

            migrationBuilder.DropColumn(
                name: "BSFName",
                schema: "master",
                table: "BSFGov");
        }
    }
}
