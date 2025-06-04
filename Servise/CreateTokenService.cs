using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using myProject.Interfaces;



namespace myProject.Services
{
    public class CreateTokenService
    {
        //ECDsa ecdsa = ECDsa.Create();
        private static SymmetricSecurityKey key = new SymmetricSecurityKey(
             Encoding.UTF8.GetBytes("hihowareyougili&&&batihere"));
        private static string issuer = "https://poh.education.gov";


        public static SecurityToken GetToken(List<Claim> claims) =>
        new JwtSecurityToken(
         issuer,
         issuer,
         claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512)
        );
    

    public static TokenValidationParameters
        GetTokenValidationParameters() =>
        new TokenValidationParameters
        {
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };
    public static string WriteToken(SecurityToken token) =>
            new JwtSecurityTokenHandler().WriteToken(token);        
}



// List<Claim> claims = new List<Claim>
// {
//     new Claim(ClaimTypes.Name, "username"),
//     new Claim(ClaimTypes.Role, "admin")
// };

// SecurityToken token = LoginService.GetToken(claims);
// string tokenString = LoginService.WriteToken(token);
// Console.WriteLine(tokenString);

}

