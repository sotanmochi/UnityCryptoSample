using UnityEngine;
using System.Security.Cryptography;

public static class RijndaelDecryptor
{
	private static int keySize = 128;
	private static int blockSize = 128;
	private static string salt = "Salt2018XyZ";

	public static byte[] Decrypt(byte[] encryptedData, string password)
	{
		RijndaelManaged rijndael = new RijndaelManaged();
		rijndael.KeySize = keySize;
		rijndael.BlockSize = blockSize;

		byte[] key, iv;
		GenerateKeyFromPassword(password, rijndael.KeySize, rijndael.BlockSize, out key, out iv);
		rijndael.Key = key;
		rijndael.IV = iv;

		ICryptoTransform decryptor = rijndael.CreateDecryptor();
		byte[] decryptedData = null;
		try
		{
			decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
		}
		catch(CryptographicException e)
		{
			Debug.LogError("Decryption is failed!! Decryption password is incorrect, or encrypted asset is not allowed in this project.");
			Debug.LogError(e);
		}
		decryptor.Dispose();

		return decryptedData;
	}

	private static void GenerateKeyFromPassword(string password, int keySize, int blockSize, out byte[] key, out byte[] iv)
	{
		string pw = ConvertPassword(password);
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
