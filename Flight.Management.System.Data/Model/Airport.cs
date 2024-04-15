using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Management.System.Data.Model
{
    public class Airport
    {
        public string PublicId { get; set; }
        public int Id { get; set; }
        public string AirportName { get; set; }
        public City City { get; set; }
    }
}
