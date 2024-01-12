using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    class Cup : IceCream
    {
        public Cup() { }
        public Cup(string o, int s, List<Flavour> f, List<Topping> t)
        : base(o, s, f, t)
        {
            option = o;
            scoops = s;
            flavours = f;
            toppings = t;
        }

        public override double CalculatePrice()
        {
            double base_price = 0;
            if (scoops == 1)
            {
                base_price = 4;
            }
            else if (scoops == 2)
            {
                base_price = 5.5;
            }
            else if (scoops == 2)
            {
                base_price = 6.5;
            }
            double prem = 0;
            foreach (var flavour in flavours)
            {
                if (flavour.premium)
                {
                    prem += 2;
                }
            }

            double total_price = base_price + (toppings.Count * 1) + prem;
            return total_price;
        }

        public override string ToString()
        {
            return $"{option}\t{scoops}\t{flavours}\t{toppings}";
        }
    }
}
