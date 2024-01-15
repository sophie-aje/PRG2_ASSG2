//basic features
//nehaa: option 1,3,4
//sophie: option 2,5,6

using assg;

void DisplayMenu()
{
    while (true)
    {
        try
        {


            Console.WriteLine("\nThe I.C. Treats Management System (enter 0 to break)");
            Console.WriteLine("=================================");
            Console.WriteLine("[1] List all customers");
            Console.WriteLine("[2] List all current orders");
            Console.WriteLine("[3] Regsiter a new customer");
            Console.WriteLine("[4] Create a customers' order");
            Console.WriteLine("[5] Display order details of a customer");
            Console.WriteLine("[6] Modify order details");
            Console.Write("\nEnter an option: ");
            int option = Convert.ToInt32(Console.ReadLine());
            if (option == 1)
            {
                Option1();

            }
            else if (option == 2)
            {
                
            }
            else if (option == 3)
            {
                Option3();                
            }
            else if (option == 4)
            {

            }
            else if (option == 5)
            {

            }
            else if (option == 6)
            {

            }
            else if (option == 0)
            {
                break;
            }
            else
            {
                Console.WriteLine("Please enter a valid option.");
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Please enter a valid number for the option.");
        }



    }
}

DisplayMenu();

//---BASIC FEATURES---

//Option 1: 
void Option1()
{
    //reading from customers.csv file
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine(); // read the heading
                                   // display the heading
        if (s != null)
        {
            string[] heading = s.Split(',');
            Console.WriteLine("{0,-10}  {1,-10}  {2,-10}  {3,-10}",
                heading[0], heading[1], heading[2], heading[3], heading[4], heading[5]);
            // repeat until end of file
        }
        while ((s = sr.ReadLine()) != null)
        {
            string[] info = s.Split(',');
            Console.WriteLine("{0,-10}  {1,-10}  {2,-10}  {3,-10}",
                info[0], info[1], info[2], info[3], info[4], info[5]);
        }
    }
}

//Option 2: 


//Option 3: 
void Option3()
{
    //prompt user for details
    Console.Write("Enter customer name: ");
    string name = Console.ReadLine();

    Console.Write("Enter customer ID number: ");
    string id = Console.ReadLine();

    Console.Write("Enter customer data of birth: ");
    DateTime dob = Console.ReadLine();

    //create customer object
    Customer customer = new Customer(name, id, dob);

    //create pointcard object
    PointCard pointCard = new PointCard(0,0);

    //assign the PointCard to Customer
    customer1.rewards = pointCard1;

    //append customer info into customers csv file
    string memstatus = "Silver";
    List<string> newlist = new List<string> { name, id, dob, memstatus, 0, 0};
    using (StreamWriter sw = new StreamWriter("WriteLines2.txt", false))
    {
        foreach (string s in newlist)
        {
            sw.WriteLine(s);
        }

        Console.WriteLine("Registration status: SUCCESSFUL");
    }





}

//Option 4: 
void Option4()
{
    //list customers from customer.csv
    List<Customer> customerList = new List<Customer>();
    using (StreamReader sr = new StreamReader("customers.csv"))
    {
        string? s = sr.ReadLine(); // read the heading
                                   // display the heading
        while ((s = sr.ReadLine()) != null)
        {
            string[] info = s.Split(',');
            Console.WriteLine("{0,-10}  {1,-10}  {2,-10}  {3,-10}",
                info[0], info[1], info[2], info[3], info[4], info[5]);
            string c_name = info[0];
            string c_id = info[1];
            DateTime c_dob = info[2];
            Customer customer = new Customer(c_name, c_id, c_dob-dob);
            customerList.Add(customer);
        }
    }

    //prompt user to select customer and retrieve selected customer
    Console.Write("Select a customer (enter Customer ID) : ");
    strig cus_id = Console.ReadLine();

    foreach (var x in Customer)
    {
        if (x.id == cus_id)
        {

        }

    }
}

//Option 5: 


//Option 6: 