using WineApi.Model.DTO;

namespace WineApi.Model
{
    public class MostTreatment
    {
        public int Id { get; set; }

        public bool IsTreated { get; set; } // Mostschönung (Ja/Nein)
        public DateTime? TreatmentDate { get; set; } // Datum der Behandlung
    }


}
