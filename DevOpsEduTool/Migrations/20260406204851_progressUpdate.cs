using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevOpsEduTool.Migrations
{
    /// <inheritdoc />
    public partial class progressUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Modules_ModuleId",
                table: "UserProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Users_UserId",
                table: "UserProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProgresses",
                table: "UserProgresses");

            migrationBuilder.RenameTable(
                name: "UserProgresses",
                newName: "UserProgress");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgresses_UserId",
                table: "UserProgress",
                newName: "IX_UserProgress_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgresses_ModuleId",
                table: "UserProgress",
                newName: "IX_UserProgress_ModuleId");

            migrationBuilder.AddColumn<string>(
                name: "ControllerAction",
                table: "Modules",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Modules",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProgress",
                table: "UserProgress",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ControllerAction", "IsLocked" },
                values: new object[] { "", true });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ControllerAction", "IsLocked" },
                values: new object[] { "", true });

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ControllerAction", "IsLocked" },
                values: new object[] { "", true });

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgress_Modules_ModuleId",
                table: "UserProgress",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgress_Users_UserId",
                table: "UserProgress",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProgress_Modules_ModuleId",
                table: "UserProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgress_Users_UserId",
                table: "UserProgress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProgress",
                table: "UserProgress");

            migrationBuilder.DropColumn(
                name: "ControllerAction",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Modules");

            migrationBuilder.RenameTable(
                name: "UserProgress",
                newName: "UserProgresses");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgress_UserId",
                table: "UserProgresses",
                newName: "IX_UserProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgress_ModuleId",
                table: "UserProgresses",
                newName: "IX_UserProgresses_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProgresses",
                table: "UserProgresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Modules_ModuleId",
                table: "UserProgresses",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Users_UserId",
                table: "UserProgresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
