using UnityEngine;
using System.IO;

public class DecryptionSample : MonoBehaviour
{
	private string password = "test";

	void Start()
	{
		string filename = "encrypted.txt";
		string path = Application.streamingAssetsPath + "/Text/" + filename;
		byte[] encryptedData = File.ReadAllBytes(path);

		byte[] decryptedData = RijndaelDecryptor.Decrypt(encryptedData, password);
		if (decryptedData != null)
		{
			string decryptedText = System.Text.Encoding.UTF8.GetString(decryptedData);
			Debug.Log("DecryptedText: " + decryptedText);
		}
	}
}
