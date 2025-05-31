using DataLayer;
using SQLite;
using System;
namespace DataLayer
{
    public class MealDay : ICalorieTrackable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Ignore]
        public List<Meal> Meals { get; set; }

        public double? GetCalories()
        {
            double? calories = 0;

            for (int i = 0; i < Meals.Count; i++)
            {
                calories += Meals[i].GetCalories();
            }

            return calories;
        }

        public double? GetProteins()
        {
            double? proteins = 0;

            for (int i = 0; i < Meals.Count; ++i)
            {
                proteins += Meals[i].GetProteins();
            }

            return proteins;
        }

        public double? GetCarbohydrates()
        {
            double? carbohydrates = 0;

            for (int i = 0; i < Meals.Count; ++i)
            {
                carbohydrates += Meals[i].GetCarbohydrates();
            }

            return carbohydrates;
        }

        public double? GetFats()
        {
            double? fats = 0;

            for (int i = 0; i < Meals.Count; ++i)
            {
                fats += Meals[i].GetFats();
            }

            return fats;
        }
    }
}
