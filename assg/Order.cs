using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                // Modify the ice cream object's info accordingly
                Console.Write("Enter new option: (Cup/Cone/Waffle) ");
                string newoption = Console.ReadLine();

                // Create a new ice cream instance based on the chosen option
                IceCream newIceCream;
                switch (newoption)
                {
                    case "Cup":
                        newIceCream = new Cup();
                        break;
                    case "Cone":
                        newIceCream = new Cone();
                        break;
                    case "Waffle":
                        newIceCream = new Waffle();
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        return;
                }

                // Modify common properties
                Console.Write("Enter new scoops: ");
                if (int.TryParse(Console.ReadLine(), out int newScoops))
                {
                    newIceCream.scoops = newScoops;
                }

                // Modify new flavours accordingly
                List<Flavour> newFlavours = new List<Flavour>();
                for (int i = 1; i <= newScoops; i++)
                {
                    Console.WriteLine($"Enter flavour {i}: ");
                    string flavourr = Console.ReadLine();

                    bool isPremium = (flavourr == "durian" || flavourr == "ube" || flavourr == "sea salt");
                    newFlavours.Add(new Flavour { type = flavourr, premium = isPremium });
                }
                newIceCream.flavours = newFlavours;

                // Modify new toppings accordingly
                Console.WriteLine("Enter new number of toppings: ");
                int notop = Convert.ToInt32(Console.ReadLine());

                List<Topping> toppingsList = new List<Topping>();
                for (int i = 1; i <= notop; i++)
                {
                    Console.WriteLine($"Enter topping {i}: ");
                    Topping newTopping = new Topping(Console.ReadLine());
                    toppingsList.Add(newTopping);
                }
                newIceCream.toppings = toppingsList;

                // Modify specific properties based on the ice cream type
                if (newoption == "Cone" || newoption == "2")
                {
                    if (newIceCream is Cone cone)
                    {
                        Console.WriteLine("Do you want your cone dipped? (true/false): ");
                        if (bool.TryParse(Console.ReadLine(), out bool dipped))
                        {
                            cone.dipped = dipped;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                            return;
                        }
                    }
                }
                else if (newoption == "Waffle" || newoption == "3")
                {
                    if (newIceCream is Waffle waffle)
                    {
                        Console.WriteLine("Enter new waffle flavour: ");
                        waffle.waffleFlavour = Console.ReadLine();
                    }
                }

                // Update the original ice cream with the modified one
                iceCreamList[iceCreamindex] = newIceCream;

                Console.WriteLine("Ice Cream modified successfully.");
            }
            else
            {
                Console.WriteLine("Invalid index. Ice Cream not found.");
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

