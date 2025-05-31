namespace DataLayer
{
    public interface ICalorieTrackable
    {
        double? GetCalories();

        double? GetProteins();
        double? GetCarbohydrates();
        double? GetFats();
    }
}
