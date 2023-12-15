using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_Content_Models.Migrations
{
    /// <inheritdoc />
    public partial class IFileNamep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileUploadName",
                table: "StudyMaterials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileUploadName",
                table: "StudyMaterials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
