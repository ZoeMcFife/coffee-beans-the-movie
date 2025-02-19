namespace WineApi.Model
{
    public class FermentationEntry
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; } 
        public float Density { get; set; }

        public Guid WineId { get; set; }
    }

}
