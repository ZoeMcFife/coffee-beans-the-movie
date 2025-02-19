using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace WineApi.Model
{
    public class WineBarrelHistory
    {
        public Guid Id { get; set; }

        public Guid WineBarrelId { get; set; }
        public Guid WineTypeId { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public WineBarrel WineBarrel { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public WineType WineType { get; set; }

    }
}
