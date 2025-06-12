using BCrypt.Net;

namespace myProject.Controllers;

public class PasswordService
{
    internal static string HashPassword(object password)
    {
        throw new NotImplementedException();
    }

    // פונקציה להצפנת הסיסמא
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    // פונקציה לבדוק אם הסיסמא שהוזנה תואמת לסיסמא המוצפנת
    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
