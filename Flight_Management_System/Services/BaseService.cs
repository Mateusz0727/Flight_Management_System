using AutoMapper;
using Flight.Management.System.Data.Model;

namespace Flight.Management.System.API.Services
{
    public class BaseService
    {
        protected BaseContext Context { get; }
        protected IMapper Mapper { get; }

        public BaseService(IMapper mapper, BaseContext context)
        {
            Mapper = mapper;
            Context = context;

        }
    }
}
