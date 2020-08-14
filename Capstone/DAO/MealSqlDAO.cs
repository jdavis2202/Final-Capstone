using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public class MealSqlDAO : IMealDAO
    {
        private readonly string connectionString;

        public MealSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;

        }

        public List<Meal> GetMeals()
        {
            List<Meal> meals = new List<Meal>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"select * from meals
                                order by meal_date,
                                case when meal_type = 'Breakfast' then 1
                                when meal_type = 'Brunch' then 2
                                when meal_type = 'Lunch' then 3
                                when meal_type = 'Dinner' then 4
                                else (ROW_NUMBER() OVER (order by meal_type)+4) end asc";

                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Meal meal = GetMealFromReader(reader);
                        meals.Add(meal);
                    }
                }
            }
            return meals;
        }

        private Meal GetMealFromReader(SqlDataReader reader)
        {
            Meal meal = new Meal();
            meal.MealId = Convert.ToInt32(reader["meal_id"]);
            meal.MealDate = Convert.ToDateTime(reader["meal_date"]);
            meal.MealName = Convert.ToString(reader["meal_name"]);
            meal.MealType = Convert.ToString(reader["meal_type"]);

            return meal;

        }
        private Recipe GetRecipeFromReader(SqlDataReader reader)
        {
            Recipe recipe = new Recipe();
            recipe.RecipeName = Convert.ToString(reader["recipe_name"]);
            recipe.RecipeId = Convert.ToInt32(reader["recipe_id"]);
            recipe.Description = Convert.ToString(reader["description"]);
            recipe.Instructions = Convert.ToString(reader["instructions"]);

            return recipe;
        }

        public Meal NewMeal(Meal meal)
        {
            int mealId = 0;

            string sql = @"BEGIN TRANSACTION
            Insert into meals (meal_date, meal_name, meal_type)
            Values (@meal_date, @meal_name, @meal_type);
            Insert into meal_recipe (meal_id, recipe_id)
			Values (@@IDENTITY, 1)
            COMMIT TRANSACTION";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@meal_date", meal.MealDate);
                    cmd.Parameters.AddWithValue("@meal_name", meal.MealName);
                    cmd.Parameters.AddWithValue("@meal_type", meal.MealType);
                    mealId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return meal;
        }

        public bool UpdateMeal(Meal updatedMeal)
        {
            string sqlStart =
                @"BEGIN TRANSACTION
                    Update meals set meal_date = @meal_date, meal_name = @meal_name, meal_type = @meal_type where meal_id = @meal_id
                    Delete from meal_recipe where meal_id = @meal_id
                    Insert into meal_recipe (meal_id, recipe_id) VALUES ";





            string sqlEnd = @"COMMIT TRANSACTION";

            string recipeString = "";

            for (int i = 0; i < updatedMeal.Recipes.Count; i++)
            {
                Recipe rec = updatedMeal.Recipes[i];
                if (rec.RecipeId == 0)
                {
                    continue;
                }
                if (i > 0)
                {
                    recipeString += ",";
                }
                recipeString += $"(@meal_id, {rec.RecipeId})";
            }

            string sql = sqlStart + recipeString + sqlEnd;


            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@meal_date", updatedMeal.MealDate);
                    cmd.Parameters.AddWithValue("@meal_name", updatedMeal.MealName);
                    cmd.Parameters.AddWithValue("@meal_type", updatedMeal.MealType);
                    cmd.Parameters.AddWithValue("@meal_id", updatedMeal.MealId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return (rowsAffected > 0);
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public Meal GetMealById(int id)
        {
            Meal returnMeal = null;

            string sql = @"select m.meal_id, m.meal_date, m.meal_name, m.meal_type, r.recipe_id, r.recipe_name, r.description, r.instructions
                            from meals as m
                            inner join meal_recipe as mr on m.meal_id = mr.meal_id
                            inner join recipes as r on r.recipe_id = mr.recipe_id
                            where mr.meal_id = @meal_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@meal_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (returnMeal == null)
                        {
                            returnMeal = GetMealFromReader(reader);
                        }
                        returnMeal.Recipes.Add(GetRecipeFromReader(reader));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return returnMeal;
        }
    }
}
