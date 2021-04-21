using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Crypto.Sample
{
	public class DecryptionSample : MonoBehaviour
	{
		[SerializeField] Button _StartButton;

		[SerializeField] FolderType _FolderType;
		[SerializeField] string _SubFolder;
		[SerializeField] string _FileName;
		[SerializeField] string _Password = "test";

		private string _FolderPath = Application.streamingAssetsPath;

		void Start()
		{
			_StartButton.onClick.AddListener(() => 
			{
				string fileFullPath = GetFilePath();
				if (fileFullPath != null)
				{
					DecryptTest(fileFullPath);
				}
			});
		}

		private async void DecryptTest(string fileFullPath)
		{
			Debug.Log("***** Start *****");

			Debug.Log("***** ReadAsync *****");
			byte[] encryptedData;
			using (var sourceStream = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
			{
				encryptedData = new byte[sourceStream.Length];
				await sourceStream.ReadAsync(encryptedData, 0, encryptedData.Length);
			}

			Debug.Log("***** Decrypt *****");
			byte[] decryptedData = RijndaelDecryptor.Decrypt(encryptedData, _Password);

			if (decryptedData != null)
			{
				Debug.Log("***** WriteAsync *****");

				Directory.CreateDirectory($"{_FolderPath}/Decrypted/{_SubFolder}");
				string savePath = $"{_FolderPath}/Decrypted/{_SubFolder}/{_FileName}";

				using (var sourceStream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, true))
				{
					await sourceStream.WriteAsync(decryptedData, 0, decryptedData.Length);
				}

				Debug.Log("***** Decrypted data has been saved *****");
			}
		}

		private string GetFilePath()
		{
			switch (_FolderType)
			{
				case FolderType.StreamingAssets:
					_FolderPath = Application.streamingAssetsPath;
					break;
				case FolderType.Desktop:
					_FolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
					break;
				default:
					_FolderPath = Application.streamingAssetsPath;
					break;
			}

			string filePath =  $"{_FolderPath}/{_SubFolder}/{_FileName}";
			if (!File.Exists(filePath))
			{
				Debug.LogError("File not found");
				return null;
			}

			return filePath;
		}
	}
}
