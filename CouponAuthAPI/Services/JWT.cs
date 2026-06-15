using CouponAuthAPI.Model;
using CouponAuthAPI.Services.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CouponAuthAPI.Services
{
    public class JWT : IJWT
    {
        private readonly JWTOption _jwtoption;
        public JWT(IOptions<JWTOption> jwtoption)
        {
            _jwtoption = jwtoption.Value;
        }
        public string GenerateToken(ApplicationUser applicationUser)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtoption.Secret);

            var claimlist = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email,applicationUser.UserEmail),
                new Claim(JwtRegisteredClaimNames.Name,applicationUser.UserName),
                //new Claim(JwtRegisteredClaimNames.Email,applicationUser.UserEmail),
            };

            var tokendescripter = new SecurityTokenDescriptor
            {
                Audience = _jwtoption.Audience,
                Issuer = _jwtoption.Issuer,
                Subject = new ClaimsIdentity(claimlist),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenhandler.CreateToken(tokendescripter);
            return tokenhandler.WriteToken(token);
        }
    }
}
