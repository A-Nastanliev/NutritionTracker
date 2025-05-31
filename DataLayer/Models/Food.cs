using SQLite;

namespace DataLayer
{
    public class Food
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }
        public double? Calories { get; set; }
        public double? Fats { get; set; }
        public double? Carbohydrates { get; set; }
        public double? Proteins { get; set; }
    }
}
