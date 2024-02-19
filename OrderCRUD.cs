using System;
using System.Data.SqlClient;

namespace QueenLocalDataHandling
{
    class ConnectionString
    {
        public static string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QueensDB;Integrated Security=True";
    }
    class OrderCRUD
    {
       
        //string connectionString = @"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=QueensDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public void InsertOrder(Order order)
        {
            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            
            try
            {
                if (order.Size != "Large" && order.Size != "Medium" && order.Size != "Small")
                {
                    Console.WriteLine("\nInvalid size. Size should be Large, Medium, or Small.");
                    Environment.Exit(0); 
                }

                connection.Open();
                string query = "INSERT INTO Orders VALUES ("+order.OrderID+",'"+order.CNIC+"','"+order.CustomerName+"','"+order.CustomerPhone+"','"+order.CustomerAddress+"',"+order.ProductID+","+order.Price+",'"+order.Size+"')";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting order: " + ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public void GetAllOrders()
        {
            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "SELECT * FROM Orders";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"OrderID: {reader[0]} \t CustomerCNIC: {reader[1]} CusotmerName: {reader[2]} CustomerPhone: {reader[3]} CustomerAddrress: {reader[4]} ProductID: {reader[5]} Price: {reader[6]} ProductSize: {reader[7]} ");
                    Console.WriteLine('\n');
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting orders: " + ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public void UpdateAddress(string customerPhone, string newAddress)
        {
            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = $"UPDATE Orders SET [CustomerAddress] = '{newAddress}' WHERE [CustomerPhone] = '{customerPhone}'";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating address: " + ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }


        public void UpdateOrderAddress(string customerPhone, string newAddress)
        {
            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = "UPDATE Orders SET CustomerAddress = @NewAddress WHERE CustomerPhone = @Phone";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NewAddress", newAddress);
                command.Parameters.AddWithValue("@Phone", customerPhone);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating address: " + ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public void DeleteOrder(int orderId)
        {
            SqlConnection connection = new SqlConnection(ConnectionString.constr);
            try
            {
                connection.Open();
                string query = $"DELETE FROM Orders WHERE [OrderID] = '{orderId}'";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting order: " + ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
    }
}