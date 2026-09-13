using System.Security.Cryptography;

// Layer: security. Run: dotnet run --project examples/csharp/06-security

var stored = HashPassword("correct horse battery staple");
Console.WriteLine($"Stored format: iterations.salt.hash = {stored}");
Console.WriteLine($"Correct password: {VerifyPassword("correct horse battery staple", stored)}");
Console.WriteLine($"Wrong password: {VerifyPassword("wrong", stored)}");

static string HashPassword(string password)
{
    const int iterations = 120_000;
    var salt = RandomNumberGenerator.GetBytes(16);
    var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, 32);
    return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
}

static bool VerifyPassword(string candidate, string stored)
{
    var parts = stored.Split('.');
    if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations)) return false;
    var salt = Convert.FromBase64String(parts[1]);
    var expected = Convert.FromBase64String(parts[2]);
    var actual = Rfc2898DeriveBytes.Pbkdf2(candidate, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
    return CryptographicOperations.FixedTimeEquals(actual, expected);
}

