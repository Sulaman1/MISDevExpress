using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_hts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "HTS",
                schema: "master",
                columns: table => new
                {
                    HTSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Khasra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TunnelSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitute = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Longitute = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    TotalGrant = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    VillageId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    CNICAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgricultureLandProofAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationFormAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TunnelSiteSuitabilityFormAttachment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HTS", x => x.HTSId);
                    table.ForeignKey(
                        name: "FK_HTS_Member_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "master",
                        principalTable: "Member",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HTS_Village_VillageId",
                        column: x => x.VillageId,
                        principalSchema: "master",
                        principalTable: "Village",
                        principalColumn: "VillageId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "HTSStage",
                schema: "master",
                columns: table => new
                {
                    HTSStageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HTSId = table.Column<int>(type: "int", nullable: false),
                    InstallmentNo = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<int>(type: "int", nullable: false),
                    StageAttachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateofPayment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HTSStage", x => x.HTSStageId);
                    table.ForeignKey(
                        name: "FK_HTSStage_HTS_HTSId",
                        column: x => x.HTSId,
                        principalSchema: "master",
                        principalTable: "HTS",
                        principalColumn: "HTSId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HTS_MemberId",
                schema: "master",
                table: "HTS",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_HTS_VillageId",
                schema: "master",
                table: "HTS",
                column: "VillageId");

            migrationBuilder.CreateIndex(
                name: "IX_HTSStage_HTSId",
                schema: "master",
                table: "HTSStage",
                column: "HTSId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HTSStage",
                schema: "master");

            migrationBuilder.DropTable(
                name: "HTS",
                schema: "master");           
        }
    }
}
