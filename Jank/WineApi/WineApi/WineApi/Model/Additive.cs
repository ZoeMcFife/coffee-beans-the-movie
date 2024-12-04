namespace WineApi.Model
{
    public class Additive
    {
        public int Id { get; set; }
        public string Type { get; set; } // Type of additive (e.g., Schwefel, Säure)
        public DateTime Date { get; set; } // Datum
        public string Time { get; set; } // Uhrzeit
        public float AmountGrammsPerLitre { get; set; } // Menge (gr/L, gr/Hekto, gr/1000)
        public float AmountGrammsPerHectoLitre { get; set; } // Menge (gr/L, gr/Hekto, gr/1000)
        public float AmountGrammsPer1000Litre { get; set; } // Menge (gr/L, gr/Hekto, gr/1000)
        public int WineId { get; set; }

        public Wine Wine { get; set; }
    }
}
