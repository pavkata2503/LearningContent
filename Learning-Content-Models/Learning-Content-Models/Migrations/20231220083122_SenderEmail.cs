using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_Content_Models.Migrations
{
    /// <inheritdoc />
    public partial class SenderEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderEmail",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderEmail",
                table: "Messages");
        }
    }
}
