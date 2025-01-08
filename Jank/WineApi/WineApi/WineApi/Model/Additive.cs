namespace WineApi.Model
{
    public class Additive
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public float AmountGrammsPerLitre { get; set; }
        public float AmountGrammsPerHectoLitre { get; set; } 
        public float AmountGrammsPer1000Litre { get; set; }
        public int WineId { get; set; }

        public Wine Wine { get; set; }
    }
}
