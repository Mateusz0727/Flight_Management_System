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

         if(databaseContext.AirplaneType.Count() <= 0)
            {
                for(int i =0; i < 5; i++)
                {
                    databaseContext.Add(new AirplaneType()
                    {
                        Id = i + 1,
                        PublicId = Guid.NewGuid().ToString(),
                        Name = "Boeing" + (i + 1),
                        SymbolInRegistrationNumber = (char)('A' + i),
                        NumberOfSeats =(uint) i * 100
                    }); ;
                    

                }
                await databaseContext.SaveChangesAsync();
            }
            if (databaseContext.Airplane.Count() <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    var type = databaseContext.AirplaneType.Where(x => x.Id == i+1).FirstOrDefault();
                    databaseContext.Airplane.Add(
                        new Airplane()
                        {
                            Id = i + 1,
                            RegistrationNumber = "SP-L" + type.SymbolInRegistrationNumber.ToString() + (char)('A' + i),
                            AirplaneType= type,
                            PublicId = Guid.NewGuid().ToString(),
                        }); ;
                }
                await databaseContext.SaveChangesAsync();
            }

          
            if (databaseContext.Flight.Count() <= 0)
            {
                var airports = await databaseContext.Airports.ToListAsync();
                Random random = new Random();
                for (int i = 0; i < 10; i++)
                {
                    var departureAirport = airports[i % airports.Count];
                    var arrivalAirport = airports[(i + 1) % airports.Count]; 

                    databaseContext.Flight.Add(
                        new Data.Model.Flight()
                        {
                            FlightNumber =""+ (char)('A' + i)+ (char)('A' + i)+random.Next(1,999),
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

            if (databaseContext.Users.Count()<=0)
            {
                for(int i=0;i<5;i++)
                {
                    databaseContext.Users.Add(new User() { Id = i+1,
                    DateCreatedUtc= DateTime.UtcNow,
                    DateModifiedUtc = DateTime.UtcNow,
                    Email = i+1+"@"+ i + 1,
                    GivenName = i.ToString(),
                    Surname=i.ToString(),
                    Password= "AQAAAAEAACcQAAAAEHSJ59ER9AhWN2FhmHWbxq8d+6HJmhypf0fcEljyrky/ZnZkjJ28vwpMbeFryW2tzA==",
                    UserName = i + 1 + "@"+ i + 1,
                    PublicId= Guid.NewGuid().ToString()
                    });
                }
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }
    }
}

