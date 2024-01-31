using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace assg
{

    class Order
    {
        public int Id { get; set; }

        public DateTime timeReceived { get; set; } = DateTime.Now;
        public DateTime? timeFulfilled { get; set; }
        public List<IceCream> iceCreamList { get; set; }
        = new List<IceCream>();

        public Order() { }
        public Order(int i, DateTime tr)
        {

            Id = i;

            timeReceived = tr;

        }
        public void ModifyIceCream(int iceCreamindex)
        {
            if (iceCreamindex >= 0 && iceCreamindex < iceCreamList.Count)
            {
                IceCream iceCream = iceCreamList[iceCreamindex];

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

                if (option == "1")
                {
                    Cup new_cup = new Cup("Cup", scoops, flavour, toppings);
                    iceCreamList[iceCreamindex] = new_cup; // Update the ice cream in the list
                }
                else if (option == "2")
                {
                    Cone new_cone = new Cone("Cone", scoops, flavour, toppings, isDipped);
                    iceCreamList[iceCreamindex] = new_cone; // Update the ice cream in the list
                }
                else if (option == "3")
                {
                    Waffle new_waffle = new Waffle("Waffle", scoops, flavour, toppings, wf);
                    iceCreamList[iceCreamindex] = new_waffle; // Update the ice cream in the list
                }
                else
                {
                    Console.WriteLine("Error. Please key in options from 1 to 3.");
                    // Add handling for invalid option if needed
                }
                Console.WriteLine("Ice Cream modified successfully.");
            }
        }

        private void UpdateOrderInQueue(Queue<Order> orderQueue)
        {
            foreach (Order order in orderQueue)
            {
                if (order.Id == this.Id) // Assuming Id is used to identify orders
                {
                    order.iceCreamList = iceCreamList;
                    break;
                }
            }
        }

        public void AddIceCream(IceCream addic)
        {
            iceCreamList.Add(addic);
        }
        public void DeleteIceCream(int iceCreamIndex)
        {
            if (iceCreamIndex > 1)
            {
                iceCreamList.RemoveAt(iceCreamIndex);
                Console.WriteLine("Ice Cream deleted successfully.");
            }
            else
            {
                Console.WriteLine("Cannot have zero ice creams in an order.");
            }
        }
        public double CalculateTotal()
        {
            double total_ic_price = 0;
            foreach (IceCream ice in iceCreamList)
            {
                total_ic_price += ice.CalculatePrice();
            }
            return total_ic_price;
        }
        public override string ToString()
        {
            return $"{Id}\t{timeReceived}\t{timeFulfilled}";
        }
    }
}

