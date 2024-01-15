//basic features
//nehaa: option 1,3,4
//sophie: option 2,5,6

using assg;

void DisplayMenu()
{
    while (true)
    {
        try
        {


            Console.WriteLine("\nThe I.C. Treats Management System (enter 0 to break)");
            Console.WriteLine("=================================");
            Console.WriteLine("[1] List all customers");
            Console.WriteLine("[2] List all current orders");
            Console.WriteLine("[3] Regsiter a new customer");
            Console.WriteLine("[4] Create a customers' order");
            Console.WriteLine("[5] Display order details of a customer");
            Console.WriteLine("[6] Modify order details");
            Console.Write("\nEnter an option: ");
            int option = Convert.ToInt32(Console.ReadLine());
            if (option == 1)
            {
                Option1();

            }
            else if (option == 2)
            {
                
            }
            else if (option == 3)
            {
                Option3();                
            }
            else if (option == 4)
            {
                Option4();
            }
            else if (option == 5)
            {

            }
            else if (option == 6)
            {

            }
            else if (option == 0)
            {
                break;
            }
            else
            {
                Console.WriteLine("Please enter a valid option.");
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Please enter a valid number for the option.");
        }



    }
}

DisplayMenu();

//---BASIC FEATURES---

//method to make an icecream order
IceCream iceCreamOrder()
{
    Console.Write("Enter number of scoops: ");
    int scoops = Convert.ToInt32(Console.ReadLine());

    Console.Write("Enter the ice cream option: (\"1\": Cup / \"2\": Cone / \"3\": Waffle ) ");
    string option = Console.ReadLine();

    Console.Write("Enter the number of toppings: ");
    int numberOfToppings = Convert.ToInt32(Console.ReadLine());

    List<Flavour> flavours = new List<Flavour>();

    for (int i = 0; i < scoops; i++)
    {
        Console.Write($"Enter flavour for scoop {i + 1}: ");
        string flavourName = Console.ReadLine();
        Flavour flavour = new Flavour();

        if (flavourName == "durian" || flavourName == "ube" || flavourName == "sea salt")
        {
            flavour.type = flavourName;
            flavour.premium = true;
        }

        flavours.Add(flavour);
    }

    List<Topping> toppings = new List<Topping>();
    for (int i = 0; i < numberOfToppings; i++)
    {
        Console.Write($"Enter topping {i + 1}: (sprinkles/mochi/sago/oreos) ");
        string toppingName = Console.ReadLine();
        toppings.Add(new Topping(toppingName));
    }

    if (option == "1")
    {
        return new Cup(option, scoops, flavours, toppings);
    }
    else if (option == "2")
    {
        Console.Write("Do you want the cone dipped? (true/false): ");
        bool isDipped = bool.Parse(Console.ReadLine());
        return new Cone(option, scoops, flavours, toppings, isDipped);
    }
    else if (option == "3")
    {
        Console.Write("What waffle flavour do you want? (Original/Red Velvet/Charcoal/Pandan): ");
        string wf = Console.ReadLine();
        return new Waffle(option, scoops, flavours, toppings, wf);
    }
    else
    {
        Console.WriteLine("Error. Please key in options from 1 to 3.");
        return null;
    }

}

//Option 1: 
void Option1()
{
    //reading from customers.csv file
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine(); // read the heading
                                   // display the heading
        if (s != null)
        {
            string[] heading = s.Split(',');
            Console.WriteLine("{0,-10}  {1,-10}  {2,-10}  {3,-20}  {4,-17}  {5,-15}",
                heading[0], heading[1], heading[2], heading[3], heading[4], heading[5]);
            // repeat until end of file
        }
        while ((s = sr.ReadLine()) != null)
        {
            string[] info = s.Split(',');
            Console.WriteLine("{0,-10}  {1,-10}  {2,-10}  {3,-20}  {4,-17}  {5,-15}",
                info[0], info[1], info[2], info[3], info[4], info[5]);
        }
    }
}

//Option 2: 


//Option 3: 
void Option3()
{
    //prompt user for details
    Console.Write("Enter customer name: ");
    string name = Console.ReadLine();

    Console.Write("Enter customer ID number: ");
    int id = Convert.ToInt32(Console.ReadLine());

    Console.Write("Enter customer data of birth: ");
    string dobString = Console.ReadLine();
    DateTime dob;

    if (DateTime.TryParse(dobString, out dob))
    {
        Console.WriteLine($"Date of Birth: {dob}");
    }
    else
    {
        Console.WriteLine("Invalid date format.");
    }

    //create customer object
    Customer customer = new Customer(name, id, dob);

    //create pointcard object
    PointCard pointCard = new PointCard(0,0);

    //assign the PointCard to Customer
    customer.rewards = pointCard;


    //append customer info into customers csv file
    string memstatus = "Silver";
    string sid = Convert.ToString(id);

    List<string> newList = new List<string> { name, sid, dobString, memstatus, "0", "0" };

    using (StreamWriter sw = new StreamWriter("customers.csv", true))
    {
        string csvLine = string.Join(",", newList);
        sw.WriteLine(csvLine);

        Console.WriteLine("Registration status: SUCCESSFUL");
    }

}

//Option 4: 
void Option4()
{
    //list customers from customer.csv
    List<Customer> customerList = new List<Customer>();
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine(); // read the heading
                                   // display the heading
        while ((s = sr.ReadLine()) != null)
        {
            string[] info = s.Split(',');
            string c_name = info[0];
            string c_id = info[1];

            string dobString = info[2];

            DateTime c_dob;
            DateTime.TryParse(dobString, out c_dob);
            

            int id = Convert.ToInt32(c_id);
            Customer customer = new Customer(c_name, id, c_dob);
            customerList.Add(customer);
        }
    }

    //prompt user to select customer and retrieve selected customer
    Console.Write("Select a customer (enter Customer ID) : ");
    int cus_id = Convert.ToInt32(Console.ReadLine());

    foreach (var x in customerList)
    {
        if (x.memberId == cus_id)
        {
            List<IceCream> orderList = new List<IceCream>();

            while (true)
            {
                Console.WriteLine("Do you want to add an ice cream order? (yes/no)");
                string yesno = Console.ReadLine();

                if (yesno == "no")
                {
                    break;
                }
                else
                {
                    // create new ice cream object
                    IceCream iceCream = iceCreamOrder();
                    orderList.Add(iceCream);
                }
            }
        }

    }
}

//Option 5: 


//Option 6: 