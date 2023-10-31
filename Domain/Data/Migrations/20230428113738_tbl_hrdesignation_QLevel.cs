using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_hrdesignation_QLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HREmployee_Section_SectionId",
                schema: "HR",
                table: "HREmployee");

            migrationBuilder.DropIndex(
                name: "IX_HREmployee_SectionId",
                schema: "HR",
                table: "HREmployee");

            migrationBuilder.DropColumn(
                name: "SectionId",
                schema: "HR",
                table: "HREmployee");

            migrationBuilder.AddColumn<string>(
                name: "Section",
                schema: "HR",
                table: "HREmployee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "HRDesignation",
                schema: "HR",
                columns: table => new
                {
                    HRDesignationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesignationName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDesignation", x => x.HRDesignationId);
                });

            migrationBuilder.CreateTable(
                name: "HRQualificationLevel",
                schema: "HR",
                columns: table => new
                {
                    HRQualificationLevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRQualificationLevel", x => x.HRQualificationLevelId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRDesignation",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "HRQualificationLevel",
                schema: "HR");

            migrationBuilder.DropColumn(
                name: "Section",
                schema: "HR",
                table: "HREmployee");

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                schema: "HR",
                table: "HREmployee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HREmployee_SectionId",
                schema: "HR",
                table: "HREmployee",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_HREmployee_Section_SectionId",
                schema: "HR",
                table: "HREmployee",
                column: "SectionId",
                principalSchema: "master",
                principalTable: "Section",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
