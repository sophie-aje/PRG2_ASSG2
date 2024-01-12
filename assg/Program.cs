void DisplayMenu()
{
    while (true)
    {
        try
        {


            Console.WriteLine("The I.C. Treats Management System (enter 0 to break)");
            Console.WriteLine("=================================");
            Console.WriteLine("[1] List all customers");
            Console.WriteLine("[2] List all current orders");
            Console.WriteLine("[3] Regsiter a new customer");
            Console.WriteLine("[4] Create a customers' order");
            Console.WriteLine("[5] Display order details of a customer");
            Console.WriteLine("[6] Modify order details");
            Console.Write("Enter an option: ");
            int option = Convert.ToInt32(Console.ReadLine());
            if (option == 1)
            {

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
