using WineApi.Model.DTO;

namespace WineApi.Model
{
    public class MostTreatment
    {
        public int Id { get; set; }
        public int WineId { get; set; }

        public bool IsTreated { get; set; } // Mostschönung (Ja/Nein)
        public DateTime? TreatmentDate { get; set; } // Datum der Behandlung

        public Wine Wine { get; set; }

        public MostTreatment MapDtoToMostTreatment(MostTreatmentDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new MostTreatment
            {
                Id = dto.Id,
                WineId = dto.WineId,
                IsTreated = dto.IsTreated,
                TreatmentDate = dto.TreatmentDate,
                Wine = null // Set to null here; populate it later if needed
            };
        }

        public MostTreatmentDTO MapMostTreatmentToDto(MostTreatment mostTreatment)
        {
            if (mostTreatment == null) throw new ArgumentNullException(nameof(mostTreatment));

            return new MostTreatmentDTO
            {
                Id = mostTreatment.Id,
                WineId = mostTreatment.WineId,
                IsTreated = mostTreatment.IsTreated,
                TreatmentDate = mostTreatment.TreatmentDate
            };
        }


    }


}
