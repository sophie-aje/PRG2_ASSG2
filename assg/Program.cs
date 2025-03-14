// BASIC FEATURES
// Nehaa: option 1,3,4
// Sophie: option 2,5,6

// ADVANCED FEATURES
// Nehaa:  option b
// Sophie: option a


using assg;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;


// list for customer info
List<Customer> customerList = new List<Customer>();

// lists to append orders
Queue<Order> regularOrderQueue = new Queue<Order>();
Queue<Order> goldOrderQueue = new Queue<Order>();

// list to store new order info
Order order = new Order();
List<IceCream> iceCreamOrder = order.iceCreamList;

// list to hold pointcard information
List<PointCard> pointCards = new List<PointCard>();

// list to store ice creams
List<IceCream> iceCreamList = new List<IceCream>();


Dictionary<int, List<IceCream>> ordersHistoryDictionary = new Dictionary<int, List<IceCream>>();
Dictionary<int, List<Order>> orderHistoryDetails = new Dictionary<int, List<Order>>();



// method to read data from orders csv
void ReadOrdersCSV()
{
    using (StreamReader sr = new StreamReader("orders.csv")) // read info from orders.csv
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

// method to read data from customers csv
void ReadCustomerCSV()
{
    using (StreamReader sr = new StreamReader("customers.csv")) // read customer info from customers.csv
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

// method to make an ice cream order
IceCream MakeIceCreamOrder() //method to make an icecream order
{
    Console.WriteLine("\nIce Cream Options");
    Console.WriteLine("\n1.Cup\n2.Cone\n3.Waffle");

    int op;
    while (true)
    {
        // prompt user to enter ice cream option
        Console.Write("Enter the ice cream option (CHOOSE 1, 2 OR 3): ");

        try
        {
            op = Convert.ToInt32(Console.ReadLine());

            if (op >= 1 && op <= 3)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 3.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }
    string option = Convert.ToString(op);



    int scoops;
    while (true)
    {
        // prompt user to enter ice cream order
        Console.Write("Enter number of scoops (MIN 1 MAX 3): ");

        try
        {
            scoops = Convert.ToInt32(Console.ReadLine());

            if (scoops >= 1 && scoops <= 3)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 3 scoops.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }



    int numberOfToppings;
    while (true)
    {
        // prompt user to enter ice cream order
        Console.Write("Enter number of toppings (MAX 4): ");

        try
        {
            numberOfToppings = Convert.ToInt32(Console.ReadLine());

            if (numberOfToppings >= 0 && numberOfToppings <= 4)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 4 toppings");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }

    Console.WriteLine("\n---Flavours---");
    Console.WriteLine("\n{0,-17} | {1,-20}", "Regular Flavours", "Premium Flavours (+$2 per scoop)");
    Console.WriteLine("{0,-17}-|-{1,-20}", "-----------------", "---------------------------------");
    Console.WriteLine("{0,-17} | {1,-20}", "Vanilla", "Durian");
    Console.WriteLine("{0,-17} | {1,-20}", "Chocolate", "Ube");
    Console.WriteLine("{0,-17} | {1,-20}", "Strawberry", "Sea salt");

    string l_flavourName;
    string[] word;

    List<Flavour> flavour = new List<Flavour>();
    List<Topping> toppings = new List<Topping>();

    for (int i = 0; i < scoops; i++)
    {
        while (true)
        {
            try
            {
                // prompt user to enter flavour
                Console.Write($"\nEnter flavour {i + 1}: ");

                // Attempt to convert user input to an integer
                string flavName = Console.ReadLine();

                string[] words = flavName.Split(' ');

                for (int j = 0; j < words.Length; j++)
                {
                    if (!string.IsNullOrEmpty(words[j]))
                    {
                        words[j] = char.ToUpper(words[j][0]) + words[j].Substring(1);
                    }
                }
                string c_flavourName = string.Join(' ', words);


                string[] validFlavours = { "Vanilla", "Chocolate", "Strawberry", "Durian", "Ube", "Sea Salt" };
                bool valid = false;

                foreach (string f in validFlavours)
                {
                    if (string.Equals(c_flavourName, f, StringComparison.OrdinalIgnoreCase))
                    {
                        valid = true;
                        break;
                    }
                }

                if (valid)
                {
                    Console.WriteLine($"Selected flavour: {c_flavourName}");

                    Flavour fv = new Flavour();

                    // Check if the flavour name is one of the specified options
                    if (c_flavourName == "Durian" || c_flavourName == "Ube" || c_flavourName == "Sea Salt")
                    {
                        fv.type = c_flavourName;
                        fv.premium = true;
                    }
                    else
                    {
                        fv.type = c_flavourName;
                        fv.premium = false;
                    }

                    flavour.Add(fv);
                    break;
                }
                else
                {
                    Console.WriteLine("Error: Invalid input. Please enter Vanilla, Chocolate, Strawberry, Durian, Ube, Sea Salt");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid flavour name.");
            }
        }
    }

    if (numberOfToppings != 0)
    {
        Console.WriteLine("\n{0,-20}", "Toppings");
        Console.WriteLine("--------");
        Console.WriteLine("{0,-20}", "Sprinkles");
        Console.WriteLine("{0,-20}", "Mochi");
        Console.WriteLine("{0,-20}", "Sago");
        Console.WriteLine("{0,-20}", "Oreos");
    }

    for (int k = 0; k < numberOfToppings; k++)
    {
        while (true)
        {
            try
            {
                Console.Write($"\nEnter topping {k + 1}: ");
                string toppingName = Console.ReadLine();

                // Capitalize the first letter of the flavour name
                string top = char.ToUpper(toppingName[0]) + toppingName.Substring(1);


                string[] validToppings = { "Sprinkles", "Mochi", "Sago", "Oreos" };
                bool yesno = false;

                foreach (string t in validToppings)
                {
                    if (string.Equals(top, t, StringComparison.OrdinalIgnoreCase))
                    {
                        yesno = true;
                        break;
                    }
                }

                if (yesno)
                {
                    Console.WriteLine($"Selected flavour: {top}");
                    Topping tp = new Topping();

                    if (top == "Sprinkles" || top == "Mochi" || top == "Sago" || top == "Oreos")
                    {
                        tp.type = top;
                        toppings.Add(new Topping(top));
                    }

                    break;
                }
                else
                {
                    Console.WriteLine("Error: Invalid input. Please enter Sprinkles, Mochi, Sago or Oreos.");
                }

            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid topping name.");
            }
        }
    }

    bool isDipped = false;
    if (option == "2")
    {
        do
        {
            Console.Write("\nDo you want the cone dipped? (true/false): ");
            string userInput = Console.ReadLine();

            // Validate user input
            if (userInput == "true" || userInput == "false")
            {
                isDipped = bool.Parse(userInput);
                // Display the validated input
                Console.WriteLine($"Cone dipped: {isDipped}");
                break; // Exit the loop if input is valid
            }
            else
            {
                // Display an error message for invalid input
                Console.WriteLine("Invalid input. Please enter 'true' or 'false'.");
            }

        } while (true);
    }

    string wf = "";

    if (option == "3")
    {
        if (option == "3")
        {
            string[] allowedFlavors = { "Original", "Red Velvet", "Charcoal", "Pandan" };

            do
            {
                Console.WriteLine("\nWaffle Flavour\n--------------\nOriginal\nRed Velvet (+$3)\nCharcoal (+$3)\nPandan (+$3)");
                Console.Write("\nWaffle flavour: ");

                wf = Console.ReadLine();

                string[] wc = wf.Split(' ');

                for (int w = 0; w < wc.Length; w++)
                {
                    if (!string.IsNullOrEmpty(wc[w]))
                    {
                        wc[w] = char.ToUpper(wc[w][0]) + wc[w].Substring(1);
                    }
                }
                wf = string.Join(' ', wc);


                if (allowedFlavors.Contains(wf))
                {
                    Console.WriteLine($"Waffle flavour chosen: {wf}");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid waffle flavor. Flavours allowed: Original, Red Velvet, Charcoal &");
                }

            } while (true);
        }
    }


    if (option == "1")
    {
        Cup new_cup = new Cup("Cup", scoops, flavour, toppings);
        iceCreamList.Add(new_cup);
        return new_cup;
    }
    else if (option == "2")
    {
        Cone new_cone = new Cone("Cone", scoops, flavour, toppings, isDipped);
        iceCreamList.Add(new_cone);
        return new_cone;
    }
    else if (option == "3")
    {
        Waffle new_waffle = new Waffle("Waffle", scoops, flavour, toppings, wf);
        iceCreamList.Add(new_waffle);
        return new_waffle;
    }
    else
    {
        Console.WriteLine("Error. Please key in options from 1 to 3.");
        return null;
    }
}


// method to display order details from history and current orders
void PrintOrderDetails(Queue<Order> orderQueue, string queueName, int selectedMemberId)
{
    if (orderQueue.Count == null) //if orderQueeu is 0, indicating that orderqueue is empty
    {
        Console.WriteLine($"{queueName}");
        Console.WriteLine("Order information:");
        Console.WriteLine("{0,-7} {1,-20}", "ID", "Time Received");
    }

    bool foundMember = false;

    string prem = "";
    foreach (Order order in orderQueue)
    {
        if (order.Id == selectedMemberId)
        {
            foundMember = true;
            Console.WriteLine("{0,-7}{1,-20}", order.Id, order.timeReceived); // Print order details

            foreach (IceCream iceCream in order.iceCreamList) //printing order details
            {
                Console.WriteLine("\nIce Cream information:");
                Console.WriteLine($"Option: {iceCream.option}");
                Console.WriteLine($"Scoops: {iceCream.scoops}");
                Console.WriteLine($"Flavours: ");

                for (int i = 0; i < iceCream.flavours.Count; i++)
                {
                    Flavour flavour = iceCream.flavours[i];
                    if (flavour.premium == true)
                    {
                        prem = "(Premium)";
                    }
                    Console.WriteLine($" - {flavour.type} {prem}");
                }
                if (iceCream.toppings.Count != 0)
                {
                    Console.WriteLine($"Toppings: ");
                }
                for (int i = 0; i < iceCream.toppings.Count; i++)
                {
                    Topping topping = iceCream.toppings[i];
                    Console.WriteLine($" - {topping.type}");
                }

            }
        }
    }

    if (!foundMember) // data validation
    {
        Console.WriteLine($"\nNo orders found for Member ID in {queueName}: {selectedMemberId}");
    }
}

// method to display ice cream details
void DisplayIceCreamDetails(Order order, IceCream iceCream) //print ice cream details
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

// option 1
void Option1()
{
    //reading from customers.csv file
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine();
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


// option 2
void Option2()
{
    Console.WriteLine("\nGOLD MEMBER QUEUE");
    if (goldOrderQueue.Count != 0)
    {
        Console.WriteLine("Order information:");
        Console.WriteLine("{0,-7}{1,-20}", "ID", "Time Received");

        foreach (Order order in goldOrderQueue)
        {
            Console.WriteLine("{0,-7}{1,-20}", order.Id, order.timeReceived);


            foreach (IceCream iceCream in order.iceCreamList)
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
    }
    else
    {
        Console.WriteLine("Nothing in Gold Queue.");
    }

    Console.WriteLine("\nREGULAR MEMBER QUEUE");
    if (regularOrderQueue.Count != 0)
    {
        Console.WriteLine("Order Information:");
        Console.WriteLine("{0,-7}{1,-20}", "ID", "Time Received");

        foreach (Order order in regularOrderQueue)
        {
            Console.WriteLine("{0,-7}{1,-20}", order.Id, order.timeReceived); // Print order details

            foreach (IceCream iceCream in order.iceCreamList)
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
    }
    else
    {
        Console.WriteLine("Nothing in Regular Queue.");
    }
}



// option 3
void Option3()
{
    string name;
    while (true)
    {
        Console.Write("Enter customer name: "); //prompt user for details   

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

    name = string.IsNullOrWhiteSpace(name) ? name : char.ToUpper(name[0]) + name[1..]; //ensures that string name is not null, empty, or contains only whitespace
                                                                                       //then returns the string with the first letter capitalized

    int id = 0;
    bool idCheck = true;

    while (idCheck)
    {
        try
        {
            Console.Write("Enter a 6-digit integer ID: ");
            id = Convert.ToInt32(Console.ReadLine());

            if (id >= 100000 && id <= 999999) //ensure id's first digit is not starting with 0 and that the id contains 6 digits
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

    string dobString;
    DateTime dob;

    while (true)
    {
        Console.Write("Enter customer date of birth in DD/MM/YYYY format: ");
        dobString = Console.ReadLine();


        if (DateTime.TryParseExact(dobString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dob)) //ensuring dob has the right format
                                                                                                                      //parsse the string dobString into the specified date format "dd/MM/yyyy"
                                                                                                                      //store the result in the dob variable
        {
            if (dob < DateTime.Now)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid date. Please enter a valid date.");
            }
        }
        else
        {
            Console.WriteLine("Invalid date format. Please enter the date in the correct format (DD/MM/YYYY).");
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

    customerList.Add(customer);
    using (StreamWriter sw = new StreamWriter("customers.csv", true))
    {
        string csvLine = string.Join(",", newList);
        sw.WriteLine(csvLine);

        Console.WriteLine($"\nName: {name}\nId: {id}\nDate of birth: {dob}");

        Console.WriteLine("Registration status: SUCCESSFUL");
    }

}


// option 4
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
        Customer customer = customerList.FirstOrDefault(customer => customer.memberId == cus_id); //assign memberid to cus_id


        if (customer != null)
        {
            string customerName = customer.name;
            DateTime customerDob = customer.dob;
            PointCard customerRewards = customer.rewards;

            Console.WriteLine($"Customer Name: {customerName}");
            Console.WriteLine($"Customer Date of Birth: {customerDob}");

            if (customerRewards != null)
            {
                string tierLevel = customerRewards.tier;
                Console.WriteLine($"Customer Tier Level: {tierLevel}");

                // new order object
                Order newOrder = new Order(cus_id, DateTime.Now);

                while (true)
                {
                    Console.WriteLine("---- Enter your ice cream order details ----");

                    IceCream iceCream = MakeIceCreamOrder();
                    newOrder.AddIceCream(iceCream);

                    bool input = false;
                    string xyz;

                    do
                    {
                        Console.Write("\nDo you want to add another ice cream to the order? ('y' / 'n'): ");
                        xyz = Console.ReadLine().ToLower();

                        if (xyz == "y" || xyz == "n")
                        {
                            input = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
                        }
                    } while (!input);

                    if (xyz == "y")
                    {
                        Console.WriteLine("Adding another ice cream.");
                    }
                    else
                    {
                        Console.WriteLine("Order is complete.");
                        break;
                    }
                }

                customer.currentOrder = newOrder;

                if (tierLevel == "Gold")
                {
                    newOrder.Id = customer.memberId;
                    goldOrderQueue.Enqueue(newOrder);
                    Console.WriteLine("Added to Gold Order Queue.");
                }
                else
                {
                    newOrder.Id = customer.memberId;
                    Console.WriteLine("Added to Regular Order Queue.");
                    regularOrderQueue.Enqueue(newOrder);
                }

                Console.WriteLine("Order has been made successfully!");
            }
        }
        else
        {
            Console.WriteLine("Invalid customer ID. Customer not found. Please try again.");
        }
    }
    else
    {
        Console.WriteLine("Error... Invalid input. Please enter a valid integer.");
    }
}


// option 5
void Option5()
{
    Console.WriteLine("List of Customers:");
    foreach (var x in customerList)  // for loop to run through customer list and retrieve information in it
    {
        Console.WriteLine("{0, -10} {1, -10}", x.name, x.memberId); //customerList contains information of each customer's name and memberId.
    }

    List<IceCream> iceCreamList; // calling iceCreamList

    foreach (var kvp in ordersHistoryDictionary)
    {
        int memberId = kvp.Key;
        iceCreamList = kvp.Value; // initialising keys and values from ordersHistoryDictionary 

        int pastOrdersCount = iceCreamList.Count;

        int currentOrdersCount = 0;

        foreach (Order order in goldOrderQueue) //run thru goldOrderQueue which contains order info for gold members
        {
            if (order.Id == memberId) //matching Id in goldorderQueue to memberId 
            {
                currentOrdersCount++; // number of orders in the queue
            }
        }

        foreach (Order orders in regularOrderQueue) //same thing but for regularOrderQueeu
        {
            if (orders.Id == memberId)
            {
                currentOrdersCount++; //number of orders in the queue
            }
        }

    }



    Console.Write("\nEnter the Member ID to retrieve order details: ");
    int selectedMemberId;

    if (int.TryParse(Console.ReadLine(), out selectedMemberId))
    {

        var keys = ordersHistoryDictionary.Keys.ToList(); //all keys in the dictionary will be put into a list


        //Console.WriteLine($"\nID: {selectedMemberId} Time Received : {order.timeReceived} Time Fulfilled : {order.timeFulfilled}"); //printing all order information


        if (ordersHistoryDictionary.ContainsKey(selectedMemberId))
        {
            Console.WriteLine("Displaying orders from PAST HISTORY queues..."); //past orders from orders.csv
            iceCreamList = ordersHistoryDictionary[selectedMemberId]; //iceCreamList will contain the value associated
                                                                      //with the key selectedMemberId in the dict 

            Console.WriteLine($"Order details for Member ID: {selectedMemberId}");
            Console.WriteLine($"{iceCreamList.Count}");
            foreach (IceCream iceCream in iceCreamList)
            {
                if (iceCream is Cup cup) //printing out the icecream details
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
            Console.WriteLine("\n\nDisplaying orders from CURRENT queues..."); //orders pullewd from option 2
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


// option 6 
void Option6()
{
    Console.WriteLine("List of Customers: \n");
    foreach (var x in customerList)  //printing customer information
    {
        Console.WriteLine("{0, -10} {1, -10}", x.name, x.memberId);
    }

    Console.Write("\nEnter the Member ID to select a customer: ");  //tries to retrieve a valye associated with selectedmemberId from the dict 
    if (int.TryParse(Console.ReadLine(), out int selectedMemberId))
    {
        Customer selectedCustomer = customerList.Find(customer => customer.memberId == selectedMemberId);

        Order selectedOrder = new Order(); 
        selectedOrder.Id = selectedMemberId; 
        selectedOrder.iceCreamList = iceCreamList;

        Console.WriteLine("Order Information:");

        int count = 0;
        foreach (IceCream iceCream in selectedOrder.iceCreamList) //print ice cream details
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
            count++;
        }

        int option;
        while (true)
        {
            Console.WriteLine("\nChoose an action: \n [1] Modify\n [2] Add new\n [3] Delete existing");
            Console.Write("Enter your choice: ");
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
                Console.WriteLine($"Total index: {count}");
                Console.Write("Enter the index of the ice cream to modify: ");

                if (int.TryParse(Console.ReadLine(), out int modifyIndex) && modifyIndex >= 1 && modifyIndex <= selectedOrder.iceCreamList.Count)
                {
                    selectedOrder.ModifyIceCream(modifyIndex - 1);

                    updateOrder(regularOrderQueue);
                    updateOrder(goldOrderQueue);

                    void updateOrder(Queue<Order> orderQueue)
                    {
                        foreach (Order order in orderQueue)
                        {
                            if (order.Id == selectedMemberId) 
                            {
                                order.iceCreamList = iceCreamList;
                                break;
                            }
                        }
                    }
                    Console.WriteLine("Order modified successfully...");                   
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
                break;
            case 2:
                IceCream newIceCream = MakeIceCreamOrder();

                addNewOrder(regularOrderQueue, selectedOrder);
                addNewOrder(goldOrderQueue, selectedOrder);                

                void addNewOrder(Queue<Order> orderQueue, Order newOrder)
                {
                    Order existingOrder = orderQueue.FirstOrDefault(order => order.Id == selectedMemberId);

                    if (existingOrder != null)
                    {
                        existingOrder.iceCreamList = newOrder.iceCreamList;
                    }
                    else
                    {
                        orderQueue.Enqueue(newOrder);
                    }
                }
                break;
            case 3:
                Console.Write("Enter the index of the ice cream you want to delete: ");
                if (int.TryParse(Console.ReadLine(), out int deleteIndex) && deleteIndex >= 1 && deleteIndex <= selectedOrder.iceCreamList.Count)
                {
                    selectedOrder.DeleteIceCream(deleteIndex - 1);

                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
                break;

            default:
                Console.WriteLine("Invalid action. Please try again.");
                break;
        }

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

//---ADVANCED FEATURES---

// option 7
void Option7()
{

    Order currentOrder = null; //initially currentorder is null
    // Check if there are orders in the queue
    if (goldOrderQueue.Count > 0)
    {
        currentOrder = goldOrderQueue.Dequeue();
    }
    else if (regularOrderQueue.Count > 0)
    {
        currentOrder = regularOrderQueue.Dequeue();
    }
    // Display all ice creams in the order
    Console.WriteLine("Ice Creams in the Order:");
    int icecreamcount = 0;
    if (currentOrder != null)
    {
        foreach (IceCream iceCream in currentOrder.iceCreamList) //displays ice cream order
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




        // Display customer's membership status and points
        Customer currentCustomer = customerList.FirstOrDefault(c => c.memberId == currentOrder.Id);
        // Display total bill amount


        if (currentCustomer != null)
        {
            Console.WriteLine($"Membership Status: {currentCustomer.rewards.tier}");
            Console.WriteLine($"Points: {currentCustomer.rewards.points}"); //displays customer's tier status and nummber of points
            double totalBill = currentOrder.CalculateTotal();
            Console.WriteLine($"Total Bill Amount: ${totalBill:F2}"); // total bill of order
                                                                      // Check if punch card is completed
            if (currentCustomer.rewards.punchCard == 10)
            {


                totalBill -= currentOrder.iceCreamList[0].CalculatePrice();
                Console.WriteLine("Congratulations! As a prize for finishing the punch card, you get your 11th ice cream free!");
                Console.WriteLine($"Discounted Total Bill Amount: ${totalBill:F2}");
            }
            else if (currentCustomer.IsBirthday())
            {
                //.orderbydescending sorts ice creams in descending order based on their price. takes the first element from the sorted list
                IceCream mostexic = currentOrder.iceCreamList.OrderByDescending(ic => ic.CalculatePrice()).FirstOrDefault();
                totalBill -= mostexic.CalculatePrice();
                Console.WriteLine("Happy birthday! As a reward, you get your most expensive ice cream free!");
                Console.WriteLine($"Discounted Total Bill Amount: ${totalBill:F2}");
            }
            else if (currentCustomer.rewards.punchCard > 11)
            {
                // Set the cost of the first ice cream in the order to $0.00
                currentCustomer.rewards.Punch();
            }


            // Check Pointcard status for redeeming points
            if (currentCustomer.rewards.tier == "Ordinary")
            {
                // Customer begins as an ordinary member
                currentCustomer.rewards.AddPoints(Convert.ToInt32(totalBill));

                if (currentCustomer.rewards.points >= 100)
                {
                    currentCustomer.rewards.tier = "Gold";
                }
                else if (currentCustomer.rewards.points >= 50)
                {
                    currentCustomer.rewards.tier = "Silver";
                }
            }
            else if (currentCustomer.rewards.tier == "Silver")
            {

                if (currentCustomer.rewards.points >= 100)
                {
                    currentCustomer.rewards.tier = "Gold";
                }
            }
            else if (currentCustomer.rewards.tier == "Silver" || currentCustomer.rewards.tier == "Gold")
            {
                // Existing silver or gold member
                currentCustomer.rewards.RedeemPoints(totalBill);
            }


            Console.Write("Press any key to make payment...");
            Console.ReadKey();

            // Example: Display a payment confirmation message
            Console.WriteLine("\nPayment successful! Thank you for your purchase.");

            // Increment punch card for every ice cream in the order
            foreach (IceCream iceCream in currentOrder.iceCreamList)
            {
                currentCustomer.rewards.Punch();

                // Earn points and upgrade membership status accordingly
                currentCustomer.rewards.AddPoints(Convert.ToInt32(totalBill));
            }

            // Mark the order as fulfilled with the current datetime
            currentOrder.timeFulfilled = DateTime.Now;

            // Add the fulfilled order object to the customer's order history
            currentCustomer.orderHistory.Add(currentOrder);

            Console.WriteLine("Order fulfilled and processed successfully.");
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
            foreach (IceCream icecream in currentOrder.iceCreamList) // "" an empty string, if customer requests for toppings/flavours, name of those toppings will be updated from "" to that. 
            {
                if (icecream is Cup cup)
                {
                    var f1 = "";
                    var f2 = "";
                    var f3 = "";

                    int count1 = 0;
                    foreach (Flavour flavour in cup.flavours)
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
                    var t4 = "";

                    int count2 = 0;
                    foreach (Topping topping in cup.toppings)
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
                        else if (count2 == 4)
                        {
                            t4 = topping.type;
                        }

                    }

                    sw.WriteLine($"{orderID},{currentCustomer.memberId},{currentOrder.timeReceived},{currentOrder.timeFulfilled},{"Cup"},{cup.scoops},{""},{""},{f1},{f2},{f3},{t1},{t2},{t3},{t4}");

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
                    var t4 = "";

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
                        else if (count2 == 4)
                        {
                            t4 = topping.type;
                        }

                    }

                    sw.WriteLine($"{orderID},{currentCustomer.memberId},{currentOrder.timeReceived},{currentOrder.timeFulfilled},{"Cone"},{cone.scoops},{cone.dipped},{""},{f1},{f2},{f3},{t1},{t2},{t3},{t4}");
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
                    var t4 = "";

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
                        else if (count2 == 4)
                        {
                            t4 = topping.type;
                        }

                    }

                    sw.WriteLine($"{orderID},{currentCustomer.memberId},{currentOrder.timeReceived},{currentOrder.timeFulfilled},{"Waffle"},{waffle.scoops},{""},{waffle.waffleFlavour},{f1},{f2},{f3},{t1},{t2},{t3},{t4}");
                }
            }
        }

        string[] lines = File.ReadAllLines("customers.csv");  //updating membership status, points and punchcard points for customers.csv. 
        for (int i = 1; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split(',');
            if (columns.Length >= 2)
            {
                int memberId = Convert.ToInt32(columns[1]);
                if (memberId == currentCustomer.memberId)
                {
                    columns[3] = currentCustomer.rewards.tier;
                    columns[4] = Convert.ToString(currentCustomer.rewards.points);
                    columns[5] = Convert.ToString(currentCustomer.rewards.punchCard);
                    lines[i] = string.Join(',', columns);
                    File.WriteAllLines("customers.csv", lines);
                }
            }

        }
    }
    else
    {
        Console.WriteLine("There are no orders in the queue as of now.");
    }

}

// option 8
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

    Console.WriteLine();

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

    double total_month = 0;

    foreach (var m in totalCharge)
    {
        string month = m.Key;
        List<double> charges = m.Value;

        double count = 0;
        foreach (var charge in charges)
        {
            count += charge;

        }

        total_month += count;
        Console.WriteLine($"{month} {input_year}: ${count.ToString("0.00")}");
    }

    Console.WriteLine($"\nTotal: ${string.Format("{0:0.00}", total_month)}");
}


// display menu
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