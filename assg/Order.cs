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
        public DateTime? timeFulfilled { get; set; } = DateTime.Now.AddMinutes(15);
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
                if (newoption == "Cone" || newoption == "Waffle" || newoption == "Cup")
                {
                    iceCream.option = newoption;
                }



                // modify new number of scoops accordingly
                Console.Write("Enter new scoops: ");
                if (int.TryParse(Console.ReadLine(), out int newScoops))
                {
                    iceCream.scoops = newScoops;
                }

                //modify new flavours accordingly
                List<Flavour> newFlavours = new List<Flavour>();
                for (int i = 1; i <= newScoops; i++)
                {
                    Console.WriteLine($"Enter flavour {i}: ");
                    string flavourr = Console.ReadLine();

                    if (flavourr == "durian" || flavourr == "ube" || flavourr == "sea salt")
                    {
                        bool isPremium = true;
                        newFlavours.Add(new Flavour { type = flavourr, premium = isPremium });
                        iceCream.flavours = newFlavours;
                    }
                    else
                    {
                        bool isPremium = false;
                        newFlavours.Add(new Flavour { type = flavourr, premium = isPremium });
                        iceCream.flavours = newFlavours;
                    }
                }

                //modify new toppings accordingly
                Console.WriteLine("Enter new number of toppings: ");
                int notop = Convert.ToInt32(Console.ReadLine());

                List<Topping> toppingsList = new List<Topping>();

                for (int i = 1; i <= notop; i++)
                {
                    Console.WriteLine($"Enter topping {i}: ");
                    Topping newTopping = new Topping(Console.ReadLine());
                    iceCream.toppings.Clear();
                    toppingsList.Add(newTopping);
                }

                iceCream.toppings.AddRange(toppingsList);

                //modify dipped cone or not if applicable

                if (newoption == "Cone" || newoption == "2")
                {
                    Cone cone = new Cone();
                    iceCream = cone;
                    Console.WriteLine("Do you want your cone dipped? (true/false): ");
                    if (bool.TryParse(Console.ReadLine(), out bool dipped))
                    {
                        cone.dipped = dipped;

                    }
                    else
                    {
                        Console.WriteLine("");
                    }

                }

                //modify waffle flavour if applicable
                if (iceCream.option == "3" || iceCream.option == "Waffle")
                {

                    Waffle waffle = new Waffle();
                    Console.WriteLine("Enter new waffle flavour: ");
                    waffle.waffleFlavour = Console.ReadLine();


                    Console.WriteLine("Ice Cream modified successfully.");
                }
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

