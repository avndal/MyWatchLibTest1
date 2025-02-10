using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace MyWatchLib
{
    public class MyWatchRepository
    {
        private readonly string _connectionString;

        public MyWatchRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Watch watch)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO MyWatch (Name, Model, Price) VALUES (@Name, @Model, @Price)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", watch.Name);
                cmd.Parameters.AddWithValue("@Model", watch.Model);
                cmd.Parameters.AddWithValue("@Price", watch.Price);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Watch GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM MyWatch WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Watch
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Model = reader["Model"].ToString(),
                            Price = (decimal)reader["Price"]
                        };
                    }
                }
            }
            return null;
        }

        public List<Watch> GetAll()
        {
            List<Watch> watches = new List<Watch>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM MyWatch";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        watches.Add(new Watch
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            Model = reader["Model"].ToString(),
                            Price = (decimal)reader["Price"]
                        });
                    }
                }
            }
            return watches;
        }

        public void Update(Watch watch)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "UPDATE MyWatch SET Name = @Name, Model = @Model, Price = @Price WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", watch.Id);
                cmd.Parameters.AddWithValue("@Name", watch.Name);
                cmd.Parameters.AddWithValue("@Model", watch.Model);
                cmd.Parameters.AddWithValue("@Price", watch.Price);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM MyWatch WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
