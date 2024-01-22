//basic features
//nehaa: option 1,3,4
//sophie: option 2,5,6

// orders.csv meant for order histories (inlcudes time received and time fulfilled)

using assg;
using System;
using System.Xml.Linq;

// list for customer info
List<Customer> customerList = new List<Customer>();

List<IceCream> orderList = new List<IceCream>();

// lists to append orders
List<Order> regularOrderQueue = new List<Order>();
List<Order> goldOrderQueue = new List<Order>();

// list to store new order info
Order order = new Order();
List<IceCream> iceCreamOrder = order.iceCreamList; 

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
            string[] info = s.Split(',');

            int orderId = Convert.ToInt32(info[0]);
            int memberId = Convert.ToInt32(info[1]);
            DateTime.TryParse(info[2], out DateTime timeReceived);
            DateTime.TryParse(info[3], out DateTime timeFulfilled);

            // Create an Order object
            Order order = new Order(orderId, timeReceived);

            // Assign time fulfilled
            order.timeFulfilled = timeFulfilled;

            // Add ice cream details to the order
            string option = info[4];
            int scoops = Convert.ToInt32(info[5]);

            if (option == "Cone")
            {
                bool dipped = info[6].Equals("TRUE", StringComparison.OrdinalIgnoreCase);
                Cone cone = new Cone();
                cone.dipped = dipped;
                order.iceCreamList.Add(cone);
            }
            else if (option == "Waffle")
            {
                string waffleFlavour = info[7];
                Waffle waffle = new Waffle();
                waffle.waffleFlavour = waffleFlavour;
                order.iceCreamList.Add(waffle);
            }

            // Additional lists for flavours and toppings
            List<Flavour> flavours = new List<Flavour>();
            List<Topping> toppings = new List<Topping>();

            for (int i = 8; i <= 10; i++)
            {
                if (!string.IsNullOrEmpty(info[i]))
                {
                    Flavour flavour = new Flavour();
                    flavour.type = info[i];
                    flavour.premium = (info[i] == "durian" || info[i] == "ube" || info[i] == "sea salt");
                    flavour.quantity = 1;
                    flavours.Add(flavour);
                }
            }

            for (int i = 11; i <= 14; i++)
            {
                if (!string.IsNullOrEmpty(info[i]))
                {
                    Topping topping = new Topping();
                    topping.type = info[i];
                    toppings.Add(topping);
                }
            }

            if (order.iceCreamList.Any())
            {
                order.iceCreamList[0].flavours.AddRange(flavours);
                order.iceCreamList[0].toppings.AddRange(toppings);
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

ReadOrdersCSV();





void ReadCustomerCSV()
    {
        using (StreamReader sr = new StreamReader("customers.csv"))
        {
            sr.ReadLine(); 
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
ReadCustomerCSV();


//---BASIC FEATURES---

//method to make an icecream order
IceCream MakeiceCreamOrder()
{
    // List to store ice creams
    List<IceCream> iceCreamList = new List<IceCream>();

    // prompt user to enter ice cream order
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

    // Creating ice cream object
    if (option == "1")
    {
        Cup new_cup = new Cup(option, scoops, flavours, toppings);
        iceCreamList.Add(new_cup);
        return new_cup;
    }
    else if (option == "2")
    {
        Console.Write("Do you want the cone dipped? (true/false): ");
        bool isDipped = bool.Parse(Console.ReadLine());
        Cone new_cone = new Cone(option, scoops, flavours, toppings, isDipped);
        iceCreamList.Add(new_cone);
        return new_cone;
    }
    else if (option == "3")
    {
        Console.Write("What waffle flavour do you want? (Original/Red Velvet/Charcoal/Pandan): ");
        string wf = Console.ReadLine();
        Waffle new_waffle = new Waffle(option, scoops, flavours, toppings, wf);
        iceCreamList.Add(new_waffle);
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
    // Display orders in the queues
    if (goldOrderQueue.Count != null)
    {
        Console.WriteLine("Gold Order Queue:");
        foreach (var x in goldOrderQueue)
        {
            Console.WriteLine(x.ToString());
        }
    }

    if (regularOrderQueue.Count != null)
    {
        Console.WriteLine("Regular Order Queue:");
        foreach (var y in regularOrderQueue)
        {
            Console.WriteLine(y.ToString());
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
    Console.WriteLine("{0,-10} {1, -10}", "Name", "Member Id");

    foreach (var xyz in customerList)
    {
        Console.WriteLine("{0,-10} {1, -10}", xyz.name, xyz.memberId);
    }

    // prompt user to select a customer and retrieve the selected customer
    Console.Write("Select a customer (enter Customer ID): ");

    if (int.TryParse(Console.ReadLine(), out int cus_id))
    {
        // find customer
        Customer selectedCustomer = customerList.FirstOrDefault(customer => customer.memberId == cus_id);

        if (selectedCustomer != null)
        {
            // create a new order for the selected customer
            Order newOrder = new Order(cus_id, DateTime.Now);

            // Prompt user to enter their ice cream order
            while (true)
            {
                Console.WriteLine("---- Enter your ice cream order details ----");

                // Create a new ice cream object
                IceCream iceCream = MakeiceCreamOrder(); // Define or replace this function
                newOrder.AddIceCream(iceCream);

                // Prompt user if they want to add another ice cream to the order
                Console.WriteLine("Do you want to add another ice cream to the order? ('y' / 'n')");
                string yesno = Console.ReadLine();

                if (yesno.ToLower() == "n")
                {
                    break;
                }
            }

            // Link the new order to the customer's current order
            selectedCustomer.currentOrder = newOrder;

            // Check if the customer has a PointCard and it has a tier
            if (selectedCustomer.rewards != null && selectedCustomer.rewards.tier == "Gold")
            {
                goldOrderQueue.Add(newOrder);
            }
            else
            {
                regularOrderQueue.Add(newOrder);
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
    Console.WriteLine("List of Customers:");
    foreach (int memberId in ordersDictionary.Keys)
    {
        Console.WriteLine($" Member ID: {memberId}");
    }

    Console.Write("Enter the Member ID to retrieve order details: ");
    int selectedMemberId;

    if (int.TryParse(Console.ReadLine(), out selectedMemberId))
    {
        if (ordersDictionary.ContainsKey(selectedMemberId))
        {
            List<Order> customerOrders = ordersDictionary[selectedMemberId];

            Console.WriteLine("{0,-5} {1,-10} {2,-18} {3,-18} {4,-10} {5,-6} {6,-6} {7,-15} {8,-10} {9,-10} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10}",
                "Id", "MemberId", "TimeReceived", "TimeFulfilled", "Option", "Scoops", "Dipped", "WaffleFlavour", "Flavour1", "Flavour2", "Flavour3", "Topping1", "Topping2", "Topping3", "Topping4");

            for (int i = 0; i < customerOrders.Count; i++)
            {
                Order order = customerOrders[i];

                foreach (IceCream iceCream in order.iceCreamList)
                {
                    if (iceCream is Cup cup)
                    {
                        string[] f = new string[3];

                        for (int x = 0; x < iceCream.flavours.Count && x < f.Length; x++)
                        {
                            if (iceCream.flavours[x].type != null)
                            {
                                f[x] = iceCream.flavours[x].type;
                            }
                            else
                            {
                                f[x] = "";
                            }
                        }

                        string[] t = new string[4];

                        for (int y = 0; y < iceCream.toppings.Count && y < t.Length; y++)
                        {
                            if (iceCream.toppings[y].type != null)
                            {
                                t[y] = iceCream.toppings[y].type;
                            }
                            else
                            {
                                t[y] = "";
                            }
                        }

                        Console.WriteLine("{0,-5} {1,-10} {2,-18} {3,-18} {4,-10} {5,-6} {6,-6} {7,-15} {8,-10} {9,-10} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10}",
                            order.Id, order.timeReceived, order.timeFulfilled, iceCream.option, iceCream.scoops, "", "", f[0], f[1], f[2], t[0], t[1], t[2], t[3]);
                    }
                    else if (iceCream is Cone cone)
                    {
                        string[] f = new string[3];

                        for (int x = 0; x < iceCream.flavours.Count && x < f.Length; x++)
                        {
                            if (iceCream.flavours[x].type != null)
                            {
                                f[x] = iceCream.flavours[x].type;
                            }
                            else
                            {
                                f[x] = "";
                            }
                        }

                        string[] t = new string[4];

                        for (int y = 0; y < iceCream.toppings.Count && y < t.Length; y++)
                        {
                            if (iceCream.toppings[y].type != null)
                            {
                                t[y] = iceCream.toppings[y].type;
                            }
                            else
                            {
                                t[y] = "";
                            }
                        }

                        Console.WriteLine("{0,-5} {1,-10} {2,-18} {3,-18} {4,-10} {5,-6} {6,-6} {7,-15} {8,-10} {9,-10} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10}",
                            order.Id, order.timeReceived, order.timeFulfilled, iceCream.option, iceCream.scoops, cone.dipped, "", f[0], f[1], f[2], t[0], t[1], t[2], t[3]);
                    }
                    else if (iceCream is Waffle waffle)
                    {
                        string[] f = new string[3];

                        for (int x = 0; x < iceCream.flavours.Count && x < f.Length; x++)
                        {
                            if (iceCream.flavours[x].type != null)
                            {
                                f[x] = iceCream.flavours[x].type;
                            }
                            else
                            {
                                f[x] = "";
                            }
                        }

                        string[] t = new string[4];

                        for (int y = 0; y < iceCream.toppings.Count && y < t.Length; y++)
                        {
                            if (iceCream.toppings[y].type != null)
                            {
                                t[y] = iceCream.toppings[y].type;
                            }
                            else
                            {
                                t[y] = "";
                            }
                        }

                        Console.WriteLine("{0,-5} {1,-10} {2,-18} {3,-18} {4,-10} {5,-6} {6,-6} {7,-15} {8,-10} {9,-10} {10,-10} {11,-10} {12,-10} {13,-10} {14,-10}",
                            order.Id, order.timeReceived, order.timeFulfilled, iceCream.option, iceCream.scoops, "", waffle.waffleFlavour, f[0], f[1], f[2], t[0], t[1], t[2], t[3]);
                    }
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





void DisplayMenu()
{
    int option;

    do
    {
        Console.WriteLine("\nThe I.C. Treats Management System (enter 0 to break)");
        Console.WriteLine("=================================");
        Console.WriteLine("[1] List all customers");
        Console.WriteLine("[2] List all current orders");
        Console.WriteLine("[3] Register a new customer");
        Console.WriteLine("[4] Create a customer's order");
        Console.WriteLine("[5] Display order details of a customer");
        Console.WriteLine("[6] Modify order details");
        Console.Write("\nEnter an option: ");

        if (int.TryParse(Console.ReadLine(), out option))
        {
            switch (option)
            {
                case 1:
                    Option1();
                    break;
                case 2:
                    Option2();
                    break;
                case 3:
                    Option3();
                    break;
                case 4:
                    Option4();
                    break;
                case 5:
                    Option5();
                    break;
                case 6:
                    // Option6();
                    break;
                case 0:
                    return; // or break; if you want to exit the loop
                default:
                    Console.WriteLine("Please enter a valid option.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Please enter a valid number for the option.");
        }
    } while (true);
}





DisplayMenu();