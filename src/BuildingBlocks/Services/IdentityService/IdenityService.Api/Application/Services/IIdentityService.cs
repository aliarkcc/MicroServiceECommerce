using IdenityService.Api.Application.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdenityService.Api.Application.Services
{
    public interface IIdentityService
    {
        Task<LoginResponseModel> Login(LoginRequestModel requestModel);
    }

    public class IdentityService : IIdentityService
    {
        public Task<LoginResponseModel> Login(LoginRequestModel requestModel)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier,requestModel.UserName),
                new Claim(ClaimTypes.Name,"Mehmet Ali Arkaç")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MehmetAliMySecretKeyLong"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expire = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(claims: claims, expires: expire, signingCredentials: credentials, notBefore: DateTime.Now);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            LoginResponseModel responseModel = new LoginResponseModel()
            {
                UserName = requestModel.UserName,
                UserToken = encodedJwt
            };

            return Task.FromResult(responseModel);
        }
    }
}
