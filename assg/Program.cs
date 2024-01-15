//basic features
//nehaa: option 1,3,4
//sophie: option 2,5,6

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


//Option 4: 


//Option 5: 


//Option 6: 