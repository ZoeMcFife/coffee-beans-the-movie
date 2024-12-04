namespace WineApi.Model.DTO
{
    public class MostTreatmentDTO
    {
        public int Id { get; set; }

        public bool IsTreated { get; set; } // Mostschönung (Ja/Nein)
        public DateTime? TreatmentDate { get; set; } // Datum der Behandlung

        public static MostTreatmentDTO MapMostTreatmentToDto(MostTreatment mostTreatment)
        {
            if (mostTreatment == null) throw new ArgumentNullException(nameof(mostTreatment));

            return new MostTreatmentDTO
            {
                Id = mostTreatment.Id,
                IsTreated = mostTreatment.IsTreated,
                TreatmentDate = mostTreatment.TreatmentDate
            };
        }

        public static MostTreatment MapDtoToMostTreatment(MostTreatmentDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new MostTreatment
            {
                Id = dto.Id,
                IsTreated = dto.IsTreated,
                TreatmentDate = dto.TreatmentDate,
            };
        }




    }


}
