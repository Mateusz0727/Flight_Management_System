namespace Flight.Management.System.Data.Model
{
    public class Flight
    {
        public string PublicId { get; set; }
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureDate { get; set; }
        public Airport DeparturePoint { get; set; }
        public Airport ArrivalPoint { get; set; }
        public Airplane Airplane { get; set; }
    }
}
