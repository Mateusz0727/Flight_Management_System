
using AutoMapper;
using Flight.Management.System.API.Models.Airplane;
using Flight.Management.System.Data.Model;

namespace Flight.Management.System.API.Models
{
    public class AutoMapperInitializator : Profile
    {
        public AutoMapperInitializator()
        {
            FlightModels();
            AirplaneModels();
        }

        protected void FlightModels()
        {
          //  CreateMap<User, RegisterFormModel>().ReverseMap().ForMember(x => x.UserName, m => m.MapFrom(s => s.SurName));
            CreateMap<Data.Model.Flight, Flight.FlightModel>().ReverseMap();
        }
        protected void AirplaneModels()
        {
           /* CreateMap<Data.Model.Airplane, AirplaneModel>().ReverseMap().ForMember(x => x.Name, m => m.MapFrom(s => s.Name)).ReverseMap();*/
            CreateMap<Data.Model.Airplane, AirplaneModel>().ReverseMap();
        }
    }
}
