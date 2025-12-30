using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projet_API.Migrations
{
    /// <inheritdoc />
    public partial class AddCartAndDeliveryOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateAdded",
                table: "OrderItems",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "OrderItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DeliveryOptionId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliveryOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceCents = table.Column<int>(type: "int", nullable: false),
                    EstimatedDays = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOptions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_DeliveryOptionId",
                table: "OrderItems",
                column: "DeliveryOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_DeliveryOptions_DeliveryOptionId",
                table: "OrderItems",
                column: "DeliveryOptionId",
                principalTable: "DeliveryOptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_DeliveryOptions_DeliveryOptionId",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "DeliveryOptions");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_DeliveryOptionId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DeliveryOptionId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "OrderItems",
                newName: "DateAdded");
        }
    }
}
