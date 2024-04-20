
using AutoMapper;
using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.API.Models.Auth;
using Flight.Management.System.Data.Model;

namespace Flight.Management.System.API.Models
{
    public class AutoMapperInitializator : Profile
    {
        public AutoMapperInitializator()
        {
            FlightModels();
            AirplaneModels();
            UserModels();
            AirplaneTypeModels();
        }

        protected void FlightModels()
        {
           CreateMap<Data.Model.Flight, Flight.FlightModel>().ReverseMap();
        }
        protected void AirplaneModels()
        {
            CreateMap<Data.Model.Airplane, AirplaneModel>().ReverseMap();
        }
        protected void UserModels()
        {
              CreateMap<User, RegisterFormModel>().ReverseMap().ForMember(x => x.UserName, m => m.MapFrom(s => s.SurName));
     
        }
        protected void AirplaneTypeModels()
        {
             CreateMap<Data.Model.AirplaneType, AirplaneTypeFormModel>().ReverseMap();
        }
    }
}
