namespace WineApi.Model.DTO
{
    public class WineDTO
    {
        public int Id { get; set; } 

        public int UserId { get; set; }

        public string Name { get; set; } 
        public float MostWeight { get; set; }
        public DateTime HarvestDate { get; set; } 
        public float VolumeInHectoLitre { get; set; }
        public string Container { get; set; }
        public string ProductionType { get; set; } 

        public int MostTreatmentId { get; set; }

        public static Wine MapDtoToWine(WineDTO wineDto)
        {
            if (wineDto == null) throw new ArgumentNullException(nameof(wineDto));

            return new Wine
            {
                Id = wineDto.Id,
                UserId = wineDto.UserId,
                Name = wineDto.Name,
                MostWeight = wineDto.MostWeight,
                HarvestDate = wineDto.HarvestDate,
                VolumeInHectoLitre = wineDto.VolumeInHectoLitre,
                Container = wineDto.Container,
                ProductionType = wineDto.ProductionType,
                MostTreatment = new MostTreatment { Id = wineDto.MostTreatmentId },
                FermentationEntries = new List<FermentationEntry>(),
                Additives = new List<Additive>()
            };
        }

        public static WineDTO MapWineToDto(Wine wine)
        {
            if (wine == null) throw new ArgumentNullException(nameof(wine));

            return new WineDTO
            {
                Id = wine.Id,
                UserId = wine.UserId,
                Name = wine.Name,
                MostWeight = wine.MostWeight,
                HarvestDate = wine.HarvestDate,
                VolumeInHectoLitre = wine.VolumeInHectoLitre,
                Container = wine.Container,
                ProductionType = wine.ProductionType,
            };
        }


    }
}
