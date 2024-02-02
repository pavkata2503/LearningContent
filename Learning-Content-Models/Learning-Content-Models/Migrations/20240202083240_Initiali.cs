using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_Content_Models.Migrations
{
    /// <inheritdoc />
    public partial class Initiali : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SearchQuery",
                table: "Messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SearchQuery",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
