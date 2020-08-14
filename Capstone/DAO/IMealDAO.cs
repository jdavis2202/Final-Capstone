using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.DAO
{
    public interface IMealDAO
    {
        List<Meal> GetMeals();

        Meal NewMeal(Meal meal);

        Meal GetMealById(int id);

        bool UpdateMeal(Meal updatedMeal);
    }
}
