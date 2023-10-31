using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_hremployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.EnsureSchema(
                name: "HR");

           

            migrationBuilder.CreateTable(
                name: "HREmployee",
                schema: "HR",
                columns: table => new
                {
                    HREmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PTCLNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MailingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastEducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DomicileLocal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoiningDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistrictOfWork = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNICAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoiningLetterAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CVAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HREmployee", x => x.HREmployeeId);
                    table.ForeignKey(
                        name: "FK_HREmployee_Section_SectionId",
                        column: x => x.SectionId,
                        principalSchema: "master",
                        principalTable: "Section",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Cascade);
                });

          

            migrationBuilder.CreateIndex(
                name: "IX_HREmployee_SectionId",
                schema: "HR",
                table: "HREmployee",
                column: "SectionId");

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropTable(
                name: "HREmployee",
                schema: "HR");

           

          

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
    }
}
