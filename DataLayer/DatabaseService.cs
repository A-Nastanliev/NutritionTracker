using SQLite;

namespace DataLayer
{
    public class DatabaseService
    {
        private const string dbName = "nutritionTracker.db";
        private static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
        private static SQLiteAsyncConnection database;

        public static async Task InitAsync()
        {
            if (database != null) return;

            database = new SQLiteAsyncConnection(dbPath);

            await database.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Food (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL CHECK(length(Name) <= 30),
                Calories REAL NOT NULL CHECK(Calories >= 0 AND Calories <= 900),
                Fats REAL NOT NULL CHECK(Fats >= 0 AND Fats <= 100),
                Carbohydrates REAL NOT NULL CHECK(Carbohydrates >= 0 AND Carbohydrates <= 100),
                Proteins REAL NOT NULL CHECK(Proteins >= 0 AND Proteins <= 100)
                );
                ");


            await database.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS MealDay (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT NOT NULL
                );
                ");


            await database.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Meal (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Type INTEGER NOT NULL CHECK(Type >= 0 AND Type <= 3),
                MealDayId INTEGER NOT NULL 
                );
                ");

            await database.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS MealFood (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Weight REAL NOT NULL CHECK(Weight > 0),
                FoodId INTEGER NOT NULL,
                MealId INTEGER NOT NULL
                );
                ");

        }

        public static SQLiteAsyncConnection GetConnection()
        {
            return database;
        }

        public static async void CreateTestDb(string testDbName)
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), testDbName);
            database = new SQLiteAsyncConnection(databasePath);

			await database.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Food (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL CHECK(length(Name) <= 30),
                Calories REAL NOT NULL CHECK(Calories >= 0 AND Calories <= 900),
                Fats REAL NOT NULL CHECK(Fats >= 0 AND Fats <= 100),
                Carbohydrates REAL NOT NULL CHECK(Carbohydrates >= 0 AND Carbohydrates <= 100),
                Proteins REAL NOT NULL CHECK(Proteins >= 0 AND Proteins <= 100)
                );
                ");


			await database.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS MealDay (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT NOT NULL
                );
                ");


			await database.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Meal (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Type INTEGER NOT NULL CHECK(Type >= 0 AND Type <= 3),
                MealDayId INTEGER NOT NULL 
                );
                ");

			await database.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS MealFood (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Weight REAL NOT NULL CHECK(Weight > 0),
                FoodId INTEGER NOT NULL,
                MealId INTEGER NOT NULL
                );
                ");
		}

        public static void DeleteTestDb(string testDbName)
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), testDbName);
            if (File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }
        }
    }
}
