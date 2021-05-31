using MessagingClient.MessageFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingClient.MessageHandler
{
    public static class MessageProcessor
    {

        private static CryptoUtility mCUtil;

        static MessageProcessor()
        {
            mCUtil = new CryptoUtility();
        }

        public static BaseMessage CreateMessage(string data)
        {
            //if (data.Length < 16)
            //    throw new MessageTooShortException();

            if (mCUtil == null)
                mCUtil = new CryptoUtility();

            byte[] tdesKey = mCUtil.GenerateKey();
            byte[] text = Encoding.UTF8.GetBytes(data);
            byte[] encryptedBytes = mCUtil.EncryptionWith3DESECB(text, tdesKey);
            string encryptedText = Converter.ByteToHexString(encryptedBytes);

            string keyHex = Converter.ByteToHexString(tdesKey);
            string header = mCUtil.EncryptKeyCzar(keyHex);

            BaseMessage message = new BaseMessage();
            message.Header = header;
            message.Data = encryptedText;

            return message;
        }

        public static string ProcessMessage(BaseMessage message)
        {
            if (mCUtil == null)
                mCUtil = new CryptoUtility();

            string tdesKey = mCUtil.DecryptWithKeyCzar(message.Header);
            byte[] result = mCUtil.DecryptionWith3DESECB(message.Data, tdesKey);
            string decryptedHex = Converter.ByteToHexString(result);
            string decryptedText = Converter.HexStringToAscii(decryptedHex);

            return decryptedText;
        }


    }
}
