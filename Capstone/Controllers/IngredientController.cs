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
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private IIngredientDAO ingredientDAO;

        public IngredientController(IIngredientDAO ingredientDAO)
        {
            this.ingredientDAO = ingredientDAO;
        }

        [HttpGet("")]
        public List<Ingredient> GetIngredients()
        {
            return ingredientDAO.GetIngredients();
        }

        [HttpPost]
        public ActionResult<Ingredient> Add(Ingredient ingredient)
        {
            Ingredient newIngredient = ingredientDAO.NewIngredient(ingredient);
            return Created($"/{newIngredient.IngredientId}", newIngredient);
        }

    }
}