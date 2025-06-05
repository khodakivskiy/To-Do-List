﻿using Microsoft.Data.SqlClient;
using To_Do_List__Project.Models;

namespace To_Do_List__Project.Repositories
{
    public class SQLCategoryRepository
    {
        private readonly string _connectionString;

        public SQLCategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddDefaultCategories()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var categories = GetCategories();

                if (categories == null || categories.Count == 0)
                {
                    categories = new List<Category>
                    {
                        new() { Category_Name = "Робота" },
                        new() { Category_Name = "Особисте" },
                        new() { Category_Name = "Навчання" },
                        new() { Category_Name = "Покупки" }
                    };

                    foreach (var category in categories)
                    {
                        string insertQuery = "INSERT INTO Categories (Category_Name) VALUES (@Category_Name)";
                        using (var insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@Category_Name", category.Category_Name);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public List<Category> GetCategories()
        {
            var categories = new List<Category>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Categories";
                var command = new SqlCommand(query, connection);
                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        Category_Id = (int)reader["Category_Id"],
                        Category_Name = reader["Category_Name"].ToString()
                    });
                }
            }

            return categories;
        }

    }
}
