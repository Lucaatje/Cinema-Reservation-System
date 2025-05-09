public class RulesLogic : IItem
{
    public static void ViewAllRules()
    {
        List<string> Rules = RulesAccess.ReadRulesFromCSV(RulesAccess.RulesCSVFile);

        if (Rules.Count > 0)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(new string(' ', 28 )+"List of Rules:");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine();

            for (int i = 0; i < Rules.Count; i++)
            {
                Console.WriteLine($"Rule {i + 1}: {Rules[i]}");
                Console.WriteLine(new string('-', 70));
            }
            Console.WriteLine();
            Console.WriteLine(new string('=', 70));
        }
        else
        {
            Console.WriteLine("No rules found.");
        }
    }


    public static void EditRules(int RuleNumber)
    {
        List<string> Rules = new List<string>();
        Rules = RulesAccess.ReadRulesFromCSV(RulesAccess.RulesCSVFile);
        Console.WriteLine("Insert the new version of rule:");
        string NewRule = Console.ReadLine();
        if (NewRule != null)
        {
            Rules[RuleNumber - 1] = NewRule;
            Console.WriteLine("Rule successfully altered.");
        }
        else
        {
            Console.WriteLine("Rule is empty.");
        }
        RulesAccess.WriteRulesToCSV(Rules, RulesAccess.RulesCSVFile);
    }

    public void AddItem<T>(T item)
    {
        if (item is string NewRule)
        {
            List<string> Rules = RulesAccess.ReadRulesFromCSV(RulesAccess.RulesCSVFile);
            Rules.Add(NewRule);
            RulesAccess.WriteRulesToCSV(Rules, RulesAccess.RulesCSVFile);
            Console.WriteLine("Rule successfully added.");
        }
        else
        {
            Console.WriteLine("Invalid type for AddItem in RulesLogic. Expected type is string.");
        }
    }

    public bool RemoveItem<T>(T item)
    {
        if (item is string rule)
        {
            List<string> Rules = RulesAccess.ReadRulesFromCSV(RulesAccess.RulesCSVFile);
            if (Rules.Remove(rule))
            {
                RulesAccess.WriteRulesToCSV(Rules, RulesAccess.RulesCSVFile);
                Console.WriteLine("Rule successfully removed.");
                return true;
            }
            else
            {
                Console.WriteLine("Rule not found.");
                return false;
            }
        }
        else
        {
            throw new ArgumentException("Item must be of type string.");
        }
    }

}
