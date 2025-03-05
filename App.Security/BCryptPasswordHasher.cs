namespace App.Security;
using App.Models;
using Microsoft.AspNetCore.Identity;

public class BCryptPasswordHasher : IPasswordHasher<User>
{

    public string HashPassword(User user, string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword)
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
    }
}
