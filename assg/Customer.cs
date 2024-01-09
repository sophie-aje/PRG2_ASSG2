using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    class Customer
    {
        public string name {  get; set; }
        public int memberid { get; set; }
        public DateTime dob { get; set; }
        public Order currentOrder { get; set; }

        public List<Order> orderHistory { get; set; }

        public PointCard rewards { get; set; }

        public Customer() { }
        public Customer (string n, int mid, DateTime d)
        {
            name = n;
            memberid = mid;
            dob = d;
        }
        public Order MakeOrder()
        {

        }
        public bool IsBirthday()
        {
            if (dob = DateTime.Now)
            { 
                return true;
            } 
            else
            { 
                return false;
            }
        }
        public override string ToString()
        {
            return $"{name}\t{memberId}\t{dob}";
        }
    }
}
