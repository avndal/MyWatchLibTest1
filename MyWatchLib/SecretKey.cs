using System.Security.Cryptography;
public class SecretKey 
{



    public static string GenerateSecretKey()
    {
        using (var hmac = new HMACSHA256())
        {
            byte[] keyBytes = hmac.Key;
            return Convert.ToBase64String(keyBytes);
        }
    }

}
