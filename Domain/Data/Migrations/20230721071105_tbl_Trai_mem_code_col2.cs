using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_Trai_mem_code_col2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainingTypeCode",
                schema: "training",
                table: "MemberTraining",
                newName: "MemberTrainingCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MemberTrainingCode",
                schema: "training",
                table: "MemberTraining",
                newName: "TrainingTypeCode");
        }
    }
}
