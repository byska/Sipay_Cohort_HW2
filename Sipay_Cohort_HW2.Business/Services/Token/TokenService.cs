using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sipay_Cohort_HW2.DataAccess.Response;
using Sipay_Cohort_HW2.DataAccess.UnitOfWork;
using Sipay_Cohort_HW2.Dtos.Token;
using Sipay_Cohort_HW2.Entities;
using Sipay_Cohort_HW2.JWT;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Sipay_Cohort_HW2.Business.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IUow _uow;
        public TokenService( IOptionsMonitor<JwtConfig> jwtConfig,IUow uow)
        {
            _jwtConfig = jwtConfig.CurrentValue;
            _uow = uow;
        }

        public async Task<ApiResponse<TokenResponse>> Login(TokenRequest request)
        {
            if (request is null)
            {
                return new ApiResponse<TokenResponse>("Request was null");
            }
            if (string.IsNullOrEmpty(request.Email))
            {
                return new ApiResponse<TokenResponse>("Request was null");
            }

            request.Email = request.Email.Trim().ToLower();

           var user = await _uow.GetRepository<User>().GetByDefault(x => x.Email.Equals(request.Email));
            if (user is null)
            {
                return new ApiResponse<TokenResponse>("Invalid user informations");
            }

            string token = Token(user);

            TokenResponse response = new()
            {
                AccessToken = token,
                ExpireTime = DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpiration),
                Email = user.Email
            };

            return new ApiResponse<TokenResponse>(response);
        }
        private string Token(User user)
        {
            Claim[] claims = GetClaims(user);
            var secret = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var jwtToken = new JwtSecurityToken(
               issuer: _jwtConfig.Issuer,
               audience: _jwtConfig.Audience,
               claims: claims,
               expires: DateTime.Now.AddMinutes(_jwtConfig.AccessTokenExpiration),
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return accessToken;
        }


        private Claim[] GetClaims(User user)
        {
            var claims = new[]
            {
            new Claim("Email",user.Email),
            new Claim("Id",user.Id.ToString()),
            new Claim("Role", "User"),
            new Claim(ClaimTypes.Role,"User")
        };

            return claims;
        }
    }
}
