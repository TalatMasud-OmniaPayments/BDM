//using Keyczar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MessagingClient.MessageHandler
{
    public class CryptoUtility
    {
        public string PUBLIC_ENC_KEY = "";//SystemDirectories.ConfigurationsDirectory +  "public\\crypt";
        //public string SIGNER_KEY = "C:/Users/Toxic/Downloads/MessagingClient/Keys/2048/sender/private/sign";
        public string DEC_KEY = "";// SystemDirectories.ConfigurationsDirectory +  "private\\crypt";
        //public string VERIFY_KEY = "C:/Users/Toxic/Downloads/MessagingClient/Keys/2048/sender/public/sign";

        public byte[] EncryptionWith3DESECB(byte[] pData, byte[] pKey)
        {
            TripleDES des = CreateDES(pKey);
            ICryptoTransform ct = des.CreateEncryptor();
            return ct.TransformFinalBlock(pData, 0, pData.Length);
        }

        public byte[] EncryptionWith3DESECB(string hexString, byte[] pKey)
        {
            TripleDES des = CreateDES(pKey);
            ICryptoTransform ct = des.CreateEncryptor();
            byte[] input = Converter.HexStringToByte(hexString);
            return ct.TransformFinalBlock(input, 0, input.Length);
        }

        public byte[] EncryptionWith3DESECB(string hexString, string hexKey)
        {
            TripleDES des = CreateDES(hexKey);
            ICryptoTransform ct = des.CreateEncryptor();
            byte[] input = Converter.HexStringToByte(hexString);
            return ct.TransformFinalBlock(input, 0, input.Length);
        }

        public byte[] DecryptionWith3DESECB(byte[] pData, byte[] pKey)
        {
            TripleDES des = CreateDES(pKey);
            ICryptoTransform ct = des.CreateDecryptor();
            return ct.TransformFinalBlock(pData, 0, pData.Length);
        }

        public byte[] DecryptionWith3DESECB(string hexString, byte[] pKey)
        {
            TripleDES des = CreateDES(pKey);
            ICryptoTransform ct = des.CreateDecryptor();
            byte[] input = Converter.HexStringToByte(hexString);
            return ct.TransformFinalBlock(input, 0, input.Length);
        }

        public byte[] DecryptionWith3DESECB(string hexString, string hexKey)
        {
            TripleDES des = CreateDES(hexKey);
            ICryptoTransform ct = des.CreateDecryptor();
            byte[] input = Converter.HexStringToByte(hexString);
            return ct.TransformFinalBlock(input, 0, input.Length);
        }

        private TripleDES CreateDES(string key)
        {
            //MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = Converter.HexStringToByte(key); //md5.ComputeHash(Encoding.Unicode.GetBytes(key));
            des.IV = new byte[des.BlockSize / 8];
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            return des;
        }

        private TripleDES CreateDES(byte[] key)
        {
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = key;
            des.IV = new byte[des.BlockSize / 8];
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            return des;
        }

        public byte[] GenerateKey()
        {
            var rng = new RNGCryptoServiceProvider();
            var key = new byte[24];
            rng.GetBytes(key);

            for (var i = 0; i < key.Length; ++i)
            {
                int keyByte = key[i] & 0xFE;
                var parity = 0;
                for (var b = keyByte; b != 0; b >>= 1) parity ^= b & 1;
                key[i] = (byte)(keyByte | (parity == 0 ? 1 : 0));
            }

            return key;
        }

        public string EncryptKeyCzar(string text)
        {
            Encrypter encryptor = new Encrypter(PUBLIC_ENC_KEY);
            WebBase64 base64 = encryptor.Encrypt(text);
            return base64;
        }

        public string DecryptWithKeyCzar(string text)
        {
            Crypter crypter = new Crypter(DEC_KEY);
            return crypter.Decrypt(new WebBase64(text));
        }
    }
}
