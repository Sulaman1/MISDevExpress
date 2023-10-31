using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class grm_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                schema: "master",
                table: "GrievanceRedressal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPickUpResponses",
                schema: "master",
                table: "GrievanceRedressal",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                schema: "master",
                table: "GrievanceRedressal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TehsilName",
                schema: "master",
                table: "GrievanceRedressal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictName",
                schema: "master",
                table: "GrievanceRedressal");

            migrationBuilder.DropColumn(
                name: "IsPickUpResponses",
                schema: "master",
                table: "GrievanceRedressal");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                schema: "master",
                table: "GrievanceRedressal");

            migrationBuilder.DropColumn(
                name: "TehsilName",
                schema: "master",
                table: "GrievanceRedressal");
        }
    }
}
