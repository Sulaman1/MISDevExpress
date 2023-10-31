using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_tvt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         
            migrationBuilder.CreateTable(
                name: "TVTTrainedBy",
                schema: "master",
                columns: table => new
                {
                    TVTTrainedById = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVTTrainedBy", x => x.TVTTrainedById);
                });

            migrationBuilder.CreateTable(
                name: "TVTTraining",
                schema: "training",
                columns: table => new
                {
                    TVTTrainingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttendanceAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessionPlanAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureAttachment1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureAttachment2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureAttachment3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureAttachment4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrainingTypeId = table.Column<int>(type: "int", nullable: false),
                    TrainingCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalMember = table.Column<int>(type: "int", nullable: false),
                    TotalDays = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalMale = table.Column<int>(type: "int", nullable: false),
                    TotalFemale = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    TrainingFormNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainingCenter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VillageId = table.Column<int>(type: "int", nullable: false),
                    TVTTrainer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVTTraining", x => x.TVTTrainingId);
                    table.ForeignKey(
                        name: "FK_TVTTraining_TrainingType_TrainingTypeId",
                        column: x => x.TrainingTypeId,
                        principalSchema: "master",
                        principalTable: "TrainingType",
                        principalColumn: "TrainingTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TVTTraining_Village_VillageId",
                        column: x => x.VillageId,
                        principalSchema: "master",
                        principalTable: "Village",
                        principalColumn: "VillageId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TVTTrainingMember",
                schema: "training",
                columns: table => new
                {
                    TVTTrainingMemberId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TVTTrainingId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    BeneficiaryMISCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    EducationDocAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificateAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNICAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdmissionFormAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVTTrainingMember", x => x.TVTTrainingMemberId);
                    table.ForeignKey(
                        name: "FK_TVTTrainingMember_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "master",
                        principalTable: "Member",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TVTTrainingMember_TVTTraining_TVTTrainingId",
                        column: x => x.TVTTrainingId,
                        principalSchema: "training",
                        principalTable: "TVTTraining",
                        principalColumn: "TVTTrainingId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TVTTraining_TrainingTypeId",
                schema: "training",
                table: "TVTTraining",
                column: "TrainingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TVTTraining_VillageId",
                schema: "training",
                table: "TVTTraining",
                column: "VillageId");

            migrationBuilder.CreateIndex(
                name: "IX_TVTTrainingMember_MemberId",
                schema: "training",
                table: "TVTTrainingMember",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TVTTrainingMember_TVTTrainingId",
                schema: "training",
                table: "TVTTrainingMember",
                column: "TVTTrainingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TVTTrainedBy",
                schema: "master");

            migrationBuilder.DropTable(
                name: "TVTTrainingMember",
                schema: "training");

            migrationBuilder.DropTable(
                name: "TVTTraining",
                schema: "training");
          
        }
    }
}
