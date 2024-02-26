using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace BookLibraryApi.Models;

public class JwtService
{
    private readonly string _secretKey;

    public static JwtService Service;

    public JwtService(string secretKey)
    {
        _secretKey = secretKey;
        Service = this;
    }

    public string UserToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Expires = DateTime.Now.AddHours(1)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public IEnumerable<Claim> ReadToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(token);

        foreach (var claim in decodedToken.Claims)
        {
            yield return claim;
        }
    }

    public bool IsTokenValid(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(_secretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, // Не проверять издателя токена
            ValidateAudience = false, // Не проверять аудиторию токена
            ValidateLifetime = true, // Проверить срок действия токена
            ClockSkew = TimeSpan.Zero // Не допускать временное смещение при проверке срока действия токена
        };

        try
        {
            SecurityToken validatedToken;
            tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            return true; // Токен валиден
        }
        catch (SecurityTokenException)
        {
            return false; // Токен невалиден
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception thrown during JWT token validation: " + e.Message);
            return false;
        }
    }

    public int GetUserIdByToken(string token)
    {
        var claims = JwtService.Service.ReadToken(token);

        var neededClaim = claims.FirstOrDefault(c => c.Type == "nameid");
        
        return int.Parse(neededClaim.Value);
    }
}