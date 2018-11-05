using UnityEngine;
using System.IO;

public class VRMEncryptionSample : MonoBehaviour
{
	string pw = "test";
	string filename = "default.vrm";

	void Start()
	{
		string dirpath = Application.dataPath + "/../Models/";
		string filepath = dirpath + filename;
		byte[] encryptedData = RijndaelEncryptor.Encrypt(File.ReadAllBytes(filepath), pw);
		string savepath = dirpath + "encrypted.vrm.bytes";
		File.WriteAllBytes(savepath, encryptedData);
	}
}