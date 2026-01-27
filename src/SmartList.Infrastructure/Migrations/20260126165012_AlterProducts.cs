using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_products_UserId",
                table: "products",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_users_UserId",
                table: "products",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_users_UserId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_UserId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "products");
        }
    }
}
