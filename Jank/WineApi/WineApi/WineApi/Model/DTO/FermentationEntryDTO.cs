namespace WineApi.Model.DTO
{
    public class FermentationEntryDTO
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public float Density { get; set; }

        public int WineId { get; set; }


        public static FermentationEntry MapDtoToFermentationEntry(FermentationEntryDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new FermentationEntry
            {
                Id = dto.Id,
                Date = dto.Date,
                Density = dto.Density,
                WineId = dto.WineId
            };
        }

        public static FermentationEntryDTO MapFermentationEntryToDto(FermentationEntry fermentationEntry)
        {
            if (fermentationEntry == null) throw new ArgumentNullException(nameof(fermentationEntry));

            return new FermentationEntryDTO
            {
                Id = fermentationEntry.Id,
                Date = fermentationEntry.Date,
                Density = fermentationEntry.Density,
                WineId = fermentationEntry.WineId
            };
        }


    }
}
