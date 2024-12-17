using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TeaDemo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityInsertIssue2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "IsDeleted", "Name", "Price", "Size" },
                values: new object[,]
                {
                    { 1, 1, "台灣在地水果茶", "", false, "水果茶", 60.0, "大杯" },
                    { 2, 2, "人生的味道", "", false, "鐵觀音", 35.0, "中杯" },
                    { 3, 3, "休閒時光", "", false, "美式咖啡", 60.0, "中杯" }
                });
        }
    }
}
