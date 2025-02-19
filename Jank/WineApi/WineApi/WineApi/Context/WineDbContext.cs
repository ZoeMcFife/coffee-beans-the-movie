using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WineApi.Model;

[assembly: InternalsVisibleTo("WeinManagementUnitTests")]

namespace WineApi.Context
{
    public class WineDbContext : DbContext
    {
        public DbSet<Additive> Additives { get; set; }
        public DbSet<FermentationEntry> FermentationEntries { get; set; }
        public DbSet<MostTreatment> MostTreatments { get; set; }
        public DbSet<AdditiveType> AdditiveTypes { get; set; }
        public DbSet<Wine> Wines { get; set; }
        public DbSet<User> Users { get; set; }

        public WineDbContext(DbContextOptions<WineDbContext> options) : base(options)
        {

        }
    }
}
