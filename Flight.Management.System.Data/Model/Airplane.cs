using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Management.System.Data.Model
{
    public class Airplane
    {
        public string PublicId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
    }
}
