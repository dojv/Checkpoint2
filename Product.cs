using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoint2_davidnilsson
{
    internal class Product
    {
        public string ProductName { get; set; }
        public int Price { get; set; }
        //each productobject gets a category assigned to it
        public Category Category { get; set; }

        public Product () { }
        public Product (string productname, int price, Category category)
        {
            ProductName = productname;
            Price = price;
            Category = category;
        }
    }
}
