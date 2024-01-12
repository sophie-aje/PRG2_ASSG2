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

        public DateTime timeReceived { get; set; }
        public DateTime? timeFulfilled { get; set; }
        public List<IceCream> iceCreamList { get; set; }
        = new List<IceCream>();

        public Order() { }
        public Order(int i, DateTime tr)
        {
            Id = i;
            timeReceived = tr;
        }
        public void ModifyIceCream(int m)
        {
            iceCreamList.RemoveAt(m);
        }
        public void AddIceCream(IceCream addic)
        {
            iceCreamList.Add(addic);

        }
        public void DeleteIceCream(int d)
        {
            iceCreamList.RemoveAt(d);
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

