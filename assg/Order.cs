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

        public DateTime timeRecieved { get; set; }

        public DateTime? timeFulffilled { get; set; }
        public List<IceCream> iceCreamList { get; set; } = new List<IceCream>();


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
<<<<<<< HEAD
            return $"{Id}\t{timeRecieved}\t{timeFulffilled}";
=======
            return $"{Id}\t{timeRecieved}\t{timeFulfilled}";
>>>>>>> 766ec3c4bd1dd2936aa1653bcc53ee7555f99c5d
        }
    }
}
