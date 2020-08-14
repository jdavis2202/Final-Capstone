using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private IMealDAO mealDAO;

        public MealController(IMealDAO mealDAO)
        {
            this.mealDAO = mealDAO;
        }

        [HttpGet("")]
        public List<Meal> GetMeals()
        {
            return mealDAO.GetMeals();
        }

        [HttpGet("{id}")]
        public ActionResult<Meal> GetOneMeal(int id)
        {
            Meal meal = mealDAO.GetMealById(id);
            if (meal == null)
            {
                return NotFound();
            }
            return meal;
        }

        [HttpPost]
        public ActionResult<Meal> Add(Meal meal)
        {
            Meal newMeal = mealDAO.NewMeal(meal);
            return Created($"/{newMeal.MealId}", newMeal);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Meal meal)
        {
            meal.MealId = id;
            bool updated = mealDAO.UpdateMeal(meal);
            if (updated)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}