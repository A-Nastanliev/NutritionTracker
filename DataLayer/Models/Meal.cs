using SQLite;

namespace DataLayer
{
    public class Meal : ICalorieTrackable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public MealType Type { get; set; }

        [Indexed]
        public int MealDayId { get; set; }
        [Ignore]
        public List<MealFood> MealFoods { get; set; }

        public double? GetCalories()
        {
            double? calories =0;

            for(int i=0; i<MealFoods.Count; ++i)
            {
                calories += MealFoods[i].GetCalories();
            }

            return calories;
        }

        public double? GetProteins()
        {
            double? proteins = 0;

            for(int i=0; i<MealFoods.Count; ++i)
            {
                proteins += MealFoods[i].GetProteins();
            }

            return proteins;
        }

        public double? GetCarbohydrates()
        {
            double? carbohydrates = 0;

            for (int i = 0; i < MealFoods.Count; ++i)
            {
                carbohydrates += MealFoods[i].GetCarbohydrates();
            }

            return carbohydrates;
        }

        public double? GetFats()
        {
            double? fats = 0;

            for (int i = 0; i < MealFoods.Count; ++i)
            {
                fats += MealFoods[i].GetFats();
            }

            return fats;
        }
    }
}
