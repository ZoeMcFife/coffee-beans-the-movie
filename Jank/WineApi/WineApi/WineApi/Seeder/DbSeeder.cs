using Microsoft.EntityFrameworkCore;
using WineApi.Model;

namespace WineApi.Seeder
{
    public class DbSeeder
    {
        private readonly DbContext _context;

        public DbSeeder(DbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            // Seed Users
            var users = GenerateUsers(10);
            var mostTreatments = GenerateMostTreatments(100);
            var wines = GenerateWines(100, users, mostTreatments);
            var fermentationEntries = GenerateFermentationEntries(300, wines);
            var additives = GenerateAdditives(60, wines);


            _context.Set<User>().AddRange(users);
            _context.SaveChanges();

            // Seed Most Treatments
            _context.Set<MostTreatment>().AddRange(mostTreatments);
            _context.SaveChanges();

            // Seed Wines
            _context.Set<Wine>().AddRange(wines);
            _context.SaveChanges();

            // Seed Fermentation Entries
            _context.Set<FermentationEntry>().AddRange(fermentationEntries);
            _context.SaveChanges();

            // Seed Additives
            _context.Set<Additive>().AddRange(additives);
            _context.SaveChanges();
        }

        private List<User> GenerateUsers(int count)
        {
            var users = new List<User>();
            for (int i = 1; i <= count; i++)
            {
                users.Add(new User
                {
                    Username = $"User{i}",
                    Email = $"user{i}@example.com",
                    Password = "Password123!"
                });
            }
            return users;
        }

        private List<Wine> GenerateWines(int count, List<User> users, List<MostTreatment> mostTreatments)
        {
            var random = new Random();
            var wines = new List<Wine>();
            for (int i = 1; i <= count; i++)
            {
                wines.Add(new Wine
                {
                    Name = $"Wine {i}",
                    MostWeight = (float)(random.NextDouble() * 100),
                    HarvestDate = DateTime.Now.AddDays(-random.Next(1, 365)).ToUniversalTime(),
                    VolumeInHectoLitre = (float)(random.NextDouble() * 100),
                    Container = $"Container {i}",
                    ProductionType = "Bio",
                    UserId = users[random.Next(users.Count)].Id,
                    MostTreatmentId = mostTreatments[i - 1].Id
                });
            }
            return wines;
        }

        private List<MostTreatment> GenerateMostTreatments(int count)
        {
            var random = new Random();
            var treatments = new List<MostTreatment>();
            for (int i = 1; i <= count; i++)
            {
                treatments.Add(new MostTreatment
                {
                    IsTreated = random.Next(0, 2) == 1,
                    TreatmentDate = DateTime.Now.AddDays(-random.Next(1, 365)).ToUniversalTime()
                });
            }
            return treatments;
        }

        private List<FermentationEntry> GenerateFermentationEntries(int count, List<Wine> wines)
        {
            var random = new Random();
            var entries = new List<FermentationEntry>();
            for (int i = 1; i <= count; i++)
            {
                entries.Add(new FermentationEntry
                {
                    Date = DateTime.Now.AddDays(-random.Next(1, 365)).ToUniversalTime(),
                    Density = (float)(random.NextDouble() * 100),
                    WineId = wines[random.Next(wines.Count)].Id
                });
            }
            return entries;
        }

        private List<Additive> GenerateAdditives(int count, List<Wine> wines)
        {
            var random = new Random();
            var additives = new List<Additive>();
            for (int i = 1; i <= count; i++)
            {
                additives.Add(new Additive
                {
                    Type = "Sulfur",
                    Date = DateTime.Now.AddDays(-random.Next(1, 365)).ToUniversalTime(),
                    Time = DateTime.Now.ToString("HH:mm"),
                    AmountGrammsPerLitre = (float)(random.NextDouble() * 10),
                    AmountGrammsPerHectoLitre = (float)(random.NextDouble() * 10),
                    AmountGrammsPer1000Litre = (float)(random.NextDouble() * 10),
                    WineId = wines[random.Next(wines.Count)].Id
                });
            }
            return additives;
        }
    }
}
