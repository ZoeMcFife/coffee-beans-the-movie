using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WineApi.Context;
using WineApi.Model;
using WineApi.Model.DTO;

namespace WineApi.Seeder
{
    public class DbSeeder
    {
        private readonly WineDbContext _context;
        private readonly IConfiguration _configuration;

        public DbSeeder(WineDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void Seed()
        {   
            var usersAmount = _context.Users.Count();

            Console.WriteLine(usersAmount);

            if (usersAmount > 0)
            {
                return;
            }

            // Seed Users
            var users = GenerateUsers(10);
            _context.Set<User>().AddRange(users);
            _context.SaveChanges(); // Save to generate IDs

            // Replace users list with seeded records from the database
            users = _context.Set<User>().ToList();

            // Seed Most Treatments
            var mostTreatments = GenerateMostTreatments(100);
            _context.Set<MostTreatment>().AddRange(mostTreatments);
            _context.SaveChanges(); // Save to generate IDs

            // Replace mostTreatments list with seeded records from the database
            mostTreatments = _context.Set<MostTreatment>().ToList();

            // Seed Wines
            var wines = GenerateWines(100, users, mostTreatments);
            _context.Set<Wine>().AddRange(wines);
            _context.SaveChanges(); // Save to generate IDs

            // Replace wines list with seeded records from the database
            wines = _context.Set<Wine>().ToList();

            // Seed Fermentation Entries
            var fermentationEntries = GenerateFermentationEntries(300, wines);
            _context.Set<FermentationEntry>().AddRange(fermentationEntries);
            _context.SaveChanges(); // Save to generate IDs

            // Replace fermentationEntries list with seeded records from the database
            fermentationEntries = _context.Set<FermentationEntry>().ToList();

            // Seed Additives
            var additives = GenerateAdditives(60, wines);
            _context.Set<Additive>().AddRange(additives);
            _context.SaveChanges(); // Save to generate IDs

            // Replace additives list with seeded records from the database
            additives = _context.Set<Additive>().ToList();

        }

        private List<User> GenerateUsers(int count)
        {
            var users = new List<User>();

            for (int i = 1; i <= count; i++)
            {
                // Hash the password
                var hashedPassword = HashPassword("password123");

                // Create a new User
                var newUser = new User
                {
                    Username = "testUser" + i,
                    Password = hashedPassword,
                    Email = "testUser" + i + "@gmail.com"
                };

                // Save to database
                users.Add(newUser);
                
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
                    UserId = users[random.Next(users.Count - 1)].Id,
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

        private string HashPassword(string password)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Use a key from configuration
            using var hmac = new HMACSHA256(key);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
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
