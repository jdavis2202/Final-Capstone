using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public class IngredientSqlDAO : IIngredientDAO
    {
        private readonly string connectionString;

        public IngredientSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Ingredient> GetIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from ingredients where ingredients_id > 1 order by ingredients_name ", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Ingredient ingredient = GetIngredientFromReader(reader);
                        ingredients.Add(ingredient);
                    }
                }
            }
            return ingredients;
        }

        private Ingredient GetIngredientFromReader(SqlDataReader reader)
        {
            Ingredient ingredient = new Ingredient();
            ingredient.IngredientName = Convert.ToString(reader["ingredients_name"]);
            ingredient.IngredientId = Convert.ToInt32(reader["ingredients_id"]);
            
          
            return ingredient;
        }

        public Ingredient NewIngredient(Ingredient ingredient)
        {
            int ingredientId = 0;

            string sql = @"
            Insert into ingredients (ingredients_name)
            Values (@ingredients_name);";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ingredients_name", ingredient.IngredientName);
   
                    ingredientId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return ingredient;
        }
    }
}
