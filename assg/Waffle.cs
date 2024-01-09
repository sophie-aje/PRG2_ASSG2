﻿using System;
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
        public override double CalculatePrice(double price)
        {
            price = 7.0;
            if (waffleFlavour == "Pandan" || waffleFlavour == "Red velvet" || waffleFlavour == "Charcoal")
            {
                price += 3.0;
            }

            if (scoops == 2)
            {
                price += 3.0;
            }
            else if (scoops == 3)
            {
                price += 3.0;
            }
            return price;
        }
        
        public override string ToString()
        {
            return $"{option}\t{scoops}\t{flavours}\t{toppings}\t{waffleFlavour}";
        }
    }


}
