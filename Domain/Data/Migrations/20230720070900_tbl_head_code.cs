using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_head_code : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrainingTypeCode",
                schema: "master",
                table: "TrainingType",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrainingHeadCode",
                schema: "master",
                table: "TrainingHead",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropColumn(
                name: "TrainingTypeCode",
                schema: "master",
                table: "TrainingType");

            migrationBuilder.DropColumn(
                name: "TrainingHeadCode",
                schema: "master",
                table: "TrainingHead");
        }
    }
}
