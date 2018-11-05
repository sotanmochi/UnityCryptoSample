using UnityEngine;
using System.IO;

public class EncryptionSample : MonoBehaviour
{
	private string password = "test";

	void Start()
	{
		string filename = "sample.txt";
		string path = Application.dataPath + "/CryptoSample/Data/" + filename;
		byte[] srcData = File.ReadAllBytes(path);

		byte[] encryptedData = RijndaelEncryptor.Encrypt(srcData, password);
		if (encryptedData != null)
		{
			string savepath = Application.dataPath + "/CryptoSample/Data/encrypted.txt";
			File.WriteAllBytes(savepath, encryptedData);
			Debug.Log("Encrypted data is saved.");
		}
	}
}
