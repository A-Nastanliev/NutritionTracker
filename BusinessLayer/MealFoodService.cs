using SQLite;
using DataLayer;

namespace BusinessLayer
{
    public class MealFoodService : IDbService<MealFood, int>
    {
        private readonly SQLiteAsyncConnection db = DatabaseService.GetConnection();

        public async Task CreateAsync(MealFood obj)
        {
            await db.InsertAsync(obj);
        }

        public async Task<List<MealFood>> ReadAllAsync()
        {
            var mealFoods = await db.Table<MealFood>().ToListAsync();

            var foodIds = mealFoods.Select(mf => mf.FoodId).Distinct().ToList();

            var foods = await db.Table<Food>().Where(f => foodIds.Contains(f.Id)).ToListAsync();

            var foodDict = foods.ToDictionary(f => f.Id);

            foreach (var mf in mealFoods)
            {
                mf.Food = foodDict[mf.FoodId];
            }

            return mealFoods;
        }

        public async Task<MealFood> ReadAsync(int id)
        {
            var mealFood = await db.FindAsync<MealFood>(id);
            Food food = await db.FindAsync<Food>(mealFood.FoodId);
            if(food != null) mealFood.Food = food;
            return mealFood;
        }

        public async Task UpdateAsync(MealFood obj)
        {
            var existing = await db.FindAsync<MealFood>(obj.Id);
            if (existing == null) return;

            await db.UpdateAsync(obj);
        }

        public async Task DeleteAsync(int id)
        {
            await db.DeleteAsync<MealFood>(id);
        }

        public async Task ClearAsync()
        {
            await db.ExecuteAsync("DELETE FROM MealFood");
        }

    }
}
