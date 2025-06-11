using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class LocationPhotoEntityChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "LocationPhotos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "LocationPhotos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
