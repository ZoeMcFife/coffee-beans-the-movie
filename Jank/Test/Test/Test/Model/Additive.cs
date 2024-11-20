namespace Test.Model
{
    public class Additive
    {
        public string Type { get; set; } // Type of additive (e.g., Schwefel, Säure)
        public DateTime Date { get; set; } // Datum
        public string Time { get; set; } // Uhrzeit
        public float Amount { get; set; } // Menge (gr/L, gr/Hekto, gr/1000)
    }
}
