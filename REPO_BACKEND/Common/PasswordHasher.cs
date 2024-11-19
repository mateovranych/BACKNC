using System.Security.Cryptography;
using System.Text;

namespace backnc.Common
{
	public class PasswordHasher
	{
		private const int SaltSize = 16;
		private const int HashSize = 32;

		public static string HashPassword(string password)
		{
			// Usar RandomNumberGenerator.Create() en lugar de RNGCryptoServiceProvider
			using (var rng = RandomNumberGenerator.Create())
			{
				byte[] salt = new byte[SaltSize];
				rng.GetBytes(salt);

				using (var sha256 = SHA256.Create())
				{
					byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
					byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];
					salt.CopyTo(saltedPassword, 0);
					passwordBytes.CopyTo(saltedPassword, salt.Length);

					byte[] hashBytes = sha256.ComputeHash(saltedPassword);

					byte[] hashWithSalt = new byte[salt.Length + hashBytes.Length];
					salt.CopyTo(hashWithSalt, 0);
					hashBytes.CopyTo(hashWithSalt, salt.Length);

					return Convert.ToBase64String(hashWithSalt);
				}
			}
		}

		public static bool VerifyPassword(string password, string storedHash)
		{
			byte[] hashWithSaltBytes = Convert.FromBase64String(storedHash);

			byte[] salt = new byte[SaltSize];
			Array.Copy(hashWithSaltBytes, 0, salt, 0, SaltSize);

			byte[] storedHashBytes = new byte[HashSize];
			Array.Copy(hashWithSaltBytes, SaltSize, storedHashBytes, 0, HashSize);

			using (var sha256 = SHA256.Create())
			{
				byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
				byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];
				salt.CopyTo(saltedPassword, 0);
				passwordBytes.CopyTo(saltedPassword, salt.Length);

				byte[] hashBytes = sha256.ComputeHash(saltedPassword);

				return hashBytes.SequenceEqual(storedHashBytes);
			}
		}
	}
}
