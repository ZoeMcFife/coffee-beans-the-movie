namespace WineApi.Model
{
    public class FermentationEntry
    {
        public int Id { get; set; }

        public DateTime Date { get; set; } // Datum
        public float Density { get; set; } // Dichte (OE, KMW)

        public int WineId { get; set; }

        public Wine Wine { get; set; }
    }

}
