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

            var users = GenerateUsers(10);
            _context.Set<User>().AddRange(users);
            _context.SaveChanges();

            users = _context.Set<User>().ToList();

            var mostTreatments = GenerateMostTreatments(100);
            _context.Set<MostTreatment>().AddRange(mostTreatments);
            _context.SaveChanges();

            mostTreatments = _context.Set<MostTreatment>().ToList();

            var wines = GenerateWines(100, users, mostTreatments);
            _context.Set<Wine>().AddRange(wines);
            _context.SaveChanges();

            wines = _context.Set<Wine>().ToList();

            var fermentationEntries = GenerateFermentationEntries(300, wines);
            _context.Set<FermentationEntry>().AddRange(fermentationEntries);
            _context.SaveChanges();

            fermentationEntries = _context.Set<FermentationEntry>().ToList();

            var addityTypes = GenerateAdditiveTypes(10);
            _context.Set<AdditiveType>().AddRange(addityTypes);
            _context.SaveChanges();

            var additives = GenerateAdditives(60, wines, addityTypes);
            _context.Set<Additive>().AddRange(additives);
            _context.SaveChanges();

            additives = _context.Set<Additive>().ToList();

        }


        private List<AdditiveType> GenerateAdditiveTypes(int count)
        {
            var additiveNames = new List<string>
            {
                "Sulfur Dioxide",
                "Tartaric Acid",
                "Citric Acid",
                "Potassium Sorbate",
                "Ascorbic Acid",
                "Gum Arabic",
                "Bentonite Clay",
                "Oak Extract",
                "Mega Purple",
                "Copper Sulfate",
                "Calcium Carbonate",
                "Egg Whites",
                "Gelatin",
                "Isinglass",
                "Casein"
            };

            var additives = new List<AdditiveType>();
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                string randomType = additiveNames[random.Next(additiveNames.Count)];
                additives.Add(new AdditiveType
                {
                    Id = Guid.NewGuid(),
                    Type = randomType
                });
            }

            return additives;
        }

        private List<User> GenerateUsers(int count)
        {
            var users = new List<User>();

            for (int i = 1; i <= count; i++)
            {
                var hashedPassword = HashPassword("password123");

                var newUser = new User
                {
                    Username = "testUser" + i,
                    Password = hashedPassword,
                    Email = "testUser" + i + "@gmail.com"
                };

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
                    MostTreatmentId = mostTreatments[random.Next(mostTreatments.Count)].Id
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
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
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

        private List<Additive> GenerateAdditives(int count, List<Wine> wines, List<AdditiveType> additiveTypes)
        {
            var random = new Random();
            var additives = new List<Additive>();
            for (int i = 1; i <= count; i++)
            {
                additives.Add(new Additive
                {
                    AdditiveTypeId = additiveTypes[random.Next(additiveTypes.Count)].Id,
                    Date = DateTime.Now.AddDays(-random.Next(1, 365)).ToUniversalTime(),
                    AmountGrammsPerLitre = (float)(random.NextDouble() * 10),
                    WineId = wines[random.Next(wines.Count)].Id
                });
            }
            return additives;
        }
    }
}
