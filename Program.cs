// See https://aka.ms/new-console-template for more information

using Checkpoint2_davidnilsson;

User user = new User();
user.AutoFill(user);
StartMenu(user);

Console.WriteLine("--program end");

static void StartMenu(User user)
{
    PrintMenu(); //uses this outside of the loop because i want the error msg to show if input is wrong
    while (true)
    {
        string input = CheckIfNullOrEmpty();
        if (input.ToUpper() == "Q") { break; }
        int menuChoice = VerifyInt(input, 3); 
        if (menuChoice == -1) { continue; } //VerifyInt-function sets menuChoice to -1 if input is not accepted
        if (menuChoice == 1) { CategoryMainMenu(user); PrintMenu(); continue; } 
        if (menuChoice == 2) { ProductsMainMenu(user); PrintMenu(); continue; }
        if (menuChoice == 3) { PriceMainMenu(user); PrintMenu(); continue; }
    }
}

static void PrintMenu()
{
    Console.Clear();
    Console.WriteLine("Input the corresponding number or 'Q' to quit ");
    Console.WriteLine();
    Console.WriteLine("1. Add category");
    Console.WriteLine("2. Add product");
    Console.WriteLine("3. See all prices");
    Console.WriteLine();
}

static string CheckIfNullOrEmpty()
{
    //rejecting empty inputs
    string input = "";
    while (true)
    {
        Console.Write("> ");
        input = Console.ReadLine().Trim();
        if (String.IsNullOrEmpty(input))
        {
            Console.WriteLine("Please input something...");
            continue;
        }
        else { break; }
    }
    return input;
}

static int VerifyInt(string input, int span)
{
    //rejecting everything that is not a number
    bool isInt = int.TryParse(input, out int menuChoice);
    if (!isInt) 
    {
        Console.WriteLine("Please input a number...");
        menuChoice = -1; 
        return menuChoice; 
    }
    //only accepting ints that are printed in the menu (1, 2, 3)
    if (menuChoice > span || menuChoice <= 0)
    {
        Console.WriteLine("Please input a number from the menu...");
        menuChoice = -1;
        return menuChoice;
    }
    else { return menuChoice; }
}



static User CategoryMainMenu(User user)
{
    Console.Clear();
    PrintCategories(user); //prints categories before the user can add a new category
    AddCategory(user);
    return user; //meaningless since Userproperties are public
}

static void PrintCategories(User user)
{
    Console.WriteLine("Current categories: ");
    Console.WriteLine();
    int counter = 1;
    //incrementing counter to assign each category with a menu-option in the print
    foreach (Category category in user.Categories) //remember, user object stores everything
    {
        Console.WriteLine($"{counter}. {category.CategoryName}");
        counter++;
    }
}

static User AddCategory(User user)
{
    Console.WriteLine();
    Console.WriteLine("Please input the name of your new category, or 'Q' to go back.");
    while (true)
    {
        //verifying input for category name
        string input = CheckIfNullOrEmpty();
        if (input.ToUpper() == "Q") { break; }
        bool isInt = int.TryParse(input, out int num);
        if (isInt) { Console.WriteLine("Please input a name and not a number..."); continue; }

        //creates and adds new category
        Category newCat = new Category(input);
        user.Categories.Add(newCat);
        break;
    }
    return user;
}



static User ProductsMainMenu(User user)
{
    Console.Clear();
    PrintCategories(user); //printing categories so the user first chooses category to add a new product
    AddProduct(user);
    return user;
}

static User AddProduct(User user)
{
    Console.WriteLine();
    Console.WriteLine("To add a new product you must first choose category, or 'Q' to go back");
    while (true)
    {
        //verifying input for which category to add a product in
        string input = CheckIfNullOrEmpty();
        if (input.ToUpper() == "Q") { break; }
        int span = user.Categories.Count();
        int menuChoice = VerifyInt(input, span);
        if (menuChoice == -1) { continue; }

        PrintProducts(user, menuChoice); //so user can see current products before adding a new product

        Console.WriteLine("Please input the name of your new product or 'Q' to go back");
        while (true)
        {
            //verifying input for product name
            string productName = CheckIfNullOrEmpty();
            if (productName.ToUpper() == "Q") { break; }
            bool isInt = int.TryParse(productName, out int num);
            if (isInt) { Console.WriteLine("Please input a name and not a number..."); continue; }

            Console.WriteLine("Please input the price of your new product or 'Q' to go back");
            while (true)
            {
                //verifying input for product price
                string price = CheckIfNullOrEmpty();
                if (price.ToUpper() == "Q") { break; }
                isInt = int.TryParse(price, out int verifiedPrice);
                if (!isInt)
                {
                    Console.WriteLine("Please input a number...");
                    continue;
                }

                //creates and adds the product
                Product newProduct = new Product(productName, verifiedPrice, user.Categories[menuChoice - 1]);
                user.Categories[menuChoice - 1].Products.Add(newProduct);
                break;
            }
            break;
        }
        break;
    }
    return user;
}

static void PrintProducts(User user, int menuChoice) //function is called inside AddProduct() after category is chosen
{
    Console.Clear();
    Console.WriteLine($"Current products for the category '{user.Categories[menuChoice - 1].CategoryName}':");
    Console.WriteLine();

    //uses menuChoice int from when user chose category to know in which category to print the products from
    if (user.Categories[menuChoice - 1].Products.Count() == 0) 
    { 
        Console.WriteLine("(no products added yet)");
    }
    foreach (Product product in user.Categories[menuChoice - 1].Products) 
    {
        Console.WriteLine($"'{product.ProductName}'");
    }
    Console.WriteLine();
}



static User PriceMainMenu(User user)
{
    string search = "";
    Console.Clear();
    PrintPrices(user, search); //first print is with empty search string (= no match, i.e regular print)
    while (true)
    {
        search = Search(user); //enables user to search product
        if (search == "Q") { break; } //if search input is Q (for quit) then Search() returns "Q"
        bool foundMatch = PrintPrices(user, search);
        if (!foundMatch) 
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Cant find that product...");
            Console.ResetColor();
            Console.WriteLine();
            continue; 
        }
    }
    return user;
}

static bool PrintPrices(User user, string search)
{
    Console.Clear();
    int counter = 0;
    int totalSum = 0;
    bool foundMatch = false;
    Sort(user);

    foreach (Category category in user.Categories)
    {
        int sum = 0;
        Console.WriteLine($"Category: '{category.CategoryName}'");
        //uses counter as category-index, and i as product index. could have just made nested forloop dont know why i didnt
        for (int i = 0; i < user.Categories[counter].Products.Count; i++)
        {
            //looking for a match between the search input and productname
            if (user.Categories[counter].Products[i].ProductName.ToLower().Trim() == search.ToLower().Trim())
            {
                foundMatch = true;
                Console.Write($"\t{i + 1}. ");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                //only highlighting inline. 'name' 'price' and 'kr'
                Console.Write($"{user.Categories[counter].Products[i].ProductName} {user.Categories[counter].Products[i].Price.ToString()}kr");
                Console.ResetColor();
                Console.WriteLine();
            }
            //if no match then regular print
            else
            {
                Console.WriteLine($"\t{i + 1}. {user.Categories[counter].Products[i].ProductName} {user.Categories[counter].Products[i].Price.ToString()}kr");
            }
            sum += user.Categories[counter].Products[i].Price;
        }
        //adds the sum for each category to the totalsum, then resets sum and goes to next category
        Console.WriteLine($"\tSum: {sum}kr");
        totalSum += sum;
        counter++;
        Console.WriteLine();
    }
    Console.WriteLine($"Sum of all products: {totalSum}kr");
    return foundMatch; //uses this for error message
}

static string Search(User user)
{
    string input = "";
    Console.WriteLine();
    Console.WriteLine("To highlight a product, input its product name. Or input 'Q' to go back to main menu");
    while (true)
    {
        //verifying search input
        input = CheckIfNullOrEmpty();
        if (input.ToUpper() == "Q") { input = "Q"; break; }
        break;
    }
    return input;
}

static User Sort(User user)
{
    for (int i = 0; i < user.Categories.Count; i++)
    {
        //sorts the products by price, low to high
        user.Categories[i].Products.Sort((x, y) => x.Price.CompareTo(y.Price));
    }
    return user;
}
