using Capstone.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Meal
    {
        public int MealId { get; set; }
        public DateTime MealDate { get; set; }
        public string MealName { get; set; }
        public string MealType { get; set; }
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
