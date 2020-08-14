using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; } //This points to "RecipeIngredients"
        public string IngredientName { get; set; }

    }
}
