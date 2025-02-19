namespace WineApi.Model
{
    public class Additive
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public float AmountGrammsPerLitre { get; set; }

        public AdditiveType AdditiveType { get; set; }

        public Guid AdditiveTypeId { get; set; }
        public Guid WineId { get; set; }
    }
}
