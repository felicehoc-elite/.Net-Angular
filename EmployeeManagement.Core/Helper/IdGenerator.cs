
namespace EmployeeManagement.Core.Helper;

public class IdGenerator
{
    private static readonly object lockObject = new object();
    private static long lastTimestamp = DateTime.UtcNow.Ticks;

    public static string NewId()
    {
        lock (lockObject)
        {
            long currentTimestamp = DateTime.UtcNow.Ticks;

            if (currentTimestamp <= lastTimestamp)
            {
                currentTimestamp = lastTimestamp + 1;
            }

            lastTimestamp = currentTimestamp;

            long combinedValue = (currentTimestamp << 20) | (GenerateRandomNumber() & 0xFFFFF);
            combinedValue ^= Math.Abs(Guid.NewGuid().GetHashCode());

            string extractedDigits = string.Concat(combinedValue.ToString("D20").Where(char.IsDigit));

            // Ensure the result matches the pattern XXX-XX-XXXX
            return $"{extractedDigits.Substring(0, 3)}-{extractedDigits.Substring(3, 2)}-{extractedDigits.Substring(5, 4)}";
        }
    }

    private static int GenerateRandomNumber()
    {
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            byte[] randomNumber = new byte[4];
            rng.GetBytes(randomNumber);
            return BitConverter.ToInt32(randomNumber, 0);
        }
    }
}