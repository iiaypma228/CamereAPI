using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Camera.RequestModel;

public class AuthOptions
{
    public const string ISSUER = "Camera.API";// издатель токена
    public const string AUDIENCE = "Camera";// потребитель токена
    const string KEY = "SmallMediumBusiness SmallMediumBusiness";// ключ для шифрации
    public const int LIFETIME = 60 * 24;// время жизни токена в минутах

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
public class Auth
{
    public string? Email;
    public string? Password;
}