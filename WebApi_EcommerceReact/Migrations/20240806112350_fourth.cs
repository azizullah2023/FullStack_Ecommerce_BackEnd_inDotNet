using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi_EcommerceReact.Migrations
{
    /// <inheritdoc />
    public partial class fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Category", "Description", "Image", "Name", "Price", "SpecialTag" },
                values: new object[,]
                {
                    { 1, "Pizza", "Classic Margherita with fresh mozzarella and basil.", "images/margherita_pizza.jpg", "Margherita Pizza", 9.9900000000000002, "Vegetarian" },
                    { 2, "Pizza", "Pepperoni and mozzarella cheese.", "images/pepperoni_pizza.jpg", "Pepperoni Pizza", 12.99, "" },
                    { 3, "Pizza", "BBQ sauce, grilled chicken, red onions, and cilantro.", "images/bbq_chicken_pizza.jpg", "BBQ Chicken Pizza", 14.99, "" },
                    { 4, "Salad", "Romaine lettuce, Caesar dressing, croutons, and Parmesan cheese.", "images/caesar_salad.jpg", "Caesar Salad", 8.9900000000000002, "Gluten-Free" },
                    { 5, "Salad", "Romaine, tomatoes, cucumbers, red onions, olives, and feta cheese.", "images/greek_salad.jpg", "Greek Salad", 7.9900000000000002, "Vegetarian" },
                    { 6, "Pasta", "Fettuccine pasta with creamy Alfredo sauce and grilled chicken.", "images/chicken_alfredo_pasta.jpg", "Chicken Alfredo Pasta", 13.99, "" },
                    { 7, "Pasta", "Spaghetti with a rich and savory Bolognese sauce.", "images/spaghetti_bolognese.jpg", "Spaghetti Bolognese", 11.99, "" },
                    { 8, "Appetizer", "Freshly baked bread with garlic butter.", "images/garlic_bread.jpg", "Garlic Bread", 4.9900000000000002, "" },
                    { 9, "Appetizer", "Grilled bread topped with diced tomatoes, garlic, and basil.", "images/bruschetta.jpg", "Bruschetta", 6.9900000000000002, "Vegetarian" },
                    { 10, "Dessert", "Warm chocolate cake with a molten chocolate center.", "images/chocolate_lava_cake.jpg", "Chocolate Lava Cake", 7.9900000000000002, "Dessert" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
