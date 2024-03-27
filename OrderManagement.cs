using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace QueenShopNewOrderManagement
{
    class OrderManagement
    {
        static string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QueensNewDB;Integrated Security=True;";

        static void Main(string[] args)
        {
            // Initializing DataSet and DataAdapter
            DataSet dataSet = new DataSet();

            SqlConnection connection = new SqlConnection(constr);
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Orders", connection);

            try
            {
                connection.Open();
                dataAdapter.Fill(dataSet, "Orders");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }

          
            while (true)
            {
                Console.WriteLine("--------------------------Menu--------------------------");
                Console.WriteLine("1. Modify Order Details");
                Console.WriteLine("2. Add New Order");
                Console.WriteLine("3. Remove Order");
                Console.WriteLine("4. Confirm Orders and Exit");
                Console.WriteLine("--------------------------------------------------------");
                Console.Write("Enter option (1-4): ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ModifyOrderDetails(dataSet);
                        break;
                    case "2":
                        AddNewOrder(dataSet);
                        break;
                    case "3":
                        RemoveOrder(dataSet);
                        break;
                    case "4":
                        SynchronizeChangesWithDatabase(dataSet, dataAdapter);
                        return;
                    default:
                        Console.WriteLine("Invalid option!!! Please try again.");
                        break;


                }

            }
        }



        // Modifying Order Details Locally
        static void ModifyOrderDetails(DataSet dataSet)
        {
            try
            {
                DataTable ordersTable = dataSet.Tables["Orders"];

                Console.Write("Enter Order ID to modify: ");
                int orderId = int.Parse(Console.ReadLine());

                DataRow[] foundRows = ordersTable.Select($"OrderID = {orderId}");

                if (foundRows.Length == 0)
                {
                    Console.WriteLine($"Order with ID {orderId} not found.");
                    return;
                }

                DataRow orderRow = foundRows[0];

                Console.Write("Enter new quantity: ");
                int newQuantity = int.Parse(Console.ReadLine());
                orderRow["ProductQuantity"] = newQuantity;

                Console.WriteLine("\nOrder details modified successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError modifying order details: " + ex.Message);
            }


        }

        // Adding new orders by inserting rows into the DataTable
        static void AddNewOrder(DataSet dataSet)
        {
            try
            {
                DataTable ordersTable = dataSet.Tables["Orders"];

                DataRow newRow = ordersTable.NewRow();

                Console.Write("Enter Order ID: ");
                newRow["OrderID"] = int.Parse(Console.ReadLine());

                Console.Write("Enter Product Name: ");
                newRow["ProductName"] = Console.ReadLine();

                Console.Write("Enter Product Code: ");
                newRow["ProductCode"] = Console.ReadLine();

                Console.Write("Enter Product Size: ");
                newRow["ProductSize"] = Console.ReadLine();

                Console.Write("Enter Customer Address: ");
                newRow["CustomerAddress"] = Console.ReadLine();

                Console.Write("Enter Customer Contact: ");
                newRow["CustomerContact"] = Console.ReadLine();

                Console.Write("Enter Product Quantity: ");
                newRow["ProductQuantity"] = int.Parse(Console.ReadLine());

                Console.Write("Enter Price: ");
                newRow["Price"] = decimal.Parse(Console.ReadLine());

                Console.Write("Enter Customer Name: ");
                newRow["CustomerName"] = Console.ReadLine();

                ordersTable.Rows.Add(newRow);

                Console.WriteLine("\nNew order added successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError adding new order: " + ex.Message);
            }


        }


        // Removing orders by deleting corresponding rows from the DataTable.
        static void RemoveOrder(DataSet dataSet)
        {
            try
            {
                DataTable ordersTable = dataSet.Tables["Orders"];

                Console.Write("\nEnter Order ID to remove: ");
                int orderId = int.Parse(Console.ReadLine());

                DataRow[] foundRows = ordersTable.Select($"OrderID = {orderId}");

                if (foundRows.Length == 0)
                {
                    Console.WriteLine($"\nOrder with ID {orderId} not found.");
                    return;
                }

                foundRows[0].Delete();

                Console.WriteLine("\nOrder removed successfully.");


            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError removing order: " + ex.Message);
            }


        }



        // Synchronizing Changes with Database
        static void SynchronizeChangesWithDatabase(DataSet dataSet, SqlDataAdapter dataAdapter)
        {
            try
            {

                //Instead of defining lengthy insert, update, delete commands code i am using sqlcommandbuilder automatically generate the necessary SQL commands
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);


                dataAdapter.Update(dataSet, "Orders");
                Console.WriteLine("\nChanges synchronized with database successfully.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError synchronizing changes with database: " + ex.Message);
            }


        }


    }
}
