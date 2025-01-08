namespace WineApi.Model.DTO
{
    public class AdditiveDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public float AmountGrammsPerLitre { get; set; }
        public float AmountGrammsPerHectoLitre { get; set; }
        public float AmountGrammsPer1000Litre { get; set; }

        public int WineId { get; set; }

        public static Additive MapDtoToAdditive(AdditiveDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Additive
            {
                Id = dto.Id,
                Type = dto.Type,
                Date = dto.Date,
                AmountGrammsPerLitre = dto.AmountGrammsPerLitre,
                AmountGrammsPerHectoLitre = dto.AmountGrammsPerHectoLitre,
                AmountGrammsPer1000Litre = dto.AmountGrammsPer1000Litre,
                WineId = dto.WineId
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
                AmountGrammsPerLitre = additive.AmountGrammsPerLitre,
                AmountGrammsPerHectoLitre = additive.AmountGrammsPerHectoLitre,
                AmountGrammsPer1000Litre = additive.AmountGrammsPer1000Litre,
                WineId = additive.Wine.Id
            };
        }


    }
}
