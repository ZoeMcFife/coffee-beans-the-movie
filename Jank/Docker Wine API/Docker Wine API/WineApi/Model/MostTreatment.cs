using WineApi.Model.DTO;

namespace WineApi.Model
{
    public class MostTreatment
    {
        public int Id { get; set; }

        public bool IsTreated { get; set; }
        public DateTime? TreatmentDate { get; set; }
    }
}
