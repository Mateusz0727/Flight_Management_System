using AutoMapper;
using Flight.Management.System.API.Models.Auth;
using Flight.Management.System.Data.Model;
using Microsoft.AspNetCore.Identity;

namespace Flight.Management.System.API.Services.User
{
    public class UserService : BaseService
    {
        protected IPasswordHasher<Data.Model.User> Hasher { get; }
        public UserService(IMapper mapper, IPasswordHasher<Data.Model.User> hasher, BaseContext context) : base(mapper, context)
        {

            Hasher = hasher;
        }

        public async Task<Data.Model.User> GetByEmailAsync(string email)
        {
            Data.Model.User entity = Context.Users.FirstOrDefault(x => x.Email == email);
            return entity;
        }
        public async Task<Data.Model.User> CreateAsync(RegisterFormModel user)
        {
            var entity = Mapper.Map<Data.Model.User>(user);
            try
            {
                SetPassword(entity, user.Password);
                SetEntity(entity);
                Context.Add(entity);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.Message.Contains("IX_Email"))
                        throw new Exception("The email address provided is taken");
                    if (ex.InnerException.Message.Contains("IX_UserName"))
                        throw new Exception("The email address provided is taken");
                }
            }
            return entity;

        }
        #region SetPassword()
        public bool SetPassword(Data.Model.User user, string password)
        {
            if (user != null)
            {
                user.Password = Hasher.HashPassword(user, password);
                return true;
            }

            return false;
        }
        #endregion
        public async Task SetEntity(Data.Model.User user)
        {
            if (user != null)
            {
                user.PublicId = Guid.NewGuid().ToString();
                user.UserName = user.Email;
                user.DateCreatedUtc = DateTime.Now;
                user.DateModifiedUtc = DateTime.Now;

            }

        }
    }

}
