using Flight.Management.System.API.Configuration;
using Flight.Management.System.API.Helpers;
using Flight.Management.System.API.Models.Auth;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace Flight.Management.System.API.Services
{
    public class AuthService
    {
        private JWTConfig _config;
        protected IPasswordHasher<Data.Model.User> Hasher { get; }

        public AuthService(JWTConfig config, IPasswordHasher<Data.Model.User> hasher)
        {
            _config = config;
            Hasher = hasher;
        }
        public bool Login(LoginFormModel login, Data.Model.User user)
        {
            var result = Hasher.VerifyHashedPassword(user, user.Password, login.Password);
            if (result == PasswordVerificationResult.Success)
            {
                return true;
            }

            return false;
        }
        #region CreateToken()
        public UserTokens CreateToken(Data.Model.User user)
        {
            var Token = new UserTokens();

            Token = JWTHelper.GenTokenKey(new UserTokens()
            {
                isAdmin = user.IsAdmin,
                EmailId = user.Email,
                GuidId = user.PublicId,
                UserName = user.UserName,
                Id = user.Id,

            }, _config, user);

            return Token;
        }


        #endregion

    }
}
