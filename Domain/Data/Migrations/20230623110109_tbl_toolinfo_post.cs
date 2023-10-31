using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBContext.Data.Migrations
{
    public partial class tbl_toolinfo_post : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "mobile",
                table: "ToolUserAccess",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ToolControlInfoPost",
                schema: "mobile",
                columns: table => new
                {
                    ToolControlInfoPostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ControlName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToolControlId = table.Column<int>(type: "int", nullable: false),
                    ToolId = table.Column<int>(type: "int", nullable: false),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    ControlValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControlLebel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolControlInfoPost", x => x.ToolControlInfoPostId);
                });

            migrationBuilder.CreateTable(
                name: "ToolInfoDetailPost",
                schema: "mobile",
                columns: table => new
                {
                    ToolControlDetailPostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToolControlId = table.Column<int>(type: "int", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ControlDetailValue = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolInfoDetailPost", x => x.ToolControlDetailPostId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToolControlInfoPost",
                schema: "mobile");

            migrationBuilder.DropTable(
                name: "ToolInfoDetailPost",
                schema: "mobile");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "mobile",
                table: "ToolUserAccess",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
