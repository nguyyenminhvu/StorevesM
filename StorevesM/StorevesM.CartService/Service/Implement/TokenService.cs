using StorevesM.CartService.Service.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace StorevesM.CartService.Service.Implement
{
    public class TokenService : ITokenService
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public TokenService()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }
        public string GetId(string token)
        {
            var tokenClaims = _tokenHandler.ReadJwtToken(token.Split(" ")[1]);
            return tokenClaims.Claims.FirstOrDefault(x=>x.Type=="Id")?.Value!;
        }
    }
}
