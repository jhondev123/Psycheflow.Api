using Psycheflow.Api.Interfaces.Services;
using System.Security.Cryptography;

public class PasswordGeneratorService : IPasswordGeneratorService
{
    private const int PasswordLength = 8;
    private const string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowerCase = "abcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string SpecialChars = "!@#$%^&*()-_=+<>?";

    public string GeneratePassword()
    {
        RandomNumberGenerator random = RandomNumberGenerator.Create();

        List<char> passwordChars = new List<char>
        {
            GetRandomChar(UpperCase, random),
            GetRandomChar(LowerCase, random),
            GetRandomChar(Digits, random),
            GetRandomChar(SpecialChars, random)
        };

        string allChars = UpperCase + LowerCase + Digits + SpecialChars;

        while (passwordChars.Count < PasswordLength)
        {
            passwordChars.Add(GetRandomChar(allChars, random));
        }

        Shuffle(passwordChars, random);

        return new string(passwordChars.ToArray());
    }

    private char GetRandomChar(string source, RandomNumberGenerator rng)
    {
        byte[] randomNumber = new byte[4];
        rng.GetBytes(randomNumber);
        int index = (int)(BitConverter.ToUInt32(randomNumber, 0) % source.Length);
        return source[index];
    }

    private void Shuffle(List<char> list, RandomNumberGenerator rng)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            int j = (int)(BitConverter.ToUInt32(randomNumber, 0) % (i + 1));

            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
