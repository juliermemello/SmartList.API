using Konscious.Security.Cryptography;
using Microsoft.Extensions.Options;
using SmartList.Application.Interfaces;
using SmartList.Domain.Security;
using System.Security.Cryptography;
using System.Text;

namespace SmartList.Application.Services;

public class PasswordHasherService : IPasswordHasher
{
    private readonly Argon2Settings _settings;

    public PasswordHasherService(IOptions<Argon2Settings> settings)
    {
        _settings = settings.Value;
    }

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(_settings.SaltSize);

        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = _settings.Parallelism,
            MemorySize = _settings.MemorySize,
            Iterations = _settings.Iterations
        };

        var hash = argon2.GetBytes(_settings.HashSize);

        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split('.');

        if (parts.Length != 2) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = _settings.Parallelism,
            MemorySize = _settings.MemorySize,
            Iterations = _settings.Iterations
        };

        return argon2.GetBytes(_settings.HashSize).SequenceEqual(hash);
    }
}
