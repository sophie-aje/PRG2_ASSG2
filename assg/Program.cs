//basic features
//nehaa: option 1,3,4
//sophie: option 2,5,6

// orders.csv meant for order histories (inlcudes time received and time fulfilled)

using assg;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

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

// List to store ice creams
List<IceCream> iceCreamList = new List<IceCream>();



Dictionary<int, List<IceCream>> ordersHistoryDictionary = new Dictionary<int, List<IceCream>>();
Dictionary<int, List<Order>> orderHistoryDetails = new Dictionary<int, List<Order>>();

void ReadOrdersCSV()
{
    using (StreamReader sr = new StreamReader("orders.csv"))
    {
        sr.ReadLine();

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

            order.timeFulfilled = timeFulfilled;

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
                    flavour.premium = (info[i] == "Durian" || info[i] == "Ube" || info[i] == "Sea Salt");
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

//VALIDATION DONE
void ReadCustomerCSV()
{
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        

        // Read the second line
        string s = sr.ReadLine();

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

//VALIDATION DONE
IceCream MakeIceCreamOrder()
{
    Console.WriteLine("Ice cream Options");
    Console.WriteLine("1.Cup\n2.Cone\n3.Waffle");

    string option;
    while (true)
    {
        // prompt user to enter ice cream option
        Console.Write("Enter the ice cream option: ");

        // convert user input to an string
        option = Console.ReadLine();

        try
        {
            option = option;
            break;
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }

    int scoops;
    while (true)
    {
        // prompt user to enter ice cream order
        Console.Write("Enter number of scoops: ");

        // Attempt to convert user input to an integer
        scoops = Convert.ToInt32(Console.ReadLine());

        try
        {
            scoops = Convert.ToInt32(scoops);
            break;
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }

    int numberOfToppings;
    while (true)
    {
        // prompt user to enter number of toppings
        Console.Write("Enter the number of toppings: ");

        // Attempt to convert user input to an integer
        numberOfToppings = Convert.ToInt32(Console.ReadLine());

        try
        {
            numberOfToppings = Convert.ToInt32(numberOfToppings);
            break;
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }



    Console.WriteLine("{0,-10} {1,-10}", "Regular Flavours", "Premium Flavours (+$2 per scoop)");
    Console.WriteLine("{0,-10} {1,-10}", "Vanilla", "Durian");
    Console.WriteLine("{0,-10} {1,-10}", "Chocolate", "Ube");
    Console.WriteLine("{0,-10} {1,-10}", "Strawberry", "Sea salt");


    string l_flavourName;
    string[] word;

    List<Flavour> flavour = new List<Flavour>();
    List<Topping> toppings = new List<Topping>();

    for (int i = 0; i < scoops; i++)
    {

        Console.WriteLine("Flavours:");
        for (int j = 0; j < scoops; j++) // Assuming you have 3 flavours
        {
            Console.Write($"Enter flavour {j + 1}: ");
            string flavourName = Console.ReadLine();

            // Capitalize the first letter of the flavour name
            string c_flavourName = char.ToUpper(flavourName[0]) + flavourName.Substring(1);

            Flavour fv = new Flavour();

            // Check if the flavour name is one of the specified options
            if (c_flavourName == "Durian" || c_flavourName == "Ube" || c_flavourName == "Sea Salt")
            {
                fv.type = c_flavourName;
                fv.premium = true;
            }

            flavour.Add(fv);
        }

        Console.WriteLine("Toppings(+$1 each)\nSprinkles\nMochi\nSago\nOreos\n");

        // Prompt user to enter the number of toppings


        // Collect toppings
        for (int k = 0; k < numberOfToppings; k++)
        {
            Console.Write($"Enter topping {k + 1}: ");
            string toppingName = Console.ReadLine();

            // Check if the topping name is one of the specified options
            if (toppingName == "Sprinkles" || toppingName == "Mochi" || toppingName == "Sago" || toppingName == "Oreos")
            {
                toppings.Add(new Topping(toppingName));
            }
            else
            {
                Console.WriteLine("Invalid topping name. Please enter one of the allowed toppings.");
                i--; // Decrement i to re-enter the current topping
            }
        }

        // Creating ice cream object
        if (option == "1")
        {
            Cup new_cup = new Cup(option, scoops, flavour, toppings);
            iceCreamList.Add(new_cup);
            return new_cup;
        }
        else if (option == "2")
        {
            Console.Write("Do you want the cone dipped? (true/false): ");
            bool isDipped = bool.Parse(Console.ReadLine());
            Cone new_cone = new Cone(option, scoops, flavour, toppings, isDipped);
            iceCreamList.Add(new_cone);
            return new_cone;
        }
        else if (option == "3")
        {
            Console.WriteLine("Waffle Flavour\nOriginal\nRed Velvet\nCharcoal\nPandan");
            Console.Write("Waffle flavour: ");
            string wf = Console.ReadLine();
            Waffle new_waffle = new Waffle(option, scoops, flavour, toppings, wf);
            iceCreamList.Add(new_waffle);
            return new_waffle;
        }
        
    }
    Console.WriteLine("Error. Please key in options from 1 to 3.");
    return null;
}




//Option 1: 
//VALIDATION DONE
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
//VALIDATION DONE
void Option2()
{
    
    if (goldOrderQueue.Count != 0)
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

    if (regularOrderQueue.Count != 0)
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
//VALIDATION DONE
void Option3()
{
    //prompt user for details
    

    string name;
    while (true)
    {
        Console.Write("Enter customer name: ");

        name = Console.ReadLine();

        try
        {
            name = name;
            break;
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a valid string. Only letters allowed.");
        }
    }


    int id = 0;
    bool idCheck = true;

    while (idCheck)
    {
        try
        {
            Console.Write("Enter a 6-digit integer ID: ");
            id = Convert.ToInt32(Console.ReadLine());

            if (id >= 100000 && id <= 999999)
            {
                idCheck = false; 
            }
            else
            {
                Console.WriteLine("Error. Please enter a 6-digit ID.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Error. Please enter a valid integer.");
        }
    }

    DateTime dob = DateTime.MinValue;
    string dobString;

    while (true)
    {
        Console.Write("Enter customer data of birth in DD/MM/YYYY format: ");
        dobString = Console.ReadLine();

        if (DateTime.TryParseExact(dobString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dob))
        {
            if (dob.Day >= 1 && dob.Day <= 31 && dob.Month >= 1 && dob.Month <= 12)
            {
                Console.WriteLine($"Date of Birth: {dob:dd/MM/yyyy}");
                break;
            }
            else
            {
                Console.WriteLine("Invalid date format. Please enter the date in correct format again.");
            }
        }
        else
        {
            Console.WriteLine("Invalid date format. Please enter the date in correct format again.");
        }
    }


    //create customer object
    Customer customer = new Customer(name, id, dob);

    //create pointcard object
    PointCard pointCard = new PointCard(0, 0);
    pointCard.tier = "Ordinary";
    //assign the PointCard to Customer
    customer.rewards = pointCard;


    //append customer info into customers csv file
    string memstatus = "Ordinary";
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
//VALIDAITON DONE
void Option4()
{
    Console.WriteLine("List of Customers: \n");
    foreach (var x in customerList)
    {
        Console.WriteLine("{0, -10} {1, -10}", x.name, x.memberId);
    }

    Console.Write("\nSelect a customer (enter Customer ID): ");
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

                    IceCream iceCream = MakeIceCreamOrder();
                    newOrder.AddIceCream(iceCream);                    

                    Console.Write("Do you want to add another ice cream to the order? ('y' / 'n'): ");
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
                    newOrder.Id = selectedCustomer.memberId;
                    goldOrderQueue.Add(newOrder);
                    Console.WriteLine("Added to Gold Order Queue.");
                }
                else
                {
                    newOrder.Id = selectedCustomer.memberId;
                    Console.WriteLine("Added to Regular Order Queue.");
                    regularOrderQueue.Add(newOrder);
                }
                
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


// option 5
void Option5()
{
    Console.WriteLine("List of Customers:");
    foreach (var x in customerList)
    {
        Console.WriteLine("{0, -10} {1, -10}", x.name, x.memberId);
    }

    List<IceCream> iceCreamList;

    foreach (var kvp in ordersHistoryDictionary)
    {
        int memberId = kvp.Key;
        iceCreamList = kvp.Value;

        int pastOrdersCount = iceCreamList.Count; 

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

    }  



    Console.Write("\nEnter the Member ID to retrieve order details: ");
    int selectedMemberId;       

    if (int.TryParse(Console.ReadLine(), out selectedMemberId))
    {

        var keys = ordersHistoryDictionary.Keys.ToList();

        
        Console.WriteLine($"\nID: {order.Id} Time Received : {order.timeReceived} Time Fulfilled : {order.timeFulfilled}");

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
                    Console.WriteLine($"\nOption: Cup");
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
                }
                else if (iceCream is Cone cone)
                {
                    Console.WriteLine($"\nOption: Cone");
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
                }
                else if (iceCream is Waffle waffle)
                {

                    Console.WriteLine($"\nOption: Waffle");
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



//VALIDATION DONE
void PrintOrderDetails(List<Order> orderQueue, string queueName, int selectedMemberId)
{
    if (orderQueue.Count == null)
    {
        Console.WriteLine($"{queueName}");
        Console.WriteLine("Order information:");
        Console.WriteLine("{0,-7} {1,-20}", "ID", "Time Received");
    }    

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
        Console.WriteLine($"No orders found for Member ID in {queueName}: {selectedMemberId}");
    }
}

//VALIDATION DONE
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
//VALIDATION DONE
void Option6()
{
    Console.WriteLine("List of Customers: \n");
    foreach (var x in customerList)
    {
        Console.WriteLine("{0, -10} {1, -10}", x.name, x.memberId);
    }

    // Prompt user to select a customer
    Console.Write("Enter the Member ID to select a customer: ");
    if (int.TryParse(Console.ReadLine(), out int selectedMemberId) && ordersHistoryDictionary.TryGetValue(selectedMemberId, out List<IceCream> iceCreamList))
    {
        // Retrieve the selected customer's current order
        Order selectedOrder = new Order(); // Create an instance of the Order class
        selectedOrder.Id = selectedMemberId; // Assuming Id is used to store Member ID
        selectedOrder.iceCreamList = iceCreamList;

        Console.WriteLine($"Found orders for Member ID: {selectedMemberId}");
        Console.WriteLine("Order Information:");

        foreach (IceCream iceCream in selectedOrder.iceCreamList)
        {
            if (iceCream is Cup cup)
            {
                Console.WriteLine($"\nOption: Cup");
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
            }
            else if (iceCream is Cone cone)
            {
                Console.WriteLine($"\nOption: Cone");
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
            }
            else if (iceCream is Waffle waffle)
            {

                Console.WriteLine($"\nOption: Waffle");
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
            }
        }

        int option;
        while (true)
        {
            // prompt user to enter ice cream option
            Console.Write("Choose an action: \n [1] Modify\n [2] Add new\n [3] Delete existing: ");

            // convert user input to an string
            option = Convert.ToInt32(Console.ReadLine());

            try
            {
                option = option;
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
        }


        switch (option)
        {
            case 1:
                // Modify existing ice cream
                Console.Write("Enter the index of the ice cream to modify: ");

                // NEED KNOW HOW TO EXPLAIN
                if (int.TryParse(Console.ReadLine(), out int modifyIndex) && modifyIndex >= 1 && modifyIndex <= selectedOrder.iceCreamList.Count)
                {
                    // Call the ModifyIceCream method on the selected order
                    selectedOrder.ModifyIceCream(modifyIndex - 1);
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
                break;

            case 2:
                // Add new ice cream
                IceCream newIceCream = MakeIceCreamOrder();
                selectedOrder.AddIceCream(newIceCream);
                break;

            case 3:
                // Delete existing ice cream
                Console.Write("Enter the index of the ice cream to delete: ");
                if (int.TryParse(Console.ReadLine(), out int deleteIndex) && deleteIndex >= 1 && deleteIndex <= selectedOrder.iceCreamList.Count)
                {
                    // Call the DeleteIceCream method on the selected order
                    selectedOrder.DeleteIceCream(deleteIndex - 1);
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
                break;

            default:
                Console.WriteLine("Invalid action.");
                break;
        }

        // Display the new updated order
        Console.WriteLine("Updated Order:");
        foreach (IceCream iceCream in selectedOrder.iceCreamList)
        {
            if (iceCream is Cup cup)
            {
                Console.WriteLine($"\nOption: Cup");
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
            }
            else if (iceCream is Cone cone)
            {
                Console.WriteLine($"\nOption: Cone");
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
            }
            else if (iceCream is Waffle waffle)
            {

                Console.WriteLine($"\nOption: Waffle");
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
            }
        }

    }
    else
    {
        Console.WriteLine("Invalid input. Please enter a valid Member ID.");
    }
}

// OPTION 7
//option 7
void Option7()
{

    // Check if there are orders in the queue
    if (goldOrderQueue.Count > 0)
    {
        Queue<Order> orderqueue = new Queue<Order>(goldOrderQueue);
        // Dequeue the first order
        Order currentOrder = orderqueue.Dequeue();

        // Display all ice creams in the order
        Console.WriteLine("Ice Creams in the Order:");
        foreach (IceCream iceCream in currentOrder.iceCreamList)
        {

            if (iceCream is Cup cup)
            {
                Console.WriteLine($"\nOption: Cup");
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
            }
            else if (iceCream is Cone cone)
            {
                Console.WriteLine($"\nOption: Cone");
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
            }
            else if (iceCream is Waffle waffle)
            {

                Console.WriteLine($"\nOption: Waffle");
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
            }
        }

        // Display total bill amount
        double totalBill = currentOrder.CalculateTotal();
        Console.WriteLine($"Total Bill Amount: ${totalBill:F2}");

        // Display customer's membership status and points
        Customer currentCustomer = customerList.FirstOrDefault(c => c.memberId == currentOrder.Id);
        if (currentCustomer != null)
        {
            Console.WriteLine($"Membership Status: {currentCustomer.rewards.tier}");
            Console.WriteLine($"Points: {currentCustomer.rewards.points}");

            // Check if it's the customer's birthday
            if (currentCustomer.IsBirthday())
            {
                // Find the most expensive ice cream
                IceCream mostExpensiveIceCream = currentOrder.iceCreamList.OrderByDescending(ic => ic.CalculatePrice()).FirstOrDefault();


                // Set its cost to $0.00
                if (mostExpensiveIceCream != null)
                {
                    mostExpensiveIceCream.CalculatePrice();
                }
            }

            // Check if punch card is completed
            if (currentCustomer.rewards.punchCard >= 10)
            {
                // Set the cost of the first ice cream in the order to $0.00
                currentCustomer.rewards.Punch();
            }

            // Check Pointcard status for redeeming points
            if (currentCustomer.rewards.tier == "Silver" || currentCustomer.rewards.tier == "Gold")
            {
                currentCustomer.rewards.RedeemPoints(totalBill);
                Console.Write("Press any key to make payment...");
                Console.ReadKey();


                // Example: Display a payment confirmation message
                Console.WriteLine("\nPayment successful! Thank you for your purchase.");

                // Increment punch card for every ice cream in the order
                foreach (IceCream iceCream in currentOrder.iceCreamList)
                {
                    currentCustomer.rewards.Punch();

                    // Earn points and upgrade membership status accordingly

                    int points = Convert.ToInt32(Math.Floor(totalBill * 0.72));
                    currentCustomer.rewards.AddPoints(points);

                    if (currentCustomer.rewards.points >= 100)
                    {
                        currentCustomer.rewards.tier = "Gold";
                    }
                    else if (currentCustomer.rewards.points >= 50)
                    {
                        currentCustomer.rewards.tier = "Silver";
                    }
                    else
                    {
                        currentCustomer.rewards.tier = "Ordinary";
                    }
                }

                // Mark the order as fulfilled with the current datetime
                currentOrder.timeFulfilled = DateTime.Now;

                // Add the fulfilled order object to the customer's order history
                currentCustomer.orderHistory.Add(currentOrder);

                Console.WriteLine("Order fulfilled and processed successfully.");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }
        else
        {
            Console.WriteLine("No orders in the queue.");
        }

        int orderID = 0;
        using (StreamReader sr = new StreamReader("orders.csv"))
        {
            string? s = sr.ReadLine(); // read the heading
                                       // display the heading
            if (s != null)
            {
                
            }
            while ((s = sr.ReadLine()) != null)
            {
                string[] content = s.Split(',');
                orderID = Convert.ToInt32(content[0]) + 1;
            }
        }

        


        using (StreamWriter sw = new StreamWriter("orders.csv", true))
        {
            foreach (IceCream icecream in currentOrder.iceCreamList)
            {
                if (icecream is Cup cup)
                {
                    var f1 = "";
                    var f2 = "";
                    var f3 = "";

                    int count1 = 0;
                    foreach (Flavour flavour in cup.flavours)
                    {
                        count1 ++;
                        if (count1 == 1)
                        {
                            f1 = flavour.type;
                        }
                        else if (count1 == 2)
                        {
                            f2 = flavour.type;
                        }
                        else if (count1 == 3)
                        {
                            f3 = flavour.type;
                        }

                    }

                    var t1 = "";
                    var t2 = "";
                    var t3 = "";

                    int count2 = 0;
                    foreach (Topping topping in cup.toppings)
                    {
                        count2 ++;
                        if (count2 == 1)
                        {
                            t1 = topping.type;
                        }
                        else if (count2 == 2)
                        {
                            t2 = topping.type;
                        }
                        else if (count2 == 3)
                        {
                            t3 = topping.type;
                        }

                    }

                    sw.WriteLine($"{orderID},{currentCustomer.memberId},{currentOrder.timeReceived},{currentOrder.timeFulfilled},{"Cup"},{cup.scoops},{""},{""},{f1},{f2},{f3},{t1},{t2},{t3}");
                    
                }
                else if (icecream is Cone cone)
                {
                    var f1 = "";
                    var f2 = "";
                    var f3 = "";

                    int count1 = 0;
                    foreach (Flavour flavour in cone.flavours)
                    {
                        count1++;
                        if (count1 == 1)
                        {
                            f1 = flavour.type;
                        }
                        else if (count1 == 2)
                        {
                            f2 = flavour.type;
                        }
                        else if (count1 == 3)
                        {
                            f3 = flavour.type;
                        }

                    }

                    var t1 = "";
                    var t2 = "";
                    var t3 = "";

                    int count2 = 0;
                    foreach (Topping topping in cone.toppings)
                    {
                        count2++;
                        if (count2 == 1)
                        {
                            t1 = topping.type;
                        }
                        else if (count2 == 2)
                        {
                            t2 = topping.type;
                        }
                        else if (count2 == 3)
                        {
                            t3 = topping.type;
                        }

                    }

                    sw.WriteLine($"{orderID},{currentCustomer.memberId},{currentOrder.timeReceived},{currentOrder.timeFulfilled},{"Cone"},{cone.scoops},{cone.dipped},{""},{f1},{f2},{f3},{t1},{t2},{t3}");
                }
                else if (icecream is Waffle waffle)
                {
                    var f1 = "";
                    var f2 = "";
                    var f3 = "";

                    int count1 = 0;
                    foreach (Flavour flavour in waffle.flavours)
                    {
                        count1++;
                        if (count1 == 1)
                        {
                            f1 = flavour.type;
                        }
                        else if (count1 == 2)
                        {
                            f2 = flavour.type;
                        }
                        else if (count1 == 3)
                        {
                            f3 = flavour.type;
                        }

                    }

                    var t1 = "";
                    var t2 = "";
                    var t3 = "";

                    int count2 = 0;
                    foreach (Topping topping in waffle.toppings)
                    {
                        count2++;
                        if (count2 == 1)
                        {
                            t1 = topping.type;
                        }
                        else if (count2 == 2)
                        {
                            t2 = topping.type;
                        }
                        else if (count2 == 3)
                        {
                            t3 = topping.type;
                        }

                    }

                    sw.WriteLine($"{orderID},{currentCustomer.memberId},{currentOrder.timeReceived},{currentOrder.timeFulfilled},{"Waffle"},{waffle.scoops},{""},{waffle.waffleFlavour},{f1},{f2},{f3},{t1},{t2},{t3}");
                }
            }
        }
    }

}

// option 8
//VALIDATION DONE
void Option8()
{
    // prompt the user for the year
    
    int input_year;
    while (true)
    {
        // prompt user to enter ice cream option
        Console.Write("\nEnter the year: ");
        // convert user input to an string
        input_year = Convert.ToInt32(Console.ReadLine());

        try
        {
            input_year = input_year;
            break;
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a valid integer. example: 2023");
        }
    }


    Dictionary<string, List<double>> totalCharge = new Dictionary<string, List<double>>
    {
        {"Jan", new List<double>()},
        {"Feb", new List<double>()},
        {"Mar", new List<double>()},
        {"Apr", new List<double>()},
        {"May", new List<double>()},
        {"Jun", new List<double>()},
        {"Jul", new List<double>()},
        {"Aug", new List<double>()},
        {"Sep", new List<double>()},
        {"Oct", new List<double>()},
        {"Nov", new List<double>()},
        {"Dec", new List<double>()}
    };


    // Assuming your IceCream class has a constructor that takes option and scoops
    IceCream iceCream;

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
            order.timeFulfilled = timeFulfilled;

            // Add ice cream details to the order
            string option = info[4];
            int scoops = Convert.ToInt32(info[5]);

            if (option == "Cone")
            {
                bool dipped = info[6].Equals("TRUE", StringComparison.OrdinalIgnoreCase);
                iceCream = new Cone { dipped = dipped, option = option, scoops = scoops };
            }
            else if (option == "Waffle")
            {
                string waffleFlavour = info[7];
                iceCream = new Waffle { waffleFlavour = waffleFlavour, option = option, scoops = scoops };
            }
            else // Default to Cup if no valid option is found
            {
                iceCream = new Cup { option = option, scoops = scoops };
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
                    flavour.premium = (info[i] == "Durian" || info[i] == "Ube" || info[i] == "Sea Salt");
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

            double price;

            // Function to get the month key based on the numeric value
            static string Month(int monthValue)
            {
                switch (monthValue)
                {
                    case 1: return "Jan";
                    case 2: return "Feb";
                    case 3: return "Mar";
                    case 4: return "Apr";
                    case 5: return "May";
                    case 6: return "Jun";
                    case 7: return "Jul";
                    case 8: return "Aug";
                    case 9: return "Sep";
                    case 10: return "Oct";
                    case 11: return "Nov";
                    case 12: return "Dec";
                    default: throw new ArgumentOutOfRangeException(nameof(monthValue), "Invalid month value");
                }
            }

            if (timeFulfilled.Year == input_year)
            {
                int month = timeFulfilled.Month;
                string monthKey = Month(month);
                price = iceCream.CalculatePrice();
                totalCharge[monthKey].Add(price);
            }       
        }
    }

    foreach (var m in totalCharge)
    {
        string month = m.Key;
        List<double> charges = m.Value;

        double count = 0;
        foreach (var charge in charges)
        {
            count += charge;
        }

        Console.WriteLine($"{month} {input_year}: ${count.ToString("0.00")}");
    }
}



// display menu
//VALIDATION DONE
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
        Console.WriteLine("[7] Process an order and checkout");
        Console.WriteLine("[8] Display monthly charged amounts breakdown & total charged amounts for the year");
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
                    Option6();
                    break;
                case 7:
                    Option7();
                    break;
                case 8:
                    Option8();
                    break;
                case 0:
                    return;
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