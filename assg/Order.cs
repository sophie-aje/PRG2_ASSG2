using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    internal class Order
    {
        public int Id { get; set; }

        public DateTime timeRecieved { get; set; }
        public DateTime? timeFuffilled { get; set; }
        public List<IceCream> iceCreamList { get; set; }
        public Order() { }
        public Order(int i, DateTime tr)
        {
            Id = i;
            timeRecieved = tr;
        }
        public ModifyIceCream()
        {

        }
        public AddedIceCream(IceCream addic)
        {
            iceCreamList.Add(addic);
            
        }
        public DeleteIceCream(int )
        {

        }
        public double CalculateTotal()
        {

        }
        public override string ToString()
        {
            return $"{Id}\t{timeRecieved}\t{timeFuffilled}";
        }
    }
}
