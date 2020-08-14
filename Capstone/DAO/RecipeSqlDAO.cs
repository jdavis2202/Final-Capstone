using Capstone.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public class RecipeSqlDAO : IRecipeDAO
    {
        private readonly string connectionString;

        public RecipeSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Recipe GetRecipeById(int id)
        {
            Recipe returnRecipe = null;

            string sql = @"select r.recipe_id, r.recipe_name, r.description, r.instructions, i.ingredients_id, i.ingredients_name
                           from recipes as r 
                           inner join recipe_ingredients as ri on ri.recipe_id = r.recipe_id
                           inner join ingredients as i on ri.ingredients_id = i.ingredients_id
                           where ri.recipe_id = @recipe_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@recipe_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (returnRecipe == null)
                        {
                            returnRecipe = GetRecipeFromReader(reader);
                        }
                        returnRecipe.Ingredients.Add(GetIngredientFromReader(reader));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return returnRecipe;
        }

        public List<Recipe> GetRecipes()
        {
            List<Recipe> recipes = new List<Recipe>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from recipes where recipe_id > 1 order by recipe_name", connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Recipe recipe = GetRecipeFromReader(reader);
                        recipes.Add(recipe);
                    }
                }
            }
            return recipes;
        }


       
        public Recipe NewRecipe(Recipe recipe)
        {
            int recipeId = 0;

            string sql = @"BEGIN TRANSACTION
            Insert into recipes (recipe_name, description, instructions)
            Values (@recipe_name, @description, @instructions)
            Insert into recipe_ingredients (recipe_id, ingredients_id)
			Values (@@IDENTITY, 1)
            COMMIT TRANSACTION";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@recipe_name", recipe.RecipeName);
                    cmd.Parameters.AddWithValue("@description", recipe.Description);
                    cmd.Parameters.AddWithValue("@instructions", recipe.Instructions);
                    recipeId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return recipe;
        }

        public bool UpdateRecipe(Recipe updatedRecipe)
        {
            string sqlStart =
                @"BEGIN TRANSACTION
                    Update Recipes set recipe_name = @recipe_name, description = @description, instructions = @instructions where recipe_id = @recipe_id
                    Delete from recipe_ingredients where recipe_id = @recipe_id
                    Insert into recipe_ingredients (recipe_id, ingredients_id) VALUES ";
                    


                 
                   
            string sqlEnd = @"COMMIT TRANSACTION";

            string ingString = "";

            for (int i = 0; i < updatedRecipe.Ingredients.Count; i++)
            {
                Ingredient ing = updatedRecipe.Ingredients[i];
                if (ing.IngredientId == 0)
                {
                    continue;
                }
                if (i > 0)
                {
                    ingString += ",";
                }
                ingString += $"(@recipe_id, {ing.IngredientId})";
            }

            string sql = sqlStart + ingString + sqlEnd;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@recipe_name", updatedRecipe.RecipeName);
                    cmd.Parameters.AddWithValue("@description", updatedRecipe.Description);
                    cmd.Parameters.AddWithValue("@instructions", updatedRecipe.Instructions);
                    cmd.Parameters.AddWithValue("@recipe_id", updatedRecipe.RecipeId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return (rowsAffected > 0);
                }
            }
            catch (SqlException)
            {
                throw;
            }
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

        private Ingredient GetIngredientFromReader(SqlDataReader reader)
        {
            Ingredient ingredient = new Ingredient();
            ingredient.IngredientName = Convert.ToString(reader["ingredients_name"]);
            ingredient.IngredientId = Convert.ToInt32(reader["ingredients_id"]);

            return ingredient;
        }

        //METHOD 2 - Insert ingredient for the given RecipeId, POST
        public bool AddIngredientsToRecipe(Recipe updatedRecipe)
        {
            string sqlStart =
               @"BEGIN TRANSACTION
                    Insert into recipe_ingredients (recipe_id, ingredients_id) VALUES ";





            string sqlEnd = @"COMMIT TRANSACTION";

            string ingString = "";

            for (int i = 0; i < updatedRecipe.Ingredients.Count; i++)
            {
                Ingredient ing = updatedRecipe.Ingredients[i];
                if (ing.IngredientId == 0)
                {
                    continue;
                }
                if (i > 0)
                {
                    ingString += ",";
                }
                ingString += $"(@recipe_id, {ing.IngredientId})";
            }

            string sql = sqlStart + ingString + sqlEnd;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@recipe_id", updatedRecipe.RecipeId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return (rowsAffected > 0);
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }
    }
}

