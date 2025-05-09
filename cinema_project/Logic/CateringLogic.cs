public static class CateringLogic
{


    public static void AddMenuItem(Dictionary<string, object> newItem)
    {
        List<Dictionary<string, object>> menu = CateringAccess.LoadMenuFromJson(CateringAccess.cateringmenu);
        menu.Add(newItem);
        CateringAccess.SaveMenuToJson(menu, CateringAccess.cateringmenu);
        Console.WriteLine("Menu item added successfully!");
    }
    public static void DeleteItemFromMenu(int item_id)
    {
        List<Dictionary<string, object>> menu = CateringAccess.LoadMenuFromJson(CateringAccess.cateringmenu);
        bool found = false;
        for (int i = 0; i < menu.Count; i++)
        {
            if (menu[i].ContainsKey("id") && Convert.ToInt64(menu[i]["id"]) == item_id)
            {
                menu.RemoveAt(i);
                found = true;
                break;
            }
        }

        if (found)
        {
            CateringAccess.SaveMenuToJson(menu, CateringAccess.cateringmenu);
            Console.WriteLine("Menu item deleted successfully!");
        }
        else
        {
            Console.WriteLine("Invalid ID");
        }
    }
    public static void SortItems()
    {
        int i = 1;
        List<Dictionary<string, object>> menu = CateringAccess.LoadMenuFromJson(CateringAccess.cateringmenu);
        foreach (var item in menu)
        {
            item["id"] = i;
            i++;
        }
        CateringAccess.SaveMenuToJson(menu, CateringAccess.cateringmenu);
    }

    public static void ViewItems(string choice, string filePath)
    {
        int num = 0;
        List<Dictionary<string, object>> items = CateringAccess.LoadMenuFromJson(filePath);
        SortItems();

        Console.WriteLine(new string('=', 70));

        if (choice != "all")
        {
            Console.WriteLine("Category: {0}", choice);
            Console.WriteLine(new string('=', 70));
        }

        Console.WriteLine("{0,-5} | {1,-20} | {2,-10} | {3,-10}", "No.", "Product", "Size", "Price");
        Console.WriteLine(new string('=', 70));

        foreach (var item in items)
        {
            SortItems();
            CateringItem.Food_ID = Convert.ToInt32(item["id"]);
            CateringItem.Product = (string)item["product"];
            CateringItem.Category = (string)item["category"];
            CateringItem.Size = (string)item["size"];
            CateringItem.Price = Convert.ToDouble(item["price"]);

            if (CateringItem.Category == choice || choice == "all")
            {
                num++;
                Console.WriteLine("{0,-5} | {1,-20} | {2,-10} | {3,-10} euro",
                    num,
                    CateringItem.Product,
                    CateringItem.Size,
                    CateringItem.Price);
                Console.WriteLine(new string('-', 70));
            }
        }

        Console.WriteLine(new string('=', 70));
        Console.WriteLine();
    }

    public static void AddCateringItem()
    {
        string productcatstring = null;
        bool correctinput = false;
        Console.WriteLine("Product name?");
        string productname = Console.ReadLine();
        Console.WriteLine("Food Category? (F or D)");
        char productcat = Console.ReadKey().KeyChar;
        Console.WriteLine("Size?");
        string size = Console.ReadLine();
        Console.WriteLine("Price?");
        double price = Convert.ToDouble(Console.ReadLine());
        while (!correctinput)
        {
            if (productcat == 'f' || productcat == 'F')
            {
                productcatstring = "Food";
                correctinput = true;
            }
            else if (productcat == 'd' || productcat == 'D')
            {
                productcatstring = "Drink";
                correctinput = true;
            }
            else
            {
                Console.WriteLine("Invalid input");
                productcat = Console.ReadKey().KeyChar;
            }
        }
        Dictionary<string, object> newItem = new Dictionary<string, object>
        {
            {"id", 0 },
            { "product", productname },
            { "category", productcatstring },
            { "size", size },
            { "price", price },
        };
        AddMenuItem(newItem);
        SortItems();
    }


    public static void RemoveItem()
    {
        ViewItems("all", CateringAccess.cateringmenu);
        Console.WriteLine("Which Item would you like to remove? (by ID)");
        int idinput = Convert.ToInt32(Console.ReadLine());
        DeleteItemFromMenu(idinput);
    }

    public static int? IDFound(int id_)
    {
        CateringLogic.SortItems();
        List<Dictionary<string, object>> menu = CateringAccess.LoadMenuFromJson(CateringAccess.cateringmenu);
        for (int i = 0; i < menu.Count; i++)
        {
            if (menu[i].ContainsKey("id") && Convert.ToInt64(menu[i]["id"]) == id_)
            {
                return id_;
            }
        }
        return null;
    }
    public static void EditItem(int id_, string char_, object newValue)
    {
        List<Dictionary<string, object>> menu = CateringAccess.LoadMenuFromJson(CateringAccess.cateringmenu);
        bool found = false;
        for (int i = 0; i < menu.Count; i++)
        {
            if ((int)(long)menu[i]["id"] == id_)
            {
                if (menu[i].ContainsKey(char_))
                {
                    if (newValue is string || newValue is int)
                    {
                        menu[i][char_] = newValue;
                        CateringAccess.SaveMenuToJson(menu, CateringAccess.cateringmenu);
                        Console.WriteLine($"Successfully changed the {char_} to {newValue}");
                    }
                    else
                    {
                        Console.WriteLine("Wrong input (invalid data type)");
                    }
                    found = true;
                    break;
                }
            }
        }
        if (!found)
        {
            Console.WriteLine($"Item with {char_} not found");
        }
    }
}