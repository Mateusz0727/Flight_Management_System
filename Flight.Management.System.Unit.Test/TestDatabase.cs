using Flight.Management.System.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Flight.Management.System.Unit.Test
{
    public class TestDatabase
    {
        protected IPasswordHasher<User> Hasher { get; }
        public async Task<BaseContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<BaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new BaseContext(options);
            databaseContext.Database.EnsureCreated();

            // Dodaj 10 miast

            if (databaseContext.Countries.Count() <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    var country = new Country()
                    {
                        Id = i + 1,
                        PublicId = Guid.NewGuid().ToString(),
                        CountryName = "Country " + (i + 1),
                    };
                    databaseContext.Countries.Add(country);
                }
                await databaseContext.SaveChangesAsync(); // Zapisz państwa do bazy danych
            }

            if (databaseContext.Cities.Count() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    var countryId = (i % 5) + 1; // Id państwa od 1 do 5
                    var city = new City()
                    {
                        Id = i + 1,
                        PublicId = Guid.NewGuid().ToString(),
                        CityName = "City " + (i + 1),
                        Country =  databaseContext.Countries.FirstOrDefault(x=>x.Id==countryId), // Przypisanie miasta do odpowiedniego państwa
                    };
                    databaseContext.Cities.Add(city);
                    var airport = new Airport()
                    {
                        Id = i + 1,
                        PublicId = Guid.NewGuid().ToString(),
                        AirportName = "Airport " + (i + 1),
                        City = city, // Przypisanie lotniska do miasta
                    };
                    databaseContext.Airports.Add(airport);
                   
                }
                await databaseContext.SaveChangesAsync(); // Zapisz miasta do bazy danych
            }

            // Zdefiniuj 5 różnych samolotów
            if (databaseContext.Airplane.Count() <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    databaseContext.Airplane.Add(
                        new Airplane()
                        {
                            Id = i + 1,
                            Name = "Airplane " + (i + 1),
                            PublicId = Guid.NewGuid().ToString(),
                        });
                }
                await databaseContext.SaveChangesAsync();
            }

            // Dodaj 10 lotów z różnymi punktami wylotu i przylotu
            if (databaseContext.Flight.Count() <= 0)
            {
                var airports = await databaseContext.Airports.ToListAsync(); // Pobierz wszystkie lotniska z bazy danych

                for (int i = 0; i < 10; i++)
                {
                    var departureAirport = airports[i % airports.Count]; // Pobierz lotnisko na podstawie reszty z dzielenia, aby wybrać losowe lotnisko
                    var arrivalAirport = airports[(i + 1) % airports.Count]; // Pobierz następne lotnisko jako punkt docelowy

                    databaseContext.Flight.Add(
                        new Data.Model.Flight()
                        {
                            Id = i + 1,
                            PublicId = Guid.NewGuid().ToString(),
                            ArrivalPoint = arrivalAirport,
                            DepartureDate = DateTime.Now.AddDays(1),
                            DeparturePoint = departureAirport,
                            Airplane = await databaseContext.Airplane.FirstOrDefaultAsync(a => a.Id == (i % 5) + 1), // Przypisz różny samolot do każdego lotu
                        });
                }
            }
/*
            if (databaseContext.Users.Count() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    var user = new User
                    {
                        Id = i,
                        DateCreatedUtc = DateTime.Now,
                        DateModifiedUtc = DateTime.Now,
                        Surname = "user " + i,
                        Email = $"user{i}@admin.pl",
                        GivenName = "user " + i,
                        IsAdmin = i % 5 == 0 ? true : false,
                        PublicId = Guid.NewGuid().ToString(),
                        UserName = "user " + i
                    };
                    user.Password = Hasher.HashPassword(user, $"user{i}");
                }
            }*/
            await databaseContext.SaveChangesAsync();

            return databaseContext;
        }
    }
}

