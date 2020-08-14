using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public interface IRecipeDAO
    {
        List<Recipe> GetRecipes();

        Recipe GetRecipeById(int id);

        Recipe NewRecipe(Recipe recipe);

        bool UpdateRecipe(Recipe updatedRecipe);
        bool AddIngredientsToRecipe(Recipe updatedRecipe);

    }
}
