using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class hr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          


            migrationBuilder.AddColumn<string>(
                name: "BankAccount",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CNICAttachment",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CVAttachment",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                schema: "master",
                table: "Employee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictOfWork",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DomicileLocal",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "JoiningDate",
                schema: "master",
                table: "Employee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JoiningLetterAttachment",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastEducationLevel",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MailingAddress",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PTCLNumber",
                schema: "master",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "BankAccount",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CNICAttachment",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CVAttachment",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DOB",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DistrictOfWork",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DomicileLocal",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Gender",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "JoiningDate",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "JoiningLetterAttachment",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LastEducationLevel",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "MailingAddress",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                schema: "master",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PTCLNumber",
                schema: "master",
                table: "Employee");

          
        }
    }
}
