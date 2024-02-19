using System;

namespace QueenLocalDataHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderCRUD orderCRUD = new OrderCRUD();

            while (true)
            {
                Console.WriteLine("-------- QueenShop Order Management --------");
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("\n1. Insert Order");
                Console.WriteLine("2. Get All Orders");
                Console.WriteLine("3. Update Customer Address");
                Console.WriteLine("4. Update Order Address");
                Console.WriteLine("5. Delete Order");
                Console.WriteLine("6. Exit\n");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        // Insert Order
                        Console.WriteLine("Enter order details:");
                        Console.Write("Order ID: ");
                        int orderID = int.Parse(Console.ReadLine());
                        Console.Write("CNIC: ");
                        string cnic = Console.ReadLine();
                        Console.Write("Customer Name: ");
                        string name = Console.ReadLine();
                        Console.Write("Customer Phone: ");
                        string phone = Console.ReadLine();
                        Console.Write("Customer Address: ");
                        string address = Console.ReadLine();
                        Console.Write("Product ID: ");
                        int productID = int.Parse(Console.ReadLine());
                        Console.Write("Price: ");
                        decimal price = decimal.Parse(Console.ReadLine());
                        Console.Write("Size: ");
                        string size = Console.ReadLine();
                        Order order = new Order(orderID, cnic, name, phone, address, productID, price, size);
                        orderCRUD.InsertOrder(order);
                        break;

                    case "2":
                        // All Orders list
                        Console.WriteLine("List of All Orders:\n");
                        orderCRUD.GetAllOrders();
                        break;

                    case "3":
                        // Updating Customer Address
                        Console.WriteLine("Enter customer phone number: ");
                        string customerPhone = Console.ReadLine();
                        Console.WriteLine("Enter new address: ");
                        string newAddress = Console.ReadLine();
                        orderCRUD.UpdateAddress(customerPhone, newAddress);
                        break;

                    case "4":
                        // Updating Order Address (using parameterized query to avoid SQL Injection)
                        Console.WriteLine("Enter customer phone number: ");
                        string orderPhone = Console.ReadLine();
                        Console.WriteLine("Enter new address: ");
                        string orderNewAddress = Console.ReadLine();
                        orderCRUD.UpdateOrderAddress(orderPhone, orderNewAddress);
                        break;

                    case "5":
                        // Deleting Order
                        Console.WriteLine("Enter Order ID to delete: ");
                        int orderIdToDelete = int.Parse(Console.ReadLine());
                        orderCRUD.DeleteOrder(orderIdToDelete);
                        Console.WriteLine("Order deleted.");
                        break;

                    case "6":
                        // Exiting Menu
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }

        }
    }

}


