using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi_EcommerceReact.Models;

namespace WebApi_EcommerceReact.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MenuItem>().HasData(
               new MenuItem
               {
                   Id = 1,
                   Name = "Margherita Pizza",
                   Description = "Classic Margherita with fresh mozzarella and basil.",
                   SpecialTag = "Vegetarian",
                   Category = "Pizza",
                   Price = 9.99,
                   Image = "images/margherita_pizza.jpg"
               },
        new MenuItem
        {
            Id = 2,
            Name = "Pepperoni Pizza",
            Description = "Pepperoni and mozzarella cheese.",
            SpecialTag = "",
            Category = "Pizza",
            Price = 12.99,
            Image = "images/pepperoni_pizza.jpg"
        },
        new MenuItem
        {
            Id = 3,
            Name = "BBQ Chicken Pizza",
            Description = "BBQ sauce, grilled chicken, red onions, and cilantro.",
            SpecialTag = "",
            Category = "Pizza",
            Price = 14.99,
            Image = "images/bbq_chicken_pizza.jpg"
        },
        new MenuItem
        {
            Id = 4,
            Name = "Caesar Salad",
            Description = "Romaine lettuce, Caesar dressing, croutons, and Parmesan cheese.",
            SpecialTag = "Gluten-Free",
            Category = "Salad",
            Price = 8.99,
            Image = "images/caesar_salad.jpg"
        },
        new MenuItem
        {
            Id = 5,
            Name = "Greek Salad",
            Description = "Romaine, tomatoes, cucumbers, red onions, olives, and feta cheese.",
            SpecialTag = "Vegetarian",
            Category = "Salad",
            Price = 7.99,
            Image = "images/greek_salad.jpg"
        },
        new MenuItem
        {
            Id = 6,
            Name = "Chicken Alfredo Pasta",
            Description = "Fettuccine pasta with creamy Alfredo sauce and grilled chicken.",
            SpecialTag = "",
            Category = "Pasta",
            Price = 13.99,
            Image = "images/chicken_alfredo_pasta.jpg"
        },
        new MenuItem
        {
            Id = 7,
            Name = "Spaghetti Bolognese",
            Description = "Spaghetti with a rich and savory Bolognese sauce.",
            SpecialTag = "",
            Category = "Pasta",
            Price = 11.99,
            Image = "images/spaghetti_bolognese.jpg"
        },
        new MenuItem
        {
            Id = 8,
            Name = "Garlic Bread",
            Description = "Freshly baked bread with garlic butter.",
            SpecialTag = "",
            Category = "Appetizer",
            Price = 4.99,
            Image = "images/garlic_bread.jpg"
        },
        new MenuItem
        {
            Id = 9,
            Name = "Bruschetta",
            Description = "Grilled bread topped with diced tomatoes, garlic, and basil.",
            SpecialTag = "Vegetarian",
            Category = "Appetizer",
            Price = 6.99,
            Image = "images/bruschetta.jpg"
        },
        new MenuItem
        {
            Id = 10,
            Name = "Chocolate Lava Cake",
            Description = "Warm chocolate cake with a molten chocolate center.",
            SpecialTag = "Dessert",
            Category = "Dessert",
            Price = 7.99,
            Image = "images/chocolate_lava_cake.jpg"
        }

                );
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderHeader> orderHeaders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }

    }
}
