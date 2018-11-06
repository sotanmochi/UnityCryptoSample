using UnityEngine;
using System.IO;

public class VRMRuntimeLoadSample : MonoBehaviour
{
	public GameObject VRMRoot;

	string pw = "test";
	string filename = "encrypted.vrm.bytes";

	void Start()
	{
		string dirpath = Application.streamingAssetsPath + "/VRM/";
		string filepath = dirpath + filename;
		EncryptedVRMImporter.LoadVrm(filepath, pw, OnLoadedVrm);
	}

	private void OnLoadedVrm(GameObject vrm)
	{
		if (VRMRoot != null)
		{
			vrm.transform.SetParent(VRMRoot.transform, false);
		}
	}
}