namespace WineApi.Model
{
    public class FermentationEntry
    {
        public int Id { get; set; }

        public DateTime Date { get; set; } 
        public float Density { get; set; }

        public int WineId { get; set; }

        public Wine Wine { get; set; }
    }

}
