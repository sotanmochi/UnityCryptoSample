using UnityEngine;
using System.IO;

public class EncryptionSample : MonoBehaviour
{
	private string password = "test";

	void Start()
	{
		string filename = "sample.txt";
		string path = Application.streamingAssetsPath + "/Text/" + filename;
		byte[] srcData = File.ReadAllBytes(path);

		byte[] encryptedData = RijndaelEncryptor.Encrypt(srcData, password);
		if (encryptedData != null)
		{
			string savepath = Application.streamingAssetsPath + "/Text/encrypted.txt";
			File.WriteAllBytes(savepath, encryptedData);
			Debug.Log("Encrypted data is saved.");
		}
	}
}
