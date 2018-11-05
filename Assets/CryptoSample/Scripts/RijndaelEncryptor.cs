using UnityEngine;
using System.Security.Cryptography;

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

	private static void GenerateKeyFromPassword(string password, int keySize, int blockSize, out byte[] key, out byte[] iv)
	{
		string pw = ConvertPassword(password); // Do not allow other projects.
		byte[] bSalt = System.Text.Encoding.UTF8.GetBytes(salt);

		Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(pw, bSalt);
		deriveBytes.IterationCount = 1000;

		key = deriveBytes.GetBytes(keySize/8);
		iv = deriveBytes.GetBytes(blockSize/8);
	}

	private static string ConvertPassword(string password)
	{
		string converted = password;

		string productName = Application.productName;
		char[] charArray = productName.ToCharArray();
		int length = charArray.Length;
		int lastIndex = (length % 2 != 0) ? length - 1 : length - 2;
		for (int i = 0; i < length/2; i+=2)
		{
			char temp = charArray[i];
			charArray[i] = charArray[lastIndex - i];
			charArray[lastIndex - i] = temp;
		}
		converted = password + "@" + new string(charArray);

		return converted; 
	}
}
