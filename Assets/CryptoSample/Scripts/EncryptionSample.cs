using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Crypto.Sample
{
	public enum FolderType
	{
		StreamingAssets,
		Desktop,
	};

	public class EncryptionSample : MonoBehaviour
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
					EncryptTest(fileFullPath);
				}
			});
		}

		private async void EncryptTest(string fileFullPath)
		{
			Debug.Log("***** Start *****");

			Debug.Log("***** ReadAsync *****");
			byte[] data;
			using (var sourceStream = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, true))
			{
				data = new byte[sourceStream.Length];
				await sourceStream.ReadAsync(data, 0, data.Length);
			}

			Debug.Log("***** Encrypt *****");
			byte[] encryptedData = await RijndaelEncryptor.EncryptAsync(data, _Password);
			// byte[] encryptedData = RijndaelEncryptor.Encrypt(data, _Password);

			if (encryptedData != null)
			{
				Debug.Log("***** WriteAsync *****");

				Directory.CreateDirectory($"{_FolderPath}/Encrypted/{_SubFolder}");
				string savePath = $"{_FolderPath}/Encrypted/{_SubFolder}/{_FileName}";

				using (var sourceStream = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, true))
				{
					await sourceStream.WriteAsync(encryptedData, 0, encryptedData.Length);
				}

				Debug.Log("***** Encrypted data has been saved *****");
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
