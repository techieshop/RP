using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RP.Common.Manager
{
	public static class EncryptionManager
	{
		private const string DefaultEncryptKey = "18s12rgb93pigeon";

		public static string Decrypt(string hash, string key = DefaultEncryptKey)
		{
			string decryptedString = null;
			if (!string.IsNullOrEmpty(hash))
			{
				byte[] cipherText = Convert.FromBase64String(hash);
				if (cipherText.Length > 0)
				{
					using (RijndaelManaged rijAlg = new RijndaelManaged())
					{
						rijAlg.Key = Encoding.UTF8.GetBytes(key);
						rijAlg.Mode = CipherMode.ECB;
						rijAlg.Padding = PaddingMode.PKCS7;

						ICryptoTransform decryptor = rijAlg.CreateDecryptor();
						using (MemoryStream msDecrypt = new MemoryStream(cipherText))
						{
							using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
							{
								using (StreamReader srDecrypt = new StreamReader(csDecrypt))
								{
									decryptedString = srDecrypt.ReadToEnd();
								}
							}
						}
					}
				}
			}
			return decryptedString;
		}

		public static string Encrypt(string data, string key = DefaultEncryptKey)
		{
			string encryptedString = null;
			if (!string.IsNullOrEmpty(data))
			{
				byte[] encryptedBytes;
				using (RijndaelManaged rijAlg = new RijndaelManaged())
				{
					rijAlg.Key = Encoding.UTF8.GetBytes(key);
					rijAlg.Mode = CipherMode.ECB;
					rijAlg.Padding = PaddingMode.PKCS7;

					ICryptoTransform encryptor = rijAlg.CreateEncryptor();
					using (MemoryStream msEncrypt = new MemoryStream())
					{
						using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
						{
							using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
							{
								swEncrypt.Write(data);
							}
							encryptedBytes = msEncrypt.ToArray();
						}
					}
				}
				encryptedString = Convert.ToBase64String(encryptedBytes);
			}
			return encryptedString;
		}

		public static byte[] GetRandomBytes(int length)
		{
			byte[] data;
			using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
			{
				data = new byte[length];
				rng.GetBytes(data);
			}
			return data;
		}
	}
}