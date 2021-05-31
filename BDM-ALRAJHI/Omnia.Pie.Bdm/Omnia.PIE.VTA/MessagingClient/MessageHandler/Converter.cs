using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingClient.MessageHandler
{
    public sealed class Converter
    {
        public static readonly string NewLine = "\r\n";

        private static char[] hexDigits = new char[]
        {
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            'A',
            'B',
            'C',
            'D',
            'E',
            'F'
        };

        public static string ByteToHexString(byte[] bytes)
        {
            char[] array = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int num = (int)bytes[i];
                array[i * 2] = Converter.hexDigits[num >> 4];
                array[i * 2 + 1] = Converter.hexDigits[num & 15];
            }
            return new string(array);
        }

        public static string GetNibbleFromString(string pData)
        {
            if (string.IsNullOrEmpty(pData))
            {
                return string.Empty;
            }
            char[] array = new char[pData.Length * 2];
            for (int i = 0; i < pData.Length; i++)
            {
                int num = (int)pData[i];
                array[i * 2] = Converter.hexDigits[num >> 4];
                array[i * 2 + 1] = Converter.hexDigits[num & 15];
            }
            return new string(array);
        }

        public static string GetStringFromNibble(string pData)
        {
            if (string.IsNullOrEmpty(pData))
            {
                return string.Empty;
            }
            string text = string.Empty;
            for (int i = 0; i < pData.Length / 2; i++)
            {
                string pByte = pData.Substring(i * 2, 2);
                char c = (char)Converter.ToByte(pByte, NumberStyles.AllowHexSpecifier);
                text += c;
            }
            return text;
        }

        public static string GetNibbleFrom1252EncodedString(string pData)
        {
            byte[] bytes = Encoding.GetEncoding(1252).GetBytes(pData);
            char[] array = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                array[i * 2] = Converter.hexDigits[bytes[i] >> 4];
                array[i * 2 + 1] = Converter.hexDigits[(int)(bytes[i] & 15)];
            }
            return new string(array);
        }

        public static string GetStringFrom1252EncodedString(string pData)
        {
            byte[] bytes = Encoding.GetEncoding(1252).GetBytes(pData);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                stringBuilder.Append((char)bytes[i]);
            }
            return stringBuilder.ToString();
        }

        public static byte[] HexStringToByte(string pStrHex)
        {
            if (pStrHex == string.Empty)
            {
                return null;
            }
            byte[] array = new byte[pStrHex.Length / 2];
            pStrHex = pStrHex.ToUpper();
            for (int i = 0; i < pStrHex.Length; i++)
            {
                byte b = (byte)pStrHex[i];
                if (b >= 48 && b <= 57)
                {
                    b -= 48;
                }
                else
                {
                    b = (byte)(b - 65 + 10);
                }
                if (i % 2 == 0)
                {
                    array[i / 2] = (byte)(b * 16);
                }
                else
                {
                    byte[] expr_67_cp_0 = array;
                    int expr_67_cp_1 = i / 2;
                    expr_67_cp_0[expr_67_cp_1] += b;
                }
            }
            return array;
        }

        public static string HexStringToAscii(string hexString)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexString.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        public static bool Base94Encode(string pSrcHexString, out string pDstString)
        {
            if (string.IsNullOrEmpty(pSrcHexString))
            {
                pDstString = string.Empty;
                return false;
            }
            byte[] array = Converter.HexStringToByte(pSrcHexString);
            byte[] array2 = new byte[array.Length / 4 * 5];
            if (!Converter.CanEncodeToBase94(array, array2, array.Length, array2.Length))
            {
                pDstString = string.Empty;
                return false;
            }
            Encoding encoding = Encoding.GetEncoding(1252);
            pDstString = encoding.GetString(array2, 0, array2.Length);
            if (pDstString.Length != 320)
            {
                pDstString = string.Empty;
                return false;
            }
            return true;
        }

        private static bool CanEncodeToBase94(byte[] src, byte[] dst, int nSrcSize, int nDstSize)
        {
            if (nSrcSize % 4 != 0)
            {
                return false;
            }
            if (nSrcSize / 4 * 5 > nDstSize)
            {
                return false;
            }
            uint num = 0u;
            int num2 = nSrcSize / 4;
            for (int i = 0; i < num2; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    num = num * 256u + (uint)src[i * 4 + 3 - j];
                }
                for (int j = 0; j <= 4; j++)
                {
                    dst[i * 5 + j] = (byte)(num % 94u + 32u);
                    num /= 94u;
                }
            }
            return true;
        }

        public static bool Base94Decode(string pSrcString, out string pDstHexString)
        {
            if (string.IsNullOrEmpty(pSrcString))
            {
                pDstHexString = string.Empty;
                return false;
            }
            byte[] bytes = Encoding.ASCII.GetBytes(pSrcString);
            byte[] array = new byte[bytes.Length / 5 * 4];
            if (Converter.CanDecodeToBase94(bytes, array, bytes.Length, array.Length))
            {
                pDstHexString = Converter.ByteToHexString(array);
                return true;
            }
            pDstHexString = string.Empty;
            return false;
        }

        private static bool CanDecodeToBase94(byte[] src, byte[] dst, int nSrcSize, int nDstSize)
        {
            if (nSrcSize % 5 != 0)
            {
                return false;
            }
            if (nSrcSize / 5 * 4 > nDstSize)
            {
                return false;
            }
            uint num = 0u;
            int num2 = nSrcSize / 5;
            for (int i = 0; i < num2; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    num = num * 94u + (uint)src[i * 5 + 4 - j] - 32u;
                }
                for (int j = 0; j <= 3; j++)
                {
                    dst[i * 4 + j] = (byte)(num % 256u);
                    num /= 256u;
                }
            }
            return true;
        }

        public static string ThreeDigitToHexString(string pIntVal)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int num = pIntVal.Length / 3;
            for (int i = 0; i < num; i++)
            {
                string s = pIntVal.Substring(i * 3, 3);
                string value = int.Parse(s).ToString("X2");
                stringBuilder.Append(value);
            }
            return stringBuilder.ToString();
        }

        public static string ToUnicodeString(string pString)
        {
            string empty = string.Empty;
            byte[] bytes = Encoding.Unicode.GetBytes(pString);
            return Encoding.Unicode.GetString(bytes, 0, bytes.Length);
        }

        public static bool ToBoolean(string pBool)
        {
            if (string.IsNullOrEmpty(pBool))
            {
                return false;
            }
            bool result = false;
            try
            {
                result = Convert.ToBoolean(pBool);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static int ToInteger(string pIntVal, int pFromBase)
        {
            if (string.IsNullOrEmpty(pIntVal))
            {
                return 0;
            }
            if (pFromBase != 2 && pFromBase != 8 && pFromBase != 10 && pFromBase != 16)
            {
                return 0;
            }
            int result = 0;
            try
            {
                result = Convert.ToInt32(pIntVal, pFromBase);
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public static int ToInteger(string pIntVal)
        {
            return Converter.ToInteger(pIntVal, 10);
        }

        public static string GetStringFromStream(byte[] pData)
        {
            return Converter.GetStringFromStream(pData, 0, pData.Length);
        }

        public static string GetStringFromStream(byte[] pData, int pOffset, int pSize)
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");
            return encoding.GetString(pData, pOffset, pSize);
        }

        public static byte[] GetStreamFromString(string pVal)
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");
            return encoding.GetBytes(pVal);
        }

        public static byte ToByte(string pByte, NumberStyles pSpecifier)
        {
            byte result = 0;
            try
            {
                if (pByte == null || pByte == string.Empty)
                {
                    return result;
                }
                result = byte.Parse(pByte, pSpecifier);
            }
            catch
            {
            }
            return result;
        }

        public static string NumberStringToHex(string pIntVal)
        {
            char c = '0';
            try
            {
                c = (char)Converter.ToByte(pIntVal, NumberStyles.HexNumber);
            }
            catch
            {
            }
            return c.ToString();
        }

        public static bool NumberStringToBoolean(string pVal)
        {
            bool result = false;
            try
            {
                int num = int.Parse(pVal);
                if (num == 1)
                {
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }

        public static string BinaryToString(byte[] pStream, int pOffset, int pLength)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < pLength; i++)
            {
                byte b = pStream[pOffset + i];
                byte b2 = (byte)((b & 240) >> 4);
                if (b2 < 10)
                {
                    b2 += 48;
                }
                else
                {
                    b2 += 55;
                }
                stringBuilder.Append((char)b2);
                b2 = (byte)(b & 15);
                if (b2 < 10)
                {
                    b2 += 48;
                }
                else
                {
                    b2 += 55;
                }
                stringBuilder.Append((char)b2);
                stringBuilder.Append(" ");
                if ((i + 1) % 16 == 0)
                {
                    stringBuilder.Append(NewLine);
                }
                else if ((i + 1) % 8 == 0)
                {
                    stringBuilder.Append("- ");
                }
            }
            return stringBuilder.ToString();
        }

        public static object VariantToTypeArray(object pVariant, Type pType)
        {
            if (pVariant == null)
            {
                return null;
            }
            object[] array = pVariant as object[];
            Array array2 = null;
            int length = array.Length;
            if (array.Length <= 0)
            {
                length = 0;
            }
            try
            {
                array2 = Array.CreateInstance(pType, length);
                Array.Copy(array, array2, length);
            }
            catch (RankException ex)
            {
                throw ex;
            }
            catch (InvalidCastException ex2)
            {
                throw ex2;
            }
            return array2;
        }

        public static long ToInteger64(string pIntVal)
        {
            long result = 0L;
            if (StringConverter.IsNumber(pIntVal))
            {
                try
                {
                    result = Convert.ToInt64(pIntVal);
                }
                catch
                {
                    return 0L;
                }
                return result;
            }
            return result;
        }
    }
}
