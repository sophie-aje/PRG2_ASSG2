//basic features
//nehaa: option 1,3,4
//sophie: option 2,5,6

using assg;
using System.Xml.Linq;

// list for customer info
List<Customer> customerList = new List<Customer>();

List<IceCream> orderList = new List<IceCream>();

// lists to append orders
List<Order> regularOrderQueue = new List<Order>();
List<Order> goldOrderQueue = new List<Order>();


// list to hold pointcard information
List<PointCard> pointCards = new List<PointCard>();

Dictionary<int, List<Order>> ordersDictionary = new Dictionary<int, List<Order>>();

void ReadOrdersCSV()
{
    using (StreamReader sr = new StreamReader("orders.csv"))
    {
        sr.ReadLine(); // Read the heading (skip)
        string s;
        while ((s = sr.ReadLine()) != null)
        {
            string[] info = s.Split('\t');

            int orderId = Convert.ToInt32(info[0]);
            int memberId = Convert.ToInt32(info[1]);
            DateTime.TryParse(info[2], out DateTime timeReceived);
            DateTime.TryParse(info[3], out DateTime timeFulfilled);

            // Create an Order object
            Order order = new Order(orderId, timeReceived);

            // assign time fulfilled
            order.timeFulfilled = timeFulfilled; 


            // add ice cream details to the order
            string option = info[4];
            int scoops = Convert.ToInt32(info[5]);
            bool dipped;
            if (info[6] == "TRUE")
            {
                dipped = true;
            }
            else
            {
                dipped = false;
            }
            string waffleFlavour = info[7];

            // Additional lists for flavours and toppings
            List<Flavour> flavours = new List<Flavour>();
            List<Topping> toppings = new List<Topping>();

            for (int i = 8; i <= 13; i++)
            {
                string flavourOrTopping = info[i];
                if (!string.IsNullOrEmpty(flavourOrTopping))
                {
                    if (i <= 10)
                    {
                        Flavour flavour = new Flavour();

                        if (flavourOrTopping == "durian" || flavourOrTopping == "ube" || flavourOrTopping == "sea salt")
                        {
                            flavour.type = flavourOrTopping;
                            flavour.premium = true;
                        }
                        else
                        {
                            flavour.type = flavourOrTopping;
                            flavour.premium = false;
                        }

                        flavours.Add(flavour);
                    }
                    else
                    {
                        Console.Write($"Enter topping for {flavourOrTopping}: (sprinkles/mochi/sago/oreos) ");
                        string toppingName = Console.ReadLine();

                        // data validation
                        if (toppingName == "sprinkles" || toppingName == "mochi" || toppingName == "sago" || toppingName == "oreos" )
                        {
                            toppings.Add(new Topping(toppingName));
                        }
                        else
                        {
                            Console.WriteLine("Invalid topping name. Using a default topping.");
                            toppings.Add(new Topping("sprinkles")); // using a default topping or handle it as needed
                        }
                    }
                }
            }

            if (ordersDictionary.ContainsKey(memberId))
            {
                ordersDictionary[memberId].Add(order);
            }
            else
            {
                ordersDictionary[memberId] = new List<Order> { order };
            }
        }
    }
}




void ReadCustomerCSV()
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        sr.ReadLine(); // Read the heading (skip)
        string s;
        while ((s = sr.ReadLine()) != null)
        {
            string[] info = s.Split(',');
            string c_name = info[0];
            string c_id = info[1];
            string dobString = info[2];
            DateTime.TryParse(dobString, out DateTime c_dob);

            int id = Convert.ToInt32(c_id);
            Customer customer = new Customer(c_name, id, c_dob);
            customerList.Add(customer);

            string tier = info[3];
            int points = Convert.ToInt32(info[4]);
            int punch = Convert.ToInt32(info[5]);
            PointCard pointCard = new PointCard(points, punch);
            pointCard.tier = tier;
            pointCards.Add(pointCard);
        }
    }

}

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
                Option2();
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
                Option5();
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
    //prompt user to enter ice cream order
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

    //creating ice cream object
    if (option == "1")
    {
        
        Cup new_cup = new Cup(option, scoops, flavours, toppings);
        orderList.Add(new_cup);
        return new_cup;
    }
    else if (option == "2")
    {
        Console.Write("Do you want the cone dipped? (true/false): ");
        bool isDipped = bool.Parse(Console.ReadLine());
        Cone new_cone = new Cone(option, scoops, flavours, toppings, isDipped);
        orderList.Add(new_cone);
        return new_cone;
    }
    else if (option == "3")
    {
        Console.Write("What waffle flavour do you want? (Original/Red Velvet/Charcoal/Pandan): ");
        string wf = Console.ReadLine();
        Waffle new_waffle = new Waffle(option, scoops, flavours, toppings, wf);
        orderList.Add(new_waffle);
        return new_waffle;
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
void Option2()
{
    using (StreamReader sr = new StreamReader("orders.csv"))
    {
        string? s = sr.ReadLine();
        if (s != null)
        {
            string[] heading = s.Split(',');
            Console.WriteLine("{0,-5}  {1,-10}  {2,-20}  {3,-20} {4,-10} " +
                "{5,-10} {6,-10} {7,-15} {8,-30} {9,-10}",
                heading[0], heading[1], heading[2], heading[3], heading[4],
                heading[5], heading[6], heading[7], "Flavour", "Toppings");
        }
        while ((s = sr.ReadLine()) != null)
        {
            string[] info = s.Split(',');

            if (info != null)
            {
                Console.WriteLine("{0,-5}  {1,-10}  {2,-20}  {3,-20} {4,-10} {5,-10} {6,-10} {7,-15} {8,-30} {9,-15}",
                    info[0], info[1], info[2], info[3], info[4], info[5], info[6], info[7],
                    FormatFlavour(info[8], info[9], info[10]), FormatToppings(info[11], info[12], info[13]));

                string FormatFlavour(string part1, string part2, string part3)
                {
                    return $"{part1}{(string.IsNullOrWhiteSpace(part2) ? "" : $",{part2}")}{(string.IsNullOrWhiteSpace(part3) ? "" : $",{part3}")}";
                }

                string FormatToppings(string part1, string part2, string part3)
                {
                    return $"{part1}{(string.IsNullOrWhiteSpace(part2) ? "" : $",{part2}")}{(string.IsNullOrWhiteSpace(part3) ? "" : $",{part3}")}";
                }


            }
        }
    }
}



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
    PointCard pointCard = new PointCard(0, 0);
    pointCard.tier = "Ordinary";
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
    ReadCustomerCSV();

    // prompt user to select a customer and retrieve the selected customer
    Console.Write("Select a customer (enter Customer ID): ");

    if (int.TryParse(Console.ReadLine(), out int cus_id))
    {
        // find customer
        Customer selectedCustomer = customerList.FirstOrDefault(customer => customer.memberId == cus_id);

        if (selectedCustomer != null)
        {
            // create a new order for selected customer
            Order existingOrder = selectedCustomer.MakeOrder();

            // Prompt user if they want to add another ice cream to the order
            while (true)
            {
                Console.WriteLine("Do you want to add an ice cream order? ('y' / 'n')");
                string yesno = Console.ReadLine();

                if (yesno.ToUpper() == "n")
                {
                    break;
                }
                else
                {
                    // Create a new ice cream object
                    IceCream iceCream = iceCreamOrder();
                    existingOrder.AddIceCream(iceCream);
                }
            }

            // Link the new order to the customer's current order
            selectedCustomer.currentOrder = existingOrder;

            // Check if the customer has a PointCard and it has a tier
            if (selectedCustomer.rewards != null && selectedCustomer.rewards.tier == "Gold")
            {
                goldOrderQueue.Add(existingOrder);
            }
            else
            {
                regularOrderQueue.Add(existingOrder);
            }

            // Display message
            Console.WriteLine("Order has been made successfully!");
        }
        else
        {
            Console.WriteLine("Invalid customer ID. Please try again.");
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter a valid integer.");
    }
}


//Option 5: 
void Option5()
{
    ReadOrdersCSV();

    // list customers member id
    Console.WriteLine("List of Customers:");
    foreach (int memberId in ordersDictionary.Keys)
    {
        Console.WriteLine($" Member ID: {memberId}");
    }

    // prompt user to select a customer
    Console.Write("Enter the Member ID to retrieve order details: ");
    int selectedMemberId;
    if (int.TryParse(Console.ReadLine(), out selectedMemberId))
    {
        if (ordersDictionary.ContainsKey(selectedMemberId))
        {
            List<Order> customerOrders = ordersDictionary[selectedMemberId];

            // Display details for each order using a for loop
            for (int i = 0; i < customerOrders.Count; i++)
            {
                Order order = customerOrders[i];

                Console.WriteLine($"Order ID: {order.Id}");
                Console.WriteLine($"Time Received: {order.timeReceived}");
                if (order.timeFulfilled.HasValue)
                {
                    Console.WriteLine($"Time Fulfilled: {order.timeFulfilled.Value}");
                }

                // Display ice cream details
                foreach (IceCream iceCream in order.iceCreamList)
                {
                    Console.WriteLine($"Ice Cream Details:");
                    Console.WriteLine($"  - Option: {iceCream.option}");
                    Console.WriteLine($"  - Scoops: {iceCream.scoops}");
                    if (iceCream is Cone cone)
                    {
                        Console.WriteLine($"  - Dipped: {cone.dipped}");
                    }
                    else if (iceCream is Waffle waffle)
                    {
                        Console.WriteLine($"  - Waffle Flavour: {waffle.waffleFlavour}");
                    }

                    Console.WriteLine("  - Flavours:");
                    foreach (Flavour flavour in iceCream.flavours)
                    {
                        Console.WriteLine($"    - {flavour.type} (Premium: {flavour.premium})");
                    }

                    Console.WriteLine("  - Toppings:");
                    foreach (Topping topping in iceCream.toppings)
                    {
                        Console.WriteLine($"    - {topping.type}");
                    }

                    Console.WriteLine();
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid Member ID. No orders found for the specified customer.");
        }
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter a valid Member ID.");
    }
}

//Option 6: 