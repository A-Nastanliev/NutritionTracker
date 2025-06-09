using SQLite;
using DataLayer;

namespace BusinessLayer
{
    public class FoodService : IDbService<Food, int>
    {
        private readonly SQLiteAsyncConnection db = DatabaseService.GetConnection();

        public async Task CreateAsync(Food food)
        {
            await db.InsertAsync(food);
        }

        public async Task<List<Food>> ReadAllAsync()
        {
            return await db.Table<Food>().ToListAsync();
        }

        public async Task<Food> ReadAsync(int id)
        {
            return await db.FindAsync<Food>(id);
        }

        public async Task UpdateAsync(Food food)
        {
            var existing = await db.FindAsync<Food>(food.Id);
            if (existing == null) return;

            await db.UpdateAsync(food);
        }

        public async Task DeleteAsync(int id)
        {
            await db.DeleteAsync<Food>(id);
        }

        public async Task ClearAsync()
        {
            await db.ExecuteAsync("DELETE FROM Food");
        }
    }
}
