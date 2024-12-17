using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeaDemo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "IsDeleted", "Name", "Price", "Size" },
                values: new object[,]
                {
                    { 2, 2, "人生的味道", false, "鐵觀音", 35.0, "中杯" },
                    { 3, 3, "休閒時光", false, "美式咖啡", 60.0, "中杯" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
