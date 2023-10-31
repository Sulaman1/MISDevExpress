using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_tvtmember2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPWD",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.DropColumn(
                name: "IsSelfEmployed",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.AddColumn<string>(
                name: "PWD",
                schema: "training",
                table: "TVTTrainingMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SelfEmployed",
                schema: "training",
                table: "TVTTrainingMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PWD",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.DropColumn(
                name: "SelfEmployed",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.AddColumn<bool>(
                name: "IsPWD",
                schema: "training",
                table: "TVTTrainingMember",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSelfEmployed",
                schema: "training",
                table: "TVTTrainingMember",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
