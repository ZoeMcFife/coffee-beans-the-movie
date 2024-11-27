namespace WineApi.Model
{
    public class Wine
    {
        public User User { get; set; }
        public int UserId { get; set; }

        public int Id { get; set; } // Weinnummer
        public string Name { get; set; } // Bezeichnung
        public float MostWeight { get; set; } // Mostgewicht (Oe oder KMW)
        public DateTime HarvestDate { get; set; } // Lesetag
        public float VolumeInHectoLitre { get; set; } // Menge in Hektolitern
        public string Container { get; set; } // Gebinde
        public string ProductionType { get; set; } // Bio, Konver., Biodyn

        public MostTreatment MostTreatment { get; set; } // Mostschönung details
        public List<FermentationEntry> FermentationEntries { get; set; } // Gärkontrolle details
        public List<Additive> Additives { get; set; } // Zusätze list
    }
}
