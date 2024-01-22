//basic features
//nehaa: option 1,3,4
//sophie: option 2,5,6

// orders.csv meant for order histories (inlcudes time received and time fulfilled)

using assg;
using System;
using System.Collections.Specialized;
using System.Xml.Linq;

// list for customer info
List<Customer> customerList = new List<Customer>();

List<IceCream> orderList = new List<IceCream>();
//List<IceCream> orderHistoryList = new List<IceCream>();


// lists to append orders
List<Order> regularOrderQueue = new List<Order>();
List<Order> goldOrderQueue = new List<Order>();

// list to store new order info
Order order = new Order();
List<IceCream> iceCreamOrder = order.iceCreamList; 

// list to hold pointcard information
List<PointCard> pointCards = new List<PointCard>();


List<IceCream> orderHistoryList = new List<IceCream>();

Dictionary<int, List<IceCream>> ordersHistoryDictionary = new Dictionary<int, List<IceCream>>();

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

            IceCream iceCream;

            if (option == "Cone")
            {
                bool dipped = info[6].Equals("TRUE", StringComparison.OrdinalIgnoreCase);
                iceCream = new Cone { dipped = dipped };
            }
            else if (option == "Waffle")
            {
                string waffleFlavour = info[7];
                iceCream = new Waffle { waffleFlavour = waffleFlavour };
            }
            else // Default to Cup if no valid option is found
            {
                iceCream = new Cup();
            }

            iceCream.scoops = scoops;

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

            iceCream.flavours.AddRange(flavours);
            iceCream.toppings.AddRange(toppings);

            order.iceCreamList.Add(iceCream);

            if (ordersHistoryDictionary.ContainsKey(memberId))
            {
                ordersHistoryDictionary[memberId].Add(iceCream);
            }
            else
            {
                ordersHistoryDictionary[memberId] = new List<IceCream> { iceCream };
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
                customer.rewards = pointCard;
                pointCards.Add(pointCard);
            }
        }

    }
ReadCustomerCSV();


//---BASIC FEATURES---

//method to make an icecream order
IceCream MakeIceCreamOrder()
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
    
    if (goldOrderQueue.Count != null)
    {
        Console.WriteLine("GOLD MEMBER QUEUE");
        Console.WriteLine("Order information:");
        Console.WriteLine("{0,-7}{1,-20}", "ID", "Time Received");

        foreach (Order order in goldOrderQueue)
        {
            Console.WriteLine("{0,-7}{1,-20}", order.Id, order.timeReceived); // Print order details

            foreach (IceCream iceCream in order.iceCreamList)
            {
                Console.WriteLine("Ice Cream information:");
                Console.WriteLine($"Option: {iceCream.option}");
                Console.WriteLine($"Scoops: {iceCream.scoops}");
                Console.WriteLine($"Flavour(s) ");

                // Assuming flavours is a List<Flavour>
                Console.WriteLine("\n{0,-10} {1,-10}", "Type", "Premium");
                for (int i = 0; i < iceCream.flavours.Count; i++)
                {
                    Flavour flavour = iceCream.flavours[i];
                    Console.WriteLine("{0,-10} {1,-10}", flavour.type, flavour.premium);
                }

                // Assuming toppings is an array of Topping
                for (int i = 1; i < iceCream.toppings.Count; i++)
                {
                    Topping topping = iceCream.toppings[i];
                    Console.WriteLine($"Topping {i}: {topping.type}");
                }

            }
        }
    }
    else
    {
        Console.WriteLine("Nothing in Gold Queue.");
    }

    if (regularOrderQueue != null)
    {
        Console.WriteLine("\nREGULAR MEMBER QUEUE");
        Console.WriteLine("Order Information:");
        Console.WriteLine("{0,-7}{1,-20}", "ID", "Time Received");

        foreach (Order order in regularOrderQueue)
        {
            Console.WriteLine("{0,-7}{1,-20}{2,-20}",order.Id, order.timeReceived); // Print order details

            foreach (IceCream iceCream in order.iceCreamList)
            {
                Console.WriteLine("Ice Cream");
                Console.WriteLine($"Option: {iceCream.option}");
                Console.WriteLine($"Scoops: {iceCream.scoops}");

                string f1;
                string f2;
                string f3;

                string t1;
                string t2;
                string t3;

                foreach (Flavour flavour in iceCream.flavours)
                {
                    Console.WriteLine($"Flavours: {flavour.type}");
                    Console.WriteLine($"Is flavour premium? {flavour.premium}");
                }


                foreach (Topping topping in iceCream.toppings)
                {
                    Console.WriteLine($"Toppings: {topping.type}");
                }
            }
        }
    }
    else
    {
        Console.WriteLine("Nothing in Regular Queue.");
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
    foreach (var abc in customerList)
    {
        Console.WriteLine("{0, -15} {1, -15}", abc.name, abc.memberId);
    }

    Console.Write("Select a customer (enter Customer ID): ");
    if (int.TryParse(Console.ReadLine(), out int cus_id))
    {
        // find customer
        Customer selectedCustomer = customerList.FirstOrDefault(customer => customer.memberId == cus_id);

        if (selectedCustomer != null)
        {
            // Additional details of the customer
            string customerName = selectedCustomer.name;
            DateTime customerDob = selectedCustomer.dob;
            PointCard customerRewards = selectedCustomer.rewards;

            // Now you can use the additional details as needed
            Console.WriteLine($"Customer Name: {customerName}");
            Console.WriteLine($"Customer Date of Birth: {customerDob}");

            // Check if the customer has a PointCard
            if (customerRewards != null)
            {
                // Access other details from the PointCard
                string tierLevel = customerRewards.tier;
                Console.WriteLine($"Customer Tier Level: {tierLevel}");

                // create a new order for the selected customer
                Order newOrder = new Order(cus_id, DateTime.Now);

                // Prompt user to enter their ice cream order
                while (true)
                {
                    Console.WriteLine("---- Enter your ice cream order details ----");

                    // Create a new ice cream object
                    IceCream iceCream = MakeIceCreamOrder(); // Assuming you have a function named MakeIceCreamOrder
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

                // Check if the customer has a Gold tier
                if (tierLevel == "Gold")
                {                    
                    goldOrderQueue.Add(newOrder);
                    Console.WriteLine("Added to Gold Order Queue.");
                }
                else
                {
                    Console.WriteLine("Added to Regular Order Queue.");
                    regularOrderQueue.Add(newOrder);
                }

                // Display message
                Console.WriteLine("Order has been made successfully!");
            }            
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


void Option5()
{
    Console.WriteLine("List of Customers:");

    Console.WriteLine("Number of PAST orders per member");
    List<IceCream> iceCreamList;
    foreach (var kvp in ordersHistoryDictionary)
    {
        int memberId = kvp.Key;
        iceCreamList = kvp.Value;

        int pastOrdersCount = iceCreamList.Count; // Corrected variable name to iceCreamList

        int currentOrdersCount = 0;

        foreach (Order order in goldOrderQueue)
        { 
            if (order.Id == memberId)
            {
                currentOrdersCount++;
            }
        }

        foreach (Order orders in regularOrderQueue)
        {
            if (orders.Id == memberId)
            {
                currentOrdersCount++;
            }
        }

        Console.WriteLine($"\nMember ID: {memberId}, Number of Orders: {pastOrdersCount}");
        Console.WriteLine($"Member ID: {memberId}, Number of Orders: {currentOrdersCount}");
    }

   



    Console.Write("Enter the Member ID to retrieve order details: ");
    int selectedMemberId;

    if (int.TryParse(Console.ReadLine(), out selectedMemberId))
    {
        Console.WriteLine("Displaying orders from PAST HISTORY queues...");
        if (ordersHistoryDictionary.ContainsKey(selectedMemberId))
        {
            iceCreamList = ordersHistoryDictionary[selectedMemberId];

            Console.WriteLine($"Order details for Member ID: {selectedMemberId}");
            Console.WriteLine($"{iceCreamList.Count}");
            foreach (IceCream iceCream in iceCreamList)
            {
                if (iceCream is Cup cup)
                {
                    Console.WriteLine("Cup Details:");
                    Console.WriteLine($"Option: {cup.option}");
                    Console.WriteLine($"Scoops: {cup.scoops}");
                    Console.WriteLine("Flavours:");
                    foreach (Flavour flavour in cup.flavours)
                    {
                        Console.WriteLine($"  - {flavour.type} {(flavour.premium ? "(Premium)" : "")}");
                    }

                    Console.WriteLine("Toppings:");
                    foreach (Topping topping in cup.toppings)
                    {
                        Console.WriteLine($"  - {topping.type}");
                    }

                    // Add any other Cup-specific details you want to display
                }
                else if (iceCream is Cone cone)
                {
                    Console.WriteLine("Cone Details:");
                    Console.WriteLine($"Option: {cone.option}");
                    Console.WriteLine($"Scoops: {cone.scoops}");
                    Console.WriteLine($"Dipped: {cone.dipped}");
                    Console.WriteLine("Flavours:");
                    foreach (Flavour flavour in cone.flavours)
                    {
                        Console.WriteLine($"  - {flavour.type} {(flavour.premium ? "(Premium)" : "")}");
                    }

                    Console.WriteLine("Toppings:");
                    foreach (Topping topping in cone.toppings)
                    {
                        Console.WriteLine($"  - {topping.type}");
                    }

                    // Add any other Cone-specific details you want to display
                }
                else if (iceCream is Waffle waffle)
                {
                    Console.WriteLine("Waffle Details:");
                    Console.WriteLine($"Option: {waffle.option}");
                    Console.WriteLine($"Scoops: {waffle.scoops}");
                    Console.WriteLine($"Waffle Flavour: {waffle.waffleFlavour}");
                    Console.WriteLine("Flavours:");
                    foreach (Flavour flavour in waffle.flavours)
                    {
                        Console.WriteLine($"  - {flavour.type} {(flavour.premium ? "(Premium)" : "")}");
                    }

                    Console.WriteLine("Toppings:");
                    foreach (Topping topping in waffle.toppings)
                    {
                        Console.WriteLine($"  - {topping.type}");
                    }

                    // Add any other Waffle-specific details you want to display
                }
            }
            Console.WriteLine("\n\nDisplaying orders from CURRENT queues...");
            PrintOrderDetails(goldOrderQueue, "GOLD MEMBER QUEUE", selectedMemberId);
            PrintOrderDetails(regularOrderQueue, "REGULAR MEMBER QUEUE", selectedMemberId);

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



void PrintOrderDetails(List<Order> orderQueue, string queueName, int selectedMemberId)
{
    Console.WriteLine($"{queueName}");
    Console.WriteLine("Order information:");
    Console.WriteLine("{0,-7}{1,-20}", "ID", "Time Received");

    bool foundMember = false;

    foreach (Order order in orderQueue)
    {
        if (order.Id == selectedMemberId)
        {
            foundMember = true;
            Console.WriteLine("{0,-7}{1,-20}", order.Id, order.timeReceived); // Print order details

            foreach (IceCream iceCream in order.iceCreamList)
            {
                Console.WriteLine("Ice Cream information:");
                Console.WriteLine($"Option: {iceCream.option}");
                Console.WriteLine($"Scoops: {iceCream.scoops}");
                Console.WriteLine($"Flavour(s) ");

                // Assuming flavours is a List<Flavour>
                Console.WriteLine("\n{0,-10} {1,-10}", "Type", "Premium");
                for (int i = 0; i < iceCream.flavours.Count; i++)
                {
                    Flavour flavour = iceCream.flavours[i];
                    Console.WriteLine("{0,-10} {1,-10}", flavour.type, flavour.premium);
                }

                // Assuming toppings is an array of Topping
                for (int i = 0; i < iceCream.toppings.Count; i++)
                {
                    Topping topping = iceCream.toppings[i];
                    Console.WriteLine($"Topping {i + 1}: {topping.type}");
                }

            }
        }
    }

    if (!foundMember)
    {
        Console.WriteLine($"No orders found for Member ID: {selectedMemberId}");
    }
}

void DisplayIceCreamDetails(Order order, IceCream iceCream)
{
    // Display common ice cream details
    Console.WriteLine($"Option: {iceCream.option}");
    Console.WriteLine($"Scoops: {iceCream.scoops}");

    // Check ice cream type and display specific details
    if (iceCream is Cone cone)
    {
        Console.WriteLine($"Dipped: {cone.dipped}");
    }
    else if (iceCream is Waffle waffle)
    {
        Console.WriteLine($"Waffle Flavour: {waffle.waffleFlavour}");
    }

    // Display flavours
    Console.Write("Flavours: ");
    foreach (Flavour flavour in iceCream.flavours)
    {
        Console.Write($"{flavour.type} ");
    }
    Console.WriteLine();

    // Display toppings
    Console.Write("Toppings: ");
    foreach (Topping topping in iceCream.toppings)
    {
        Console.Write($"{topping.type} ");
    }
    Console.WriteLine();
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
                    //Option6();
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