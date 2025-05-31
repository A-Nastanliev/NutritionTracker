using SQLite;
using DataLayer;

namespace BusinessLayer
{
    public class MealDayService : IDbService<MealDay, int>
    {
        private readonly SQLiteAsyncConnection db = DatabaseService.GetConnection();
        public async Task CreateAsync(MealDay obj)
        {
            await db.InsertAsync(obj);
        }

        public async Task<List<MealDay>> ReadAllAsync()
        {
            var mealDays = await db.Table<MealDay>().ToListAsync();
            if (!mealDays.Any())
                return mealDays;

            var mealDayIds = mealDays.Select(md => md.Id).ToList();

            var meals = await db.Table<Meal>()
                                .Where(m => mealDayIds.Contains(m.MealDayId))
                                .ToListAsync();

            var mealIds = meals.Select(m => m.Id).ToList();

            var allMealFoods = await db.Table<MealFood>()
                                       .Where(mf => mealIds.Contains(mf.MealId))
                                       .ToListAsync();

            foreach (var meal in meals)
            {
                var mealFoods = allMealFoods.Where(mf => mf.MealId == meal.Id).ToList();
                foreach (var mf in mealFoods)
                {
                    mf.Food = await db.Table<Food>().FirstOrDefaultAsync(f => f.Id == mf.FoodId);
                }
                meal.MealFoods = mealFoods;
            }

            foreach (var mealDay in mealDays)
            {
                mealDay.Meals = meals.Where(m => m.MealDayId == mealDay.Id).ToList();
            }

            return mealDays;
        }

        public async Task<MealDay> ReadAsync(int id)
        {
            var mealDay = await db.FindAsync<MealDay>(id);
            if (mealDay == null)
                return null;

            var meals = await db.Table<Meal>()
                                .Where(m => m.MealDayId == id)
                                .ToListAsync();

            if (meals.Any())
            {
                var mealIds = meals.Select(m => m.Id).ToList();

                var allMealFoods = await db.Table<MealFood>()
                                           .Where(mf => mealIds.Contains(mf.MealId))
                                           .ToListAsync();

                foreach (var meal in meals)
                {
                    meal.MealFoods = allMealFoods.Where(mf => mf.MealId == meal.Id).ToList();
                }
            }

            mealDay.Meals = meals;
            return mealDay;
        }

        public async Task UpdateAsync(MealDay obj)
        {
            var existing = await db.FindAsync<MealDay>(obj.Id);
            if (existing == null) return;

            await db.UpdateAsync(obj);
        }

        public async Task DeleteAsync(int id)
        {
            var meals = await db.Table<Meal>()
                         .Where(m => m.MealDayId == id)
                         .ToListAsync();

            if (meals.Any())
            {
                var mealIds = meals.Select(m => m.Id).ToList();

                string idsParam = string.Join(",", mealIds);
                await db.ExecuteAsync($"DELETE FROM MealFood WHERE MealId IN ({idsParam})");

                await db.ExecuteAsync("DELETE FROM Meal WHERE MealDayId = ?", id);
            }

            await db.DeleteAsync<MealDay>(id);
        }
    }
}
