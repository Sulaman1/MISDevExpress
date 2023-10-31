using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class grm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "GrievanceRedressal",
                schema: "master",
                columns: table => new
                {
                    GRMId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GRMNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternateContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MethodtoContact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAlternateContact = table.Column<bool>(type: "bit", nullable: false),
                    IsByEmail = table.Column<bool>(type: "bit", nullable: false),
                    IsByPhone = table.Column<bool>(type: "bit", nullable: false),
                    IsByMobile = table.Column<bool>(type: "bit", nullable: false),
                    IsByMail = table.Column<bool>(type: "bit", nullable: false),
                    MailingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PickUpResponses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplaintChannel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GRMDetail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanUserPersonalDetail = table.Column<bool>(type: "bit", nullable: false),
                    CanUserMyName = table.Column<bool>(type: "bit", nullable: false),
                    DoDisclose = table.Column<bool>(type: "bit", nullable: false),
                    Attachment1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachment2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachment3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrievanceRedressal", x => x.GRMId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrievanceRedressal",
                schema: "master");

          
        }
    }
}
