using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevOpsEduTool.Migrations
{
    /// <inheritdoc />
    public partial class ModuleSeedDevOpsBasicsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsLocked",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsLocked",
                value: true);
        }
    }
}
