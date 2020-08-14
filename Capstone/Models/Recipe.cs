using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; } //This points to "RecipeIngredients"
        public string RecipeName { get; set; }
        public string Description { get; set; }

        public string Instructions { get; set; }

        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }
}
