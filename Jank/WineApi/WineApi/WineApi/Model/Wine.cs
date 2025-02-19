using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace WineApi.Model
{
    public class Wine
    {
        [JsonIgnore]
        [XmlIgnore]
        public User User { get; set; }

        public Guid UserId { get; set; }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public float MostWeight { get; set; }
        public DateTime HarvestDate { get; set; }
        public float VolumeInHectoLitre { get; set; }
        public string Container { get; set; } 
        public string ProductionType { get; set; } 

        public Guid? MostTreatmentId { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public MostTreatment? MostTreatment { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public List<FermentationEntry> FermentationEntries { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public List<Additive> Additives { get; set; } 
    }
}
