namespace Test.Model
{
    public class Wine
    {
        public int Id { get; set; } // Weinnummer
        public string Name { get; set; } // Bezeichnung
        public float MostWeight { get; set; } // Mostgewicht (Oe oder KMW)
        public DateTime HarvestDate { get; set; } // Lesetag
        public float VolumeInHl { get; set; } // Menge in Hektolitern
        public string Container { get; set; } // Gebinde
        public string ProductionType { get; set; } // Bio, Konver., Biodyn

        public MostTreatment MostTreatment { get; set; } // Mostschönung details
        public Fermentation Fermentation { get; set; } // Gärkontrolle details
        public List<Additive> Additives { get; set; } // Zusätze list
    }
}
