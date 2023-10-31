using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_hts_stage_pics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<string>(
                name: "Picture1",
                schema: "master",
                table: "HTSStage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture2",
                schema: "master",
                table: "HTSStage",
                type: "nvarchar(max)",
                nullable: true);



        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.DropColumn(
                name: "Picture1",
                schema: "master",
                table: "HTSStage");

            migrationBuilder.DropColumn(
                name: "Picture2",
                schema: "master",
                table: "HTSStage");
          
        }
    }
}
