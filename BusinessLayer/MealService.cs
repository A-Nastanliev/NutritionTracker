using SQLite;
using DataLayer;

namespace BusinessLayer
{
    public class MealService : IDbService<Meal, int>
    {
        private readonly SQLiteAsyncConnection db = DatabaseService.GetConnection();

        public async Task CreateAsync(Meal obj)
        {
            await db.InsertAsync(obj);
        }

        public async Task<List<Meal>> ReadAllAsync()
        {
            var meals = await db.Table<Meal>().ToListAsync();
            var mealIds = meals.Select(m => m.Id).ToList();

            var allMealFoods = await db.Table<MealFood>()
                                        .Where(mf => mealIds.Contains(mf.MealId))
                                        .ToListAsync();

            foreach (var meal in meals)
            {
                meal.MealFoods = allMealFoods.Where(mf => mf.MealId == meal.Id).ToList();
            }

            return meals;
        }

        public async Task<Meal> ReadAsync(int id)
        {
            var meal = await db.FindAsync<Meal>(id);
            meal.MealFoods = await db.Table<MealFood>().Where(mf => mf.MealId == id).ToListAsync();
            return meal;
        }

        public async Task UpdateAsync(Meal obj)
        {
            var existing = await db.FindAsync<Meal>(obj.Id);
            if (existing == null) return;

            await db.UpdateAsync(obj);
        }

        public async Task DeleteAsync(int id)
        {
            await db.ExecuteAsync("DELETE FROM MealFood WHERE MealId = ?", id);

            await db.DeleteAsync<Meal>(id);
        }
    }
}
