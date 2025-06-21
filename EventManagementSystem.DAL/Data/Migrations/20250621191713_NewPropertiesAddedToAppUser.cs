using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NewPropertiesAddedToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStudentApproved",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StudentImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStudentApproved",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudentImageUrl",
                table: "AspNetUsers");
        }
    }
}
