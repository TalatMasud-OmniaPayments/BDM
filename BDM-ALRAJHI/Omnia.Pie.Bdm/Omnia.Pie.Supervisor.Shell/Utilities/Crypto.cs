using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Omnia.Pie.Supervisor.Shell.Utilities
{
	public static class Crypto
	{
		private static byte[] _salt = Encoding.ASCII.GetBytes("R2293361kbQ4f1");
		public const string SharedKey = "@3#9$9%";

		public static string EncryptStringAes(string plainText, string sharedSecret)
		{
			string outStr = null;
			RijndaelManaged aesAlg = null;

			try
			{
				var key = new Rfc2898DeriveBytes(sharedSecret, _salt);

				aesAlg = new RijndaelManaged();
				aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
				aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
				var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (var swEncrypt = new StreamWriter(csEncrypt))
						{
							swEncrypt.Write(plainText);
						}
					}

					outStr = Convert.ToBase64String(msEncrypt.ToArray());
				}
			}
			finally
			{
				if (aesAlg != null)
					aesAlg.Clear();
			}

			return outStr;
		}

		public static string HashText(string text, string salt)
		{
			var bytes = Encoding.UTF8.GetBytes(string.Concat(text, salt));
			var hashAlgoritm = new MD5CryptoServiceProvider();

			try
			{
				var resBytes = hashAlgoritm.ComputeHash(bytes);
				return Convert.ToBase64String(resBytes);
			}
			finally
			{
				hashAlgoritm.Clear();
			}
		}
	}
}