using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vehicle_Registration_System.Migrations
{
    /// <inheritdoc />
    public partial class AddedFunc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "AspNetUsers");
        }
    }
}
