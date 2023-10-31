using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class bsf_priv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountTitle",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "BankAddress",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "BankName",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "BusinessSMEName",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "FatherName",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.RenameColumn(
                name: "OrganizationNTN",
                schema: "master",
                table: "BSFPrivate",
                newName: "TotalApplicantInRupees");

            migrationBuilder.RenameColumn(
                name: "FisibilityReportAttachment",
                schema: "master",
                table: "BSFPrivate",
                newName: "TotalGrantInRupees");

            migrationBuilder.RenameColumn(
                name: "ContractAwardAttachment",
                schema: "master",
                table: "BSFPrivate",
                newName: "StructureofProposedBusiness");

            migrationBuilder.RenameColumn(
                name: "CNICAttachment",
                schema: "master",
                table: "BSFPrivate",
                newName: "RegistrationWithGOP");

            migrationBuilder.RenameColumn(
                name: "BusinessPlanAttachment",
                schema: "master",
                table: "BSFPrivate",
                newName: "ProposedBusinessName");

         
            migrationBuilder.AlterColumn<string>(
                name: "SitesupervisorName",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SitesupervisorContactNumber",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BusinessSector",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                schema: "master",
                table: "BSFPrivate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BusinessFieldExperience",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessNature",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentRegistrationStatus",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateofRegistration",
                schema: "master",
                table: "BSFPrivate",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DetailofEmploymentJobforProposedBusiness",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExistingBusinessName",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExperienceInYear",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRefuge",
                schema: "master",
                table: "BSFPrivate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NatureofExistingBusiness",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumberofJobBeneficiaries",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherAttachment",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProposedBusinessDistrict",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: true);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropColumn(
                name: "Age",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "BusinessFieldExperience",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "BusinessNature",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "CurrentRegistrationStatus",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "DateofRegistration",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "DetailofEmploymentJobforProposedBusiness",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "Education",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "ExistingBusinessName",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "ExperienceInYear",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "IsRefuge",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "NatureofExistingBusiness",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "NumberofJobBeneficiaries",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "OtherAttachment",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.DropColumn(
                name: "ProposedBusinessDistrict",
                schema: "master",
                table: "BSFPrivate");

            migrationBuilder.RenameColumn(
                name: "TotalGrantInRupees",
                schema: "master",
                table: "BSFPrivate",
                newName: "FisibilityReportAttachment");

            migrationBuilder.RenameColumn(
                name: "TotalApplicantInRupees",
                schema: "master",
                table: "BSFPrivate",
                newName: "OrganizationNTN");

            migrationBuilder.RenameColumn(
                name: "StructureofProposedBusiness",
                schema: "master",
                table: "BSFPrivate",
                newName: "ContractAwardAttachment");

            migrationBuilder.RenameColumn(
                name: "RegistrationWithGOP",
                schema: "master",
                table: "BSFPrivate",
                newName: "CNICAttachment");

            migrationBuilder.RenameColumn(
                name: "ProposedBusinessName",
                schema: "master",
                table: "BSFPrivate",
                newName: "BusinessPlanAttachment");

           
            migrationBuilder.AlterColumn<string>(
                name: "SitesupervisorName",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SitesupervisorContactNumber",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BusinessSector",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountTitle",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankAddress",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessSMEName",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                schema: "master",
                table: "BSFPrivate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
