using BusinessLayer;
using DataLayer;

namespace NutritionTrackerConsole
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Menu();
			List<string> input = null;
			while (input[0] != "stop")
			{
				input = Console.ReadLine().Split().ToList();
				switch (input[0])
				{
					case "create":
						Create(input);
						break;
					case "read":
						Read(input);
						break;
					case "read_all":
						ReadAll(input);
						break;
					case "update":
						break;
					case "delete":
						break;
					case "help":
						Help();
						break;
					case "menu":
						Menu();
						break;
				}
			}
			Console.WriteLine("Thank you for using our service");
		}
		static void Help()
		{
			Console.WriteLine("Models: food, meal, meal_day, meal_food");
		}
		static void Menu()
		{
			Console.WriteLine("---->Menu<----");
			Console.WriteLine("create [model]");
			Console.WriteLine("read [model] [model id]");
			Console.WriteLine("read_all [model]");
			Console.WriteLine("update [model] [model id]");
			Console.WriteLine("delete [model] [model id]");
			Console.WriteLine("help -> lists all models");
			Console.WriteLine("menu");
			Console.WriteLine("stop");
		}
		static async void Read(List<string> input)
		{
			switch (input[1])
			{
				case "food":
					FoodService foodService = new FoodService();
					Food food = await foodService.ReadAsync(Convert.ToInt32(input[2]));
					Console.WriteLine($"Id: {food.Id}, Name: {food.Name}, Calories: {food.Calories}," +
						$" Protein:{food.Proteins}, Carbohydrates: {food.Carbohydrates}, Fats: {food.Fats}");
					break;
				case "meal_food":
					MealFoodService mealFoodService = new MealFoodService();
					MealFood mealFood = await mealFoodService.ReadAsync(Convert.ToInt32(input[2]));
					PrintMealFood(mealFood);
					break;
				case "meal":
					MealService mealService = new MealService();
					Meal meal = await mealService.ReadAsync(Convert.ToInt32(input[2]));
					PrintMeal(meal);
					break;
				case "meal_day":
					MealDayService mealDayService = new MealDayService();
					MealDay mealDay = await mealDayService.ReadAsync(Convert.ToInt32(input[2]));
					PrintMealDay(mealDay);
					break;
			}
		}
		static void PrintMealFood(MealFood mealFood)
		{
			Console.WriteLine($"meal food id: {mealFood.Id}, weight: {mealFood.Weight}");
			Console.WriteLine($"meal: {mealFood.MealId}");
			Console.WriteLine($"food id: {mealFood.FoodId}, food name: {mealFood.Food.Name}");
			Console.WriteLine($"calories: {mealFood.GetCalories()}, protein: {mealFood.GetProteins}, carbohydrates: {mealFood.GetCarbohydrates}, fats: {mealFood.GetFats}");
			Console.WriteLine();
		}
		static void PrintMeal(Meal meal)
		{
			Console.WriteLine($"meal id: {meal.Id}, meal type: {meal.Type.ToString()}");
			Console.WriteLine($"meal calories: {meal.GetCalories()}, protein: {meal.GetProteins}, carbohydrates: {meal.GetCarbohydrates}, fats: {meal.GetFats}");
			Console.WriteLine();
			foreach(MealFood mealFood in meal.MealFoods)
			{
				PrintMealFood(mealFood);
				Console.WriteLine();
			}
		}
		static void PrintMealDay(MealDay mealDay)
		{
			Console.WriteLine($"id: {mealDay.Id}, date: {mealDay.Date}");
			Console.WriteLine($"meal day calories: { mealDay.GetCalories}, protein: {mealDay.GetProteins}, carbohydrates: {mealDay.GetCarbohydrates}, fats: {mealDay.GetFats}");
			Console.WriteLine();
			foreach(Meal m in mealDay.Meals)
			{
				PrintMeal(m);
				Console.WriteLine();
			}
		}
		static async void ReadAll(List<string> input)
		{
			switch (input[1])
			{
				case "food":
					FoodService foodService = new FoodService();
					List<Food> foods = await foodService.ReadAllAsync();
					foreach(Food food in foods)
					{
						Console.WriteLine($"Id: {food.Id}, Name: {food.Name}, Calories: {food.Calories}," +
						$" Protein:{food.Proteins}, Carbohydrates: {food.Carbohydrates}, Fats: {food.Fats}");
					}
					break;
				case "meal_food":
					MealFoodService mealFoodService = new MealFoodService();
					List<MealFood> mealfoods = await mealFoodService.ReadAllAsync();
					foreach(MealFood mf in mealfoods)
					{
						PrintMealFood(mf);
					}
					break;
				case "meal":
					MealService mealService = new MealService();
					List<Meal> meals = await mealService.ReadAllAsync();
					foreach (Meal m in meals)
					{
						PrintMeal(m);
					}
					break;
				case "meal_day":
					MealDayService mealDayService = new MealDayService();
					List<MealDay> mealDays = await mealDayService.ReadAllAsync();
					foreach( MealDay md in mealDays)
					{
						PrintMealDay(md);
					}
					break;
			}
		}
		static async void Update(List<string> input)
		{
			switch (input[1])
			{
				case "food":

					break;
				case "meal_food":
					break;
				case "meal":
					break;
				case "meal_day":
					break;
			}
		}
		static async void Delete(List<string> input)
		{
			switch (input[1])
			{
				case "food":
					break;
				case "meal_food":
					break;
				case "meal":
					break;
				case "meal_day":
					break;
			}
		}

		static async void Create(List<string> input)
		{
			List<string> inputM = new List<string>();
			switch (input[1])
			{
				case "food":
					Console.WriteLine(" [(string) name] (all double) [calories] [proteins] [carbohydrates] [fats]");
					inputM = Console.ReadLine().Split().ToList();
					FoodService foodService = new FoodService();
					Food food = new Food
					{
						Name = inputM[0],
						Calories = Convert.ToDouble(inputM[1]),
						Proteins = Convert.ToDouble(inputM[2]),
						Carbohydrates = Convert.ToDouble(inputM[3]),
						Fats = Convert.ToDouble(inputM[4])
					};
					await foodService.CreateAsync(food);
					Console.WriteLine($"{food.Name} has id {food.Id}");
					break;
				case "meal_food":
					Console.WriteLine(" [(double) weight] [(int) food_id] [(int) meal_id]");
					inputM = Console.ReadLine().Split().ToList();
					MealFoodService mealFoodService = new MealFoodService();
					MealFood mealFood = new MealFood
					{
						Weight = Convert.ToDouble(inputM[0]),
						FoodId = Convert.ToInt32(inputM[1]),
						MealId = Convert.ToInt32(inputM[2]),
					};
					await mealFoodService.CreateAsync(mealFood);
					Console.WriteLine($"id: {mealFood.Id}");
					break;
				case "meal":
					Console.WriteLine(" [(Enum) meal_type] [(int) meal_day_id]");
					Console.WriteLine("to list meal types -> list_meal_types");
					inputM = Console.ReadLine().Split().ToList();
					if (inputM[0] == "list_meal_types")
					{
						Console.WriteLine("Meal types are : breakfast, lunch, dinner, snack");
						inputM = Console.ReadLine().Split().ToList();
					}
					byte mealTypeIndex = 0;
					switch (inputM[0])
					{
						case "breakfast":
							mealTypeIndex = 0;
							break;
						case "lunch":
							mealTypeIndex = 1;
							break;
						case "dinner":
							mealTypeIndex = 2;
							break;
						case "snack":
							mealTypeIndex = 3;
							break;
					}
					Meal meal = new Meal
					{
						MealDayId = Convert.ToInt32(inputM[1]),
						Type = (MealType)mealTypeIndex
					};
					MealService mealService = new MealService();
					await mealService.CreateAsync(meal);
					Console.WriteLine("meal id: " + meal.Id);
					break;
				case "meal_day":
					Console.WriteLine("Enter date -> [DD/MM/YYYY]");
					DateTime dateTime = DateTime.Parse(Console.ReadLine());
					MealDay mealDay = new MealDay { Date = dateTime };
					MealDayService mealDayService = new MealDayService();
					await mealDayService.CreateAsync(mealDay);
					Console.WriteLine("meal day id: " + mealDay.Id);					
					break;
			}

		}
	}
}
