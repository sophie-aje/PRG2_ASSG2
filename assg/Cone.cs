using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    class Cone : IceCream
    {
        public bool dipped { get; set; }
        public Cone() { }

        public Cone(string o, int s, List<Flavour> f, List<Topping> t, bool d)
        {
            option = o;
            scoops = s;
            flavours = f;
            toppings = t;
            dipped = d;
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
            double dip = 0;
            if (dipped)
            {
                dip = 2;
            }

            double prem = 0;
            foreach (var flavour in flavours)
            {
                if (flavour.premium)
                {
                    prem += 2;
                }
            }

            double total_price = base_price + (toppings.Count * 1) + dip + prem;

            return total_price;                       
        }
        

        public override string ToString()
        {
            return $"{option}\t{scoops}\t{flavours}\t{toppings}\t{CalculatePrice()}";
        }
    }
}
