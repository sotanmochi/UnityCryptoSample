using UnityEngine;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Crypto
{
	public static class RijndaelEncryptor
	{
		private static int keySize = 128;
		private static int blockSize = 128;
		private static string salt = "Salt2018XyZ";

		public static byte[] Encrypt(byte[] srcData, string password)
		{
			RijndaelManaged rijndael = new RijndaelManaged();
			rijndael.KeySize = keySize;
			rijndael.BlockSize = blockSize;

			byte[] key, iv;
			GenerateKeyFromPassword(password, rijndael.KeySize, rijndael.BlockSize, out key, out iv);
			rijndael.Key = key;
			rijndael.IV = iv;

			ICryptoTransform encryptor = rijndael.CreateEncryptor();
			byte[] encryptedData = null;
			try
			{
				encryptedData = encryptor.TransformFinalBlock(srcData, 0, srcData.Length);
			}
			catch(CryptographicException e)
			{
				Debug.Log("Encryption is failed!!");
				Debug.Log(e);
			}
			encryptor.Dispose();

			return encryptedData;
		}

		public static async Task<byte[]> EncryptAsync(byte[] srcData, string password)
		{
			RijndaelManaged rijndael = new RijndaelManaged();
			rijndael.KeySize = keySize;
			rijndael.BlockSize = blockSize;

			byte[] key, iv;
			GenerateKeyFromPassword(password, rijndael.KeySize, rijndael.BlockSize, out key, out iv);
			rijndael.Key = key;
			rijndael.IV = iv;

			ICryptoTransform encryptor = rijndael.CreateEncryptor();
			byte[] encryptedData = null;
			try
			{
				encryptedData = await Task.Run(() => encryptor.TransformFinalBlock(srcData, 0, srcData.Length));
			}
			catch(CryptographicException e)
			{
				Debug.Log("Encryption is failed!!");
				Debug.Log(e);
			}
			encryptor.Dispose();

			return encryptedData;
		}

		private static void GenerateKeyFromPassword(string password, int keySize, int blockSize, out byte[] key, out byte[] iv)
		{
			byte[] bSalt = System.Text.Encoding.UTF8.GetBytes(salt);

			Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, bSalt);
			deriveBytes.IterationCount = 1000;

			key = deriveBytes.GetBytes(keySize/8);
			iv = deriveBytes.GetBytes(blockSize/8);
		}
	}
}
