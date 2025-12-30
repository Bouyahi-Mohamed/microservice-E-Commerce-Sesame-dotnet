using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Ratings_RatingId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_RatingId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "RatingId",
                table: "Products",
                newName: "RatingCount");

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "RatingStars",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RatingStars",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "RatingCount",
                table: "Products",
                newName: "RatingId");

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_RatingId",
                table: "Products",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Ratings_RatingId",
                table: "Products",
                column: "RatingId",
                principalTable: "Ratings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
