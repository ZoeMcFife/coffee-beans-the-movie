namespace WineApi.Model.Contraints
{
    public class FermentationEntryConstraints
    {
        public static float MinDensity = 0;
        public static float MaxDensity = 100000;

        public static (bool IsValid, string ErrorMessage) CheckFermentationEntry(FermentationEntry entry)
        {
            List<string> errors = new();

            if (entry == null)
                return (false, "Fermentation entry cannot be null.");

            if (entry.Density < MinDensity || entry.Density > MaxDensity)
                errors.Add($"Density must be between {MinDensity} and {MaxDensity}.");

            if (errors.Count == 0)
                return (true, string.Empty); // No errors

            return (false, string.Join("\n", errors)); // Return errors as a single string
        }
    }
}
