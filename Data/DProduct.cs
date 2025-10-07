using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Entity;

namespace Data
{
    public class DProduct
    {
        private readonly string _connectionString = "Server=DESKTOP-DLET8R7\\SQLEXPRESS;Database=INVOICESABD;Integrated Security=True;TrustServerCertificate=True";

        // SP requeridos: sp_list_products, sp_insert_product, sp_update_product, sp_delete_product, sp_get_product (opcional)
        public List<Product> Read()
        {
            var products = new List<Product>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_list_products", conn) { CommandType = CommandType.StoredProcedure };
            conn.Open();
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    ProductID = reader.GetInt32(reader.GetOrdinal("product_id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Price = reader.GetDecimal(reader.GetOrdinal("price")),
                    Stock = reader.GetInt32(reader.GetOrdinal("stock")),
                    Active = reader.GetBoolean(reader.GetOrdinal("active"))
                });
            }
            return products;
        }

        public Product? Get(int id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_get_product", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@product_id", id);
            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            if (!r.Read()) return null;
            return new Product
            {
                ProductID = r.GetInt32(r.GetOrdinal("product_id")),
                Name = r.GetString(r.GetOrdinal("name")),
                Price = r.GetDecimal(r.GetOrdinal("price")),
                Stock = r.GetInt32(r.GetOrdinal("stock")),
                Active = r.GetBoolean(r.GetOrdinal("active"))
            };
        }

        public int Insert(Product p)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_insert_product", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@name", p.Name);
            cmd.Parameters.AddWithValue("@price", p.Price);
            cmd.Parameters.AddWithValue("@stock", p.Stock);
            conn.Open();
            object? result = cmd.ExecuteScalar();
            return result is int id ? id : 0;
        }

        public bool Update(Product p)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_update_product", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@product_id", p.ProductID);
            cmd.Parameters.AddWithValue("@name", p.Name);
            cmd.Parameters.AddWithValue("@price", p.Price);
            cmd.Parameters.AddWithValue("@stock", p.Stock);
            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            return r.Read() && r["affected"] is int a && a == 1;
        }

        public bool Delete(int id)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("sp_delete_product", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@product_id", id);
            conn.Open();
            using SqlDataReader r = cmd.ExecuteReader();
            return r.Read() && r["affected"] is int a && a == 1;
        }
    }
}
