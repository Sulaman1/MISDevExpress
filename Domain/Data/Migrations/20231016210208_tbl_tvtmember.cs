using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_tvtmember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                schema: "training",
                table: "TVTTrainingMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                schema: "training",
                table: "TVTTrainingMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdentifiedBy",
                schema: "training",
                table: "TVTTrainingMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<string>(
                name: "PreferredSkill1",
                schema: "training",
                table: "TVTTrainingMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredSkill2",
                schema: "training",
                table: "TVTTrainingMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredSkill3",
                schema: "training",
                table: "TVTTrainingMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreferredSkill4",
                schema: "training",
                table: "TVTTrainingMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RPL",
                schema: "training",
                table: "TVTTrainingMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessName",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.DropColumn(
                name: "Designation",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.DropColumn(
                name: "IdentifiedBy",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.DropColumn(
                name: "IsPWD",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.DropColumn(
                name: "IsSelfEmployed",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.DropColumn(
                name: "PreferredSkill1",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.DropColumn(
                name: "PreferredSkill2",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.DropColumn(
                name: "PreferredSkill3",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.DropColumn(
                name: "PreferredSkill4",
                schema: "training",
                table: "TVTTrainingMember");

            migrationBuilder.DropColumn(
                name: "RPL",
                schema: "training",
                table: "TVTTrainingMember");
        }
    }
}
