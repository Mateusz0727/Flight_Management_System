
using AutoMapper;
using Flight.Management.System.API.Extensions;
using Flight.Management.System.API.Models;
using Flight.Management.System.Data.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
try
{
    builder.Services.AddDbContext<BaseContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"))
            );
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
// Add services to the container.
#region AutoMapper
var mappingCongig = new MapperConfiguration(mc =>
     mc.AddProfile(
 new AutoMapperInitializator()
 )
);
IMapper mapper = mappingCongig.CreateMapper();
builder.Services.AddSingleton(mapper);


#endregion
builder.Services.AddControllers();
builder.Services.RegisterServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
