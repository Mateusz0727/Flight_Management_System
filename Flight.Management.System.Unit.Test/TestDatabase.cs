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
                await databaseContext.SaveChangesAsync(); 
            }

            if (databaseContext.Cities.Count() <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    var countryId = (i % 5) + 1; 
                    var city = new City()
                    {
                        Id = i + 1,
                        PublicId = Guid.NewGuid().ToString(),
                        CityName = "City " + (i + 1),
                        Country =  databaseContext.Countries.FirstOrDefault(x=>x.Id==countryId),
                    };
                    databaseContext.Cities.Add(city);
                    var airport = new Airport()
                    {
                        Id = i + 1,
                        PublicId = Guid.NewGuid().ToString(),
                        AirportName = "Airport " + (i + 1),
                        City = city, 
                    };
                    databaseContext.Airports.Add(airport);
                   
                }
                await databaseContext.SaveChangesAsync(); 
            }

         
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

          
            if (databaseContext.Flight.Count() <= 0)
            {
                var airports = await databaseContext.Airports.ToListAsync(); 

                for (int i = 0; i < 10; i++)
                {
                    var departureAirport = airports[i % airports.Count];
                    var arrivalAirport = airports[(i + 1) % airports.Count]; 

                    databaseContext.Flight.Add(
                        new Data.Model.Flight()
                        {
                            Id = i + 1,
                            PublicId = Guid.NewGuid().ToString(),
                            ArrivalPoint = arrivalAirport,
                            DepartureDate = DateTime.Now.AddDays(1),
                            DeparturePoint = departureAirport,
                            Airplane = await databaseContext.Airplane.FirstOrDefaultAsync(a => a.Id == (i % 5) + 1), 
                        });
                }
            }

            await databaseContext.SaveChangesAsync();

            return databaseContext;
        }
    }
}

