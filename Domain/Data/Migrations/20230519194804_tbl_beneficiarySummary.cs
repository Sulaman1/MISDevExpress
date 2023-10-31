using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_beneficiarySummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "BeneficiarySummary",
                columns: table => new
                {
                    BeneficiarySummaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaleBeneficiary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FemaleBeneficiary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaleRefugeeBeneficiary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FemaleRefugeeBeneficiary = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneficiarySummary", x => x.BeneficiarySummaryId);
                });

     
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropTable(
                name: "BeneficiarySummary");

           
        }
    }
}
