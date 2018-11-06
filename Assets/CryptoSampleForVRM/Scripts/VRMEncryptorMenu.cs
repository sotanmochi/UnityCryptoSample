using System.IO;
using UnityEditor;
using UnityEngine;
using VRM;

public static class VRMEncryptorMenu
{
	static string pw = "test";

	[MenuItem(VRMVersion.VRM_VERSION + "/Encrypt")]
	static void EncryptMenu()
	{
		var path = EditorUtility.OpenFilePanel("Open VRM", "", "vrm");
		if (string.IsNullOrEmpty(path))
		{
			return;
		}

		byte[] encryptedData = RijndaelEncryptor.Encrypt(File.ReadAllBytes(path), pw);
		if (EditorUtility.DisplayDialog("VRM data has been encrypted. Do you want to save the encrypted data?", "", "Save", "Don't Save"))
		{
			var savepath = EditorUtility.SaveFilePanel("Save encrypted VRM", "", "encrypted.vrm", "bytes");
			if (!string.IsNullOrEmpty(savepath))
			{
				File.WriteAllBytes(savepath, encryptedData);
			}
		}
	}
}
