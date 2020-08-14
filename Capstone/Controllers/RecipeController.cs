using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private IRecipeDAO recipeDAO;

        public RecipeController(IRecipeDAO recipeDAO)
        {
            this.recipeDAO = recipeDAO;
        }

        [HttpGet("recipe")]
        public List<Recipe> GetRecipes()
        {
            return recipeDAO.GetRecipes();
        }

        [HttpGet("recipe/{id}")]
        public ActionResult<Recipe> GetOneRecipe(int id)
        {
            Recipe recipe = recipeDAO.GetRecipeById(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return recipe;
        }

        [HttpPost("recipe")]
        public ActionResult<Recipe> Add(Recipe recipe)
        {
            Recipe newRecipe = recipeDAO.NewRecipe(recipe);
            return Created($"/{newRecipe.RecipeId}", newRecipe);
        }

        [HttpPut("recipe/{id}")]
        public ActionResult Update(int id, Recipe recipe)
        {
            recipe.RecipeId = id;
            bool updated = recipeDAO.UpdateRecipe(recipe);
            if (updated)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("updateRecipe/{id}")]
        public ActionResult UpdateRecipe(int id, Recipe recipe)
        {
            recipe.RecipeId = id;
            bool updated = recipeDAO.UpdateRecipe(recipe);
            if (updated)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("updateIng/{id}")]
        public ActionResult UpdateIngredient(int id, Recipe recipe)
        {
            recipe.RecipeId = id;
            bool updated = recipeDAO.AddIngredientsToRecipe(recipe);
            if (updated)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        //[HttpPost("recipe")]
        //public ActionResult<Recipe> Add(Recipe recipe)
        //{
        //    Recipe newRecipe = recipeDAO.NewRecipe(recipe);
        //    return Created($"/{newRecipe.RecipeId}", newRecipe);
        //}

    }
}
