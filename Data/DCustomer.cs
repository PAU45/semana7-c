using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Data
{
    public class DCustomer
    {

        private string connectionString = "Data Source=DESKTOP-DLET8R7\\SQLEXPRESS;Database=INVOICESABD;User ID=paulin;Password=123456;TrustServerCertificate=true";
        public List<Customer> Read()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_ListCustomers", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer c = new Customer
                        {
                            CustomerID = Convert.ToInt32(reader["customer_id"]),
                            Name = reader["name"].ToString(),
                            Address = reader["address"] != DBNull.Value ? reader["address"].ToString() : string.Empty,
                            Phone = reader["phone"] != DBNull.Value ? reader["phone"].ToString() : string.Empty,
                            Active = Convert.ToBoolean(reader["active"])
                        };

                        customers.Add(c);
                    }
                }
            }

            return customers;
        }
    }
}
