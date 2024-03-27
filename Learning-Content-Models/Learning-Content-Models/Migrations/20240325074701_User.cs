using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_Content_Models.Migrations
{
    /// <inheritdoc />
    public partial class User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppliationUserId",
                table: "StudyMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "StudyMaterials",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudyMaterials_ApplicationUserId",
                table: "StudyMaterials",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyMaterials_AspNetUsers_ApplicationUserId",
                table: "StudyMaterials",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyMaterials_AspNetUsers_ApplicationUserId",
                table: "StudyMaterials");

            migrationBuilder.DropIndex(
                name: "IX_StudyMaterials_ApplicationUserId",
                table: "StudyMaterials");

            migrationBuilder.DropColumn(
                name: "AppliationUserId",
                table: "StudyMaterials");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "StudyMaterials");
        }
    }
}
