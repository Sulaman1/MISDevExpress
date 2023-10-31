using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_tvt2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitute",
                schema: "training",
                table: "TVTTraining",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitute",
                schema: "training",
                table: "TVTTraining",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitute",
                schema: "training",
                table: "TVTTraining");

            migrationBuilder.DropColumn(
                name: "Longitute",
                schema: "training",
                table: "TVTTraining");
        }
    }
}
