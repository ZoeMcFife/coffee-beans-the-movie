namespace WineApi.Model.DTO
{
    public class AdditiveDTO
    {
        public int Id { get; set; }
        public string Type { get; set; } // Type of additive (e.g., Schwefel, Säure)
        public DateTime Date { get; set; } // Datum
        public string Time { get; set; } // Uhrzeit
        public float AmountGrammsPerLitre { get; set; } // Menge (gr/L, gr/Hekto, gr/1000)
        public float AmountGrammsPerHectoLitre { get; set; } // Menge (gr/L, gr/Hekto, gr/1000)
        public float AmountGrammsPer1000Litre { get; set; } // Menge (gr/L, gr/Hekto, gr/1000)

        public int WineId { get; set; }

        public static Additive MapDtoToAdditive(AdditiveDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Additive
            {
                Id = dto.Id,
                Type = dto.Type,
                Date = dto.Date,
                Time = dto.Time,
                AmountGrammsPerLitre = dto.AmountGrammsPerLitre,
                AmountGrammsPerHectoLitre = dto.AmountGrammsPerHectoLitre,
                AmountGrammsPer1000Litre = dto.AmountGrammsPer1000Litre,
                Wine = null // Set to null; can be populated later if necessary
            };
        }

        public static AdditiveDTO MapAdditiveToDto(Additive additive)
        {
            if (additive == null) throw new ArgumentNullException(nameof(additive));

            return new AdditiveDTO
            {
                Id = additive.Id,
                Type = additive.Type,
                Date = additive.Date,
                Time = additive.Time,
                AmountGrammsPerLitre = additive.AmountGrammsPerLitre,
                AmountGrammsPerHectoLitre = additive.AmountGrammsPerHectoLitre,
                AmountGrammsPer1000Litre = additive.AmountGrammsPer1000Litre,
                WineId = additive.Wine.Id
            };
        }


    }
}
