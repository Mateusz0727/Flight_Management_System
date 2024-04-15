using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Management.System.Data.Model
{
    public class City
    {
        public string PublicId { get; set; }
        public int Id { get; set; }
        public string CityName { get; set; }
        public Country Country { get; set; }
    }
}
