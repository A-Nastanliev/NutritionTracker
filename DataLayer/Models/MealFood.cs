using SQLite;

namespace DataLayer
{
    public class MealFood : ICalorieTrackable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public double? Weight { get; set; }

        public int FoodId { get; set; }
        public int MealId { get; set; }
        [Ignore]
        public Food Food { get; set; }

        public double? GetCalories()
        {
            return (Weight/100)*Food.Calories;
        } 

        public double? GetProteins()
        {
            return (Weight / 100) * Food.Proteins;
        }

        public double? GetCarbohydrates()
        {
            return (Weight / 100) * Food.Carbohydrates;
        }

        public double? GetFats() 
        {
            return (Weight / 100) * Food.Fats;
        } 
    }
}
