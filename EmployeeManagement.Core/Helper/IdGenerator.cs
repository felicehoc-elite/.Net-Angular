
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

            currentTimestamp = Math.Max(currentTimestamp, lastTimestamp + 1);
            lastTimestamp = currentTimestamp;

            long combinedValue = (currentTimestamp << 20) | (GenerateRandomNumber() & 0xFFFFF);

            combinedValue ^= (long)Guid.NewGuid().GetHashCode() << 32 | (uint)Guid.NewGuid().GetHashCode();

            string formattedId = combinedValue.ToString("D20");
            string extractedDigits = new string(formattedId.Where(char.IsDigit).ToArray());

            // Ensure the result matches the pattern XXX-XX-XXXX
            return $"{extractedDigits.Substring(0, 3)}-{extractedDigits.Substring(3, 2)}-{extractedDigits.Substring(5, 4)}";
        }
    }


    private static int GenerateRandomNumber()
    {
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            byte[] randomNumber = new byte[sizeof(int)];
            rng.GetBytes(randomNumber);
            return BitConverter.ToInt32(randomNumber, 0);
        }
    }
}