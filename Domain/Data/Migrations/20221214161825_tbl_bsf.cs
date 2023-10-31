using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_bsf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "BSFGov",
                schema: "master",
                columns: table => new
                {
                    BSFGovId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistrictName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TehsilName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitute = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Longitute = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    DepartmentContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentFaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FocalPersonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalGrant = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FocalPersonCellNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnSitePersonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnSitePersonCellNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessPlanAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FisibilityReportAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinancialProgress = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhysicalProgress = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalComponent = table.Column<int>(type: "int", nullable: false),
                    CompletedComponent = table.Column<int>(type: "int", nullable: false),
                    GrantAlocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BSFGov", x => x.BSFGovId);
                });

            migrationBuilder.CreateTable(
                name: "BSFPrivate",
                schema: "master",
                columns: table => new
                {
                    BSFPrivateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralBusinessIdeaId = table.Column<int>(type: "int", nullable: false),
                    BeneficiaryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistrictName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitute = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Longitute = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NTN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CellNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalGrant = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BusinessPlanAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FisibilityReportAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractAwardAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CNICAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NTNAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessSMEName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizationNTN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessSector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SitesupervisorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SitesupervisorContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BSFPrivate", x => x.BSFPrivateId);
                    table.ForeignKey(
                        name: "FK_BSFPrivate_GeneralBusinessIdea_GeneralBusinessIdeaId",
                        column: x => x.GeneralBusinessIdeaId,
                        principalSchema: "master",
                        principalTable: "GeneralBusinessIdea",
                        principalColumn: "GeneralBusinessIdeaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BSFGovStage",
                schema: "master",
                columns: table => new
                {
                    BSFGovStageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BSFGovId = table.Column<int>(type: "int", nullable: false),
                    StageNumber = table.Column<int>(type: "int", nullable: false),
                    StageName = table.Column<int>(type: "int", nullable: false),
                    StageAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountRelease = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BSFGovStage", x => x.BSFGovStageId);
                    table.ForeignKey(
                        name: "FK_BSFGovStage_BSFGov_BSFGovId",
                        column: x => x.BSFGovId,
                        principalSchema: "master",
                        principalTable: "BSFGov",
                        principalColumn: "BSFGovId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BSFPrivateStage",
                schema: "master",
                columns: table => new
                {
                    BSFPrivateStageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BSFPrivateId = table.Column<int>(type: "int", nullable: false),
                    StageNumber = table.Column<int>(type: "int", nullable: false),
                    StageName = table.Column<int>(type: "int", nullable: false),
                    StageAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountRelease = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BSFPrivateStage", x => x.BSFPrivateStageId);
                    table.ForeignKey(
                        name: "FK_BSFPrivateStage_BSFPrivate_BSFPrivateId",
                        column: x => x.BSFPrivateId,
                        principalSchema: "master",
                        principalTable: "BSFPrivate",
                        principalColumn: "BSFPrivateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BSFGovStage_BSFGovId",
                schema: "master",
                table: "BSFGovStage",
                column: "BSFGovId");

            migrationBuilder.CreateIndex(
                name: "IX_BSFPrivate_GeneralBusinessIdeaId",
                schema: "master",
                table: "BSFPrivate",
                column: "GeneralBusinessIdeaId");

            migrationBuilder.CreateIndex(
                name: "IX_BSFPrivateStage_BSFPrivateId",
                schema: "master",
                table: "BSFPrivateStage",
                column: "BSFPrivateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BSFGovStage",
                schema: "master");

            migrationBuilder.DropTable(
                name: "BSFPrivateStage",
                schema: "master");

            migrationBuilder.DropTable(
                name: "BSFGov",
                schema: "master");

            migrationBuilder.DropTable(
                name: "BSFPrivate",
                schema: "master");

           
        }
    }
}
