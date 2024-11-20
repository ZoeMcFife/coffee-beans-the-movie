namespace Test.Model
{
    public class Fermentation
    {
        public List<FermentationEntry> Entries { get; set; } // Gärkontrolle data
    }

    public class FermentationEntry
    {
        public DateTime Date { get; set; } // Datum
        public string Time { get; set; } // Uhrzeit
        public float Density { get; set; } // Dichte (OE, KMW)
    }

}
