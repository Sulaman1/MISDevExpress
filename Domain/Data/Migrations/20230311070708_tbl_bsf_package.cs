using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_bsf_package : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {           

            migrationBuilder.AddColumn<string>(
                name: "BSFGovtPackage",
                schema: "master",
                table: "BSFGov",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConstructionType",
                schema: "master",
                table: "BSFGov",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Stage",
                schema: "master",
                table: "BSFGov",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BSFGovtPackage",
                schema: "master",
                columns: table => new
                {
                    BSFGovtPackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BSFGovtPackage", x => x.BSFGovtPackageId);
                });

            migrationBuilder.CreateTable(
                name: "ConstructionType",
                schema: "master",
                columns: table => new
                {
                    ConstructionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConstructionType", x => x.ConstructionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Stage",
                schema: "master",
                columns: table => new
                {
                    StageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stage", x => x.StageId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BSFGovtPackage",
                schema: "master");

            migrationBuilder.DropTable(
                name: "ConstructionType",
                schema: "master");

            migrationBuilder.DropTable(
                name: "Stage",
                schema: "master");

            
            migrationBuilder.DropColumn(
                name: "BSFGovtPackage",
                schema: "master",
                table: "BSFGov");

            migrationBuilder.DropColumn(
                name: "ConstructionType",
                schema: "master",
                table: "BSFGov");

            migrationBuilder.DropColumn(
                name: "Stage",
                schema: "master",
                table: "BSFGov");

          
        }
    }
}
