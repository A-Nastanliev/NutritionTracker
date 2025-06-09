using BusinessLayer;
using DataLayer;

namespace NutritionTrackerTests
{
    public class Tests
    {
        public string testDbName = "test.db";
        FoodService foodService;
		MealService mealService;
		MealFoodService mealFoodService;
		public int testFoodId;
		public int testMealId;
		public int testMealFoodId;
		[SetUp]
        public async Task Setup()
        {
            DatabaseService.CreateTestDb(testDbName);
            foodService = new FoodService();
            mealService = new MealService();
            mealFoodService = new MealFoodService();
			Food food = new Food
			{
				Name = "aaa",
				Calories = 200,
				Proteins = 20,
				Carbohydrates = 30,
				Fats = 50,
			};
			await foodService.CreateAsync(food);
			testFoodId = food.Id;
			Meal meal = new Meal
			{
				Type = MealType.Breakfast
			};
			await mealService.CreateAsync(meal);
			testMealId = meal.Id;
			Food food2 = new Food
			{
				Name = "aaaa",
				Calories = 201,
				Proteins = 20,
				Carbohydrates = 30,
				Fats = 50,
			};
			await foodService.CreateAsync(food2);
			MealFood mealFood = new MealFood
			{
				MealId = testMealId,
				FoodId = food2.Id,
				Weight = 100
			};
			await mealFoodService.CreateAsync(mealFood);
			testMealFoodId = mealFood.Id;
		}

        [Test]
        public async Task Food_Create_Succ()
        {
            Food food = new Food
            {
                Name = "aaa",
                Calories = 200,
                Proteins = 20,
                Carbohydrates = 30,
                Fats = 50,
            };
            await foodService.CreateAsync(food);
            Food newFood = await foodService.ReadAsync(food.Id);
            Assert.That(newFood, Is.Not.Null);
        }
		[Test]
		public async Task Food_Create_UnSucc()
		{
			Food food = new Food
			{
				Name = "aaa",
				Calories = 10000,
				Proteins = 20,
				Carbohydrates = 30,
				Fats = 50,
			};
			await foodService.CreateAsync(food);
			Food newFood = await foodService.ReadAsync(food.Id);
			Assert.That(newFood, Is.Null);
		}
		[Test]
		public async Task Food_Read_Succ()
		{
			
			Food newFood = await foodService.ReadAsync(testFoodId);
			Assert.That(newFood, Is.Not.Null);
		}
		[Test]
		public async Task Food_Read_UnSucc()
		{

			Food newFood = await foodService.ReadAsync(int.MaxValue);
			Assert.That(newFood, Is.Null);
		}
		[Test]
		public async Task Food_Update_Succ()
		{
			Food newFood = await foodService.ReadAsync(testFoodId);
			newFood.Name = "test";
			await foodService.UpdateAsync(newFood);
			Food result = await foodService.ReadAsync(testFoodId);
			Assert.That(result, Is.Not.Null);
			Assert.That(newFood.Name, Is.EqualTo(result.Name));
		}
		[Test]
		public async Task Food_Update_UnSucc()
		{
			Food newFood = await foodService.ReadAsync(testFoodId);
			newFood.Name = "testaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
			await foodService.UpdateAsync(newFood);
			Food result = await foodService.ReadAsync(testFoodId);
			Assert.That(result, Is.Not.Null);
			Assert.That(newFood.Name, Is.Not.EqualTo(result.Name));
		}
		[Test]
		public async Task Food_Remove_Succ()
		{
			Food food = new Food
			{
				Name = "aaa",
				Calories = 10000,
				Proteins = 20,
				Carbohydrates = 30,
				Fats = 50,
			};
			await foodService.CreateAsync(food);
			await foodService.DeleteAsync(food.Id);
			Food result = await foodService.ReadAsync(food.Id);
			Assert.That(result, Is.Null);
		}
		[Test]
		public async Task MealFood_Create_Succ()
		{
			Food food2 = new Food
			{
				Name = "aaaa",
				Calories = 201,
				Proteins = 20,
				Carbohydrates = 30,
				Fats = 50,
			};
			await foodService.CreateAsync(food2);
			MealFood mealFood = new MealFood
			{
				MealId = testMealId,
				FoodId = food2.Id,
				Weight = 100
			};
			await mealFoodService.CreateAsync(mealFood);
			MealFood result = await mealFoodService.ReadAsync(mealFood.Id);
			Assert.That(result, Is.Not.Null);
		}
		[Test]
		public async Task MealFood_Create_UnSucc()
		{
			Food food2 = new Food
			{
				Name = "aaaa",
				Calories = 201,
				Proteins = 20,
				Carbohydrates = 30,
				Fats = 50,
			};
			await foodService.CreateAsync(food2);
			MealFood mealFood = new MealFood
			{
				MealId = testMealId,
				FoodId = food2.Id,
				Weight = -100
			};
			await mealFoodService.CreateAsync(mealFood);
			MealFood result = await mealFoodService.ReadAsync(mealFood.Id);
			Assert.That(result, Is.Null);
		}

		[TearDown] 
        public void TearDown() 
        {
            DatabaseService.DeleteTestDb(testDbName);
        }
    }
}
