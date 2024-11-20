using Microsoft.EntityFrameworkCore;
using WineApi.Model;

namespace WineApi.Context
{
    public class WineDbContext : DbContext
    {
        DbSet<Additive> Additives { get; set; }
        DbSet<FermentationEntry> FermentationEntries { get; set; }
        DbSet<MostTreatment> MostTreatments { get; set; }
        DbSet<Wine> Wines { get; set; }

        public WineDbContext(DbContextOptions<WineDbContext> options) : base(options)
        {

        }
    }
}
