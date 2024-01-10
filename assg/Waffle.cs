using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    class Waffle : IceCream
    {
        public string waffleFlavour { get; set; }

        public Waffle() { }

        public Waffle(string o, int s, List<Flavour> f, List<Topping> t, string wf)
        {
            option = o;
            scoops = s;
            flavours = f;
            toppings = t;
            waffleFlavour = wf;
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

            double wf_price = 0;
            if (waffleFlavour == "Red Velvet" || waffleFlavour == "Charcoal" || waffleFlavour == "Pandan")
            {
                wf_price = 3;
            }
            double total_price = base_price + (toppings.Count * 1) + wf_price;
            return total_price;
        }

        public override string ToString()
        {
            return $"{option}\t{scoops}\t{flavours}\t{toppings}\t{waffleFlavour}";
        }
    }


}
