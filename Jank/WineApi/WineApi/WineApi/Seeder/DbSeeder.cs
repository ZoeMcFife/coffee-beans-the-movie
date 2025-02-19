using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

            var wineTypes = GenerateWineTypes(15);
            _context.Set<WineType>().AddRange(wineTypes);
            _context.SaveChanges();

            wineTypes = _context.Set<WineType>().ToList();

            var mostTreatments = GenerateMostTreatments(100);
            _context.Set<MostTreatment>().AddRange(mostTreatments);
            _context.SaveChanges();

            mostTreatments = _context.Set<MostTreatment>().ToList();

            var (wineBarrels, barrelCurrentHistories) = GenerateWineBarrels(100, users, mostTreatments, wineTypes);
            _context.Set<WineBarrel>().AddRange(wineBarrels);
            _context.SaveChanges();

            _context.Set<WineBarrelHistory>().AddRange(barrelCurrentHistories);
            _context.SaveChanges();

            wineBarrels = _context.Set<WineBarrel>().ToList();

            foreach (var wine in wineBarrels)
            {
                GenerateWineBarrelHistory(wine, wineTypes);
            }

            var fermentationEntries = GenerateFermentationEntries(300, wineBarrels);
            _context.Set<FermentationEntry>().AddRange(fermentationEntries);
            _context.SaveChanges();

            fermentationEntries = _context.Set<FermentationEntry>().ToList();

            var addityTypes = GenerateAdditiveTypes(10);
            _context.Set<AdditiveType>().AddRange(addityTypes);
            _context.SaveChanges();

            var additives = GenerateAdditives(60, wineBarrels, addityTypes);
            _context.Set<Additive>().AddRange(additives);
            _context.SaveChanges();

            additives = _context.Set<Additive>().ToList();


            var admin = new User
            {
                Username = "Buchi",
                Password = HashPassword("admin"),
                Email = "admin",
                AdminRights = true
            };

            _context.Users.Add(admin);
            _context.SaveChanges();

        }


        private List<WineType> GenerateWineTypes(int count)
        {
            var wineNames = new List<string>
            {
                "Cabernet Sauvignon",
                "Merlot",
                "Pinot Noir",
                "Syrah",
                "Zinfandel",
                "Chardonnay",
                "Sauvignon Blanc",
                "Riesling",
                "Malbec",
                "Grenache",
                "Tempranillo",
                "Sangiovese",
                "Barbera",
                "Moscato",
                "Viognier"
            };

            var wineTypes = new List<WineType>();
            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                string randomWine = wineNames[random.Next(wineNames.Count)];
                wineTypes.Add(new WineType
                {
                    Id = Guid.NewGuid(),
                    Name = randomWine
                });
            }

            return wineTypes;
        }

        private void GenerateWineBarrelHistory(WineBarrel barrel, List<WineType> wineTypes)
        {
            var random = new Random();

            var history_count = random.Next(15);

            List<WineBarrelHistory> histories = new List<WineBarrelHistory>();

            for (int i = 0; i < history_count; i++)
            {
                var s = DateTime.Now.AddDays(-random.Next(100, 665)).ToUniversalTime();
                var e = s.AddDays(random.Next(1, 100));

                var history = new WineBarrelHistory
                {
                    Id = Guid.NewGuid(),
                    WineBarrelId = barrel.Id,
                    WineTypeId = wineTypes[random.Next(wineTypes.Count - 1)].Id,
                    StartDate = s,
                    EndDate = e,
                };

                histories.Add(history);
            }

            _context.Set<WineBarrelHistory>().AddRange(histories);
            _context.SaveChanges();
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

        private (List<WineBarrel>, List<WineBarrelHistory>) GenerateWineBarrels(int count, List<User> users, List<MostTreatment> mostTreatments, List<WineType> wineTypes)
        {
            var random = new Random();
            var wines = new List<WineBarrel>();
            var histories = new List<WineBarrelHistory>();

            for (int i = 1; i <= count; i++)
            {
                var barrel = new WineBarrel
                {
                    Id = Guid.NewGuid(),
                    Name = $"Wine Barrel {i}",
                    MostWeight = (float)(random.NextDouble() * 100),
                    HarvestDate = DateTime.Now.AddDays(-random.Next(1, 365)).ToUniversalTime(),
                    VolumeInLitre = (float)(random.NextDouble() * 100),
                    Container = $"Container {i}",
                    ProductionType = "Bio",
                    UserId = users[random.Next(users.Count - 1)].Id,
                    MostTreatmentId = mostTreatments[random.Next(mostTreatments.Count)].Id
                };

                wines.Add(barrel);

                if (i % 5 != 0)
                {
                    var history = new WineBarrelHistory
                    {
                        Id = Guid.NewGuid(),
                        WineBarrelId = barrel.Id,
                        WineTypeId = wineTypes[random.Next(wineTypes.Count - 1)].Id,
                        StartDate = DateTime.Now.AddDays(-random.Next(1, 10)).ToUniversalTime(),
                    };

                    histories.Add(history);
                }
            }
            return (wines, histories);
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

        private List<FermentationEntry> GenerateFermentationEntries(int count, List<WineBarrel> wines)
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

        private List<Additive> GenerateAdditives(int count, List<WineBarrel> wines, List<AdditiveType> additiveTypes)
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
