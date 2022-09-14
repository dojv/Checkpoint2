using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoint2_davidnilsson
{
    internal class User
    {
        //stores all categories (and stores the products withing each category) =nested lists
        public List<Category> Categories = new List<Category>();

        //these are only for the autofill function
        public List<string> RandomCategories = new List<string>() { "Cars", "Cellphones", "Clothes" };
        public List<string> RandomCars = new List<string>() { "Volvo", "SAAB", "Opel" };
        public List<string> RandomCellphones = new List<string>() { "Iphone", "Android", "Huawei" };
        public List<string> RandomClothes = new List<string>() { "Pants", "Sweater", "Shoes" };

        //just adds some data to the app by the start to save time
        public User AutoFill(User user)
        {
            Random roll = new Random();

            //makes category objects to store the products on
            Category cars = new Category("Cars");
            Category cellphones = new Category("Cellphones");
            Category clothes = new Category("Clothes");

            //each block works the same= adds products to the categoryobjects, with random prices
            foreach (string car in RandomCars)
            {
                Product product = new Product();
                product.ProductName = car;
                product.Category = cars;
                product.Price = roll.Next(10000, 100000);
                cars.Products.Add(product);
            }
            foreach (string cell in RandomCellphones)
            {
                Product product = new Product();
                product.ProductName = cell;
                product.Category = cellphones;
                product.Price = roll.Next(1000, 10000);
                cellphones.Products.Add(product);
            }
            foreach (string cloth in RandomClothes)
            {
                Product product = new Product();
                product.ProductName = cloth;
                product.Category = clothes;
                product.Price = roll.Next(250, 1000);
                clothes.Products.Add(product);
            }

            //adds the categoryobjects to a list in the user object so all data is in the same place
            user.Categories.Add(cars);
            user.Categories.Add(cellphones);
            user.Categories.Add(clothes);
            return user;
        }
    }
}
