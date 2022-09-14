using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoint2_davidnilsson
{
    internal class Category
    {
        public string CategoryName { get; set; }
        //each categoryobject has a list of products
        public List<Product> Products = new List<Product>();

        public Category (string categoryname) { CategoryName = categoryname; }
    }
}
