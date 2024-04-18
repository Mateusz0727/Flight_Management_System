using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Management.System.Data.Model
{
    public class BaseContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private IConfiguration Configuration { get; }

        private string ConnectionString { get; set; }
        public BaseContext()
        {
        }
         public BaseContext(DbContextOptions<BaseContext> options):base(options)
        {

        }
      /*  public BaseContext(DbContextOptions<BaseContext> options, IConfiguration configuration)
           : base(options)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("DefaultConnectionString");

        }*/
       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(ConnectionString);*/
        public virtual DbSet<Airplane> Airplane { get; set; }
        public virtual DbSet<Flight> Flight { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Flight>().ToTable("Flight");
            modelBuilder.Entity<Airplane>().ToTable("Airplane");
            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<City>().ToTable("City");
            modelBuilder.Entity<Airport>().ToTable("Airport");
            modelBuilder.Entity<User>().ToTable("User");

        }

    }
}
