using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name must be between 3 and 100 characters.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Most Weight is required.")]
        [Range(1, 100, ErrorMessage = "MostWeight must be between 1.0 and 250.0.")]
        public float MostWeight { get; set; }
        
        public DateTime HarvestDate { get; set; }
        
        public float VolumeInLitre { get; set; }
        
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
