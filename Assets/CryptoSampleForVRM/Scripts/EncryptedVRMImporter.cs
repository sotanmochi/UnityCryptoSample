using System;
using System.IO;
using UnityEngine;

public static class EncryptedVRMImporter
{
	public static void LoadVrm(string path, string password, Action<GameObject> onLoaded)
	{
		if (!string.IsNullOrEmpty(path))
		{
			VRM.VRMImporter.LoadVrmAsync(Crypto.RijndaelDecryptor.Decrypt(File.ReadAllBytes(path), password), onLoaded);
		}
	}
}
