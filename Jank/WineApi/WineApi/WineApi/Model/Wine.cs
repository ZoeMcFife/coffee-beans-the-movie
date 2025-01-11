namespace WineApi.Model
{
    public class Wine
    {
        public User User { get; set; }
        public int UserId { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public float MostWeight { get; set; }
        public DateTime HarvestDate { get; set; }
        public float VolumeInHectoLitre { get; set; }
        public string Container { get; set; } 
        public string ProductionType { get; set; } 

        public int? MostTreatmentId { get; set; }

        public MostTreatment? MostTreatment { get; set; } 
        public List<FermentationEntry> FermentationEntries { get; set; } 
        public List<Additive> Additives { get; set; } 
    }
}
