using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WineApi.Model.Contraints
{
    public class WineBarrelConstraints
    {
        public static float MinMostWeight = 1;
        public static float MaxMostWeight = 100000;

        public static float MinVolume = 0;
        public static float MaxVolume = 100000;

        public static (bool IsValid, string ErrorMessage) CheckBarrel(WineBarrel barrel)
        {
            List<string> errors = new();

            if (barrel == null)
            {
                return (false, "Wine barrel cannot be null.");
            }

            if (barrel.MostWeight < MinMostWeight || barrel.MostWeight > MaxMostWeight)
                errors.Add($"MostWeight must be between {MinMostWeight} and {MaxMostWeight}.");

            if (barrel.VolumeInLitre < MinVolume || barrel.VolumeInLitre > MaxVolume)
                errors.Add($"VolumeInLitre must be between {MinVolume} and {MaxVolume}.");

            if (string.IsNullOrWhiteSpace(barrel.Name))
                errors.Add("Name cannot be empty or whitespace.");

            if (errors.Count == 0)
                return (true, string.Empty);

            return (false, string.Join("\n", errors));
        }
    }
}
