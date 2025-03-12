namespace WineApi.Model.Contraints
{
    public class AdditiveConstraints
    {
        public static float MinAmountGrammsPerLitre = 0;
        public static float MaxAmountGrammsPerLitre = 100000;


        public static (bool IsValid, string ErrorMessage) CheckAdditive(Additive additive)
        {
            List<string> errors = new();

            if (additive == null)
                return (false, "Additive cannot be null.");

            if (additive.AmountGrammsPerLitre < MinAmountGrammsPerLitre || additive.AmountGrammsPerLitre > MaxAmountGrammsPerLitre)
                errors.Add($"Amount in grams per litre must be between {MinAmountGrammsPerLitre} and {MaxAmountGrammsPerLitre}.");

            if (errors.Count == 0)
                return (true, string.Empty); // No errors

            return (false, string.Join("\n", errors)); // Return errors as a single string
        }
    }
}
