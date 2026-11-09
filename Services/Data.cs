using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductsValidation.Models;

namespace ProductsValidation.Services
{
    public class Data
    {
        public List<Product> Products = new List<Product>
        {
            new Product() {Id = 1, Type = Product.Category.Toy, Name = "Product2", Description = "Product2 description", Price = 25},
            new Product() {Id = 2, Type = Product.Category.Technique, Name = "Product3", Description = "Product3 description", Price = 150},
            new Product() {Id = 3, Type = Product.Category.Clothes, Name = "Product4", Description = "Product4 description", Price = 40},
            new Product() {Id = 4, Type = Product.Category.Transport, Name = "Product5", Description = "Product5 description", Price = 500},
            new Product() {Id = 5, Type = Product.Category.Toy, Name = "Product6", Description = "Product6 description", Price = 75},
            new Product() {Id = 6, Type = Product.Category.Technique, Name = "Product1", Description = "Product1 description", Price = 250},
        };

        public List<User> Users = new List<User>
        {
            new User() {Id = 1, Name = "UserAdmin", Email = "admin@gmail.com", Role = "admin"},
            new User() {Id = 2, Name = "Guest", Email = "guest@gmail.com", Role = "guest"},
            new User() {Id = 3, Name = "User", Email = "user1@gmail.com", Role = "user"},
            new User() {Id = 4, Name = "User2", Email = "user2@gmail.com", Role = "user"},

        };
    }
}
