using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MessagingClient.MessageHandler
{
    public sealed class StringConverter
    {
        public static string StringLeft(string pSource, int pLength)
        {
            if (string.IsNullOrEmpty(pSource))
            {
                return string.Empty;
            }
            if (pLength < 1)
            {
                return pSource;
            }
            if (pLength > pSource.Length)
            {
                return pSource;
            }
            return pSource.Substring(0, pLength);
        }

        public static string StringRight(string pSource, int pLength)
        {
            if (string.IsNullOrEmpty(pSource))
            {
                return string.Empty;
            }
            if (pLength < 1)
            {
                return pSource;
            }
            if (pLength > pSource.Length)
            {
                return pSource;
            }
            return pSource.Substring(pSource.Length - pLength);
        }

        public static bool IsNumber(string pSource)
        {
            if (pSource == null)
            {
                return false;
            }
            if (pSource.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < pSource.Length; i++)
            {
                char c = pSource[i];
                if (!char.IsNumber(c))
                {
                    return false;
                }
            }
            return true;
        }

        public static int NumberOfOccurrence(string pSource, char compareChar)
        {
            int num = 0;
            if (pSource == null)
            {
                return 0;
            }
            if (pSource.Length == 0)
            {
                return 0;
            }
            for (int i = 0; i < pSource.Length; i++)
            {
                char c = pSource[i];
                if (c == compareChar)
                {
                    num++;
                }
            }
            return num;
        }

        public static bool IsValidNumberFormat(string pSource)
        {
            if (pSource == null)
            {
                return false;
            }
            if (pSource.Length == 0)
            {
                return false;
            }
            string pattern = "^([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(\\.[0-9]+)?$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(pSource);
        }

        public static string SubStringWithoutException(string pSourceStr, int pStartIndex, int pLength)
        {
            string result = string.Empty;
            try
            {
                result = pSourceStr.Substring(pStartIndex, pLength);
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        public static bool EqualWithoutException(string pSource, string pTarget)
        {
            return !string.IsNullOrEmpty(pSource) && !string.IsNullOrEmpty(pTarget) && pSource == pTarget;
        }

        public static string GetUSCentExpressionRightAllign(string pAmount)
        {
            string uSCentExpression = StringConverter.GetUSCentExpression(pAmount);
            return uSCentExpression.PadLeft(14, ' ');
        }

        public static string GetUSCentExpression(string pAmount)
        {
            if (string.IsNullOrEmpty(pAmount))
            {
                return "0.00";
            }
            if (!StringConverter.IsNumber(pAmount))
            {
                return pAmount;
            }
            string result = string.Empty;
            if (pAmount.Length == 1)
            {
                result = "0.0" + pAmount;
            }
            else if (pAmount.Length == 2)
            {
                result = "0." + pAmount;
            }
            else
            {
                string value = pAmount.Substring(0, pAmount.Length - 2);
                long num = Convert.ToInt64(value);
                string text = pAmount.Substring(pAmount.Length - 2, 2);
                if (num == 0L)
                {
                    result = "0." + text;
                }
                else
                {
                    result = num.ToString("###,###,###") + "." + text;
                }
            }
            return result;
        }

        public static string MICRHexToDec(string pCodeLineData)
        {
            if (string.IsNullOrEmpty(pCodeLineData))
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            while (num < pCodeLineData.Length && num + 2 <= pCodeLineData.Length)
            {
                string text = pCodeLineData.Substring(num, 2);
                if (text == "64")
                {
                    stringBuilder.Append(":");
                }
                else if (text == "62")
                {
                    stringBuilder.Append(";");
                }
                else if (text == "63")
                {
                    stringBuilder.Append("<");
                }
                else if (text == "2D")
                {
                    stringBuilder.Append("=");
                }
                else if (text == "62")
                {
                    stringBuilder.Append(";");
                }
                else if (text[0] == '3' && char.IsDigit(text[1]))
                {
                    stringBuilder.Append(text[1]);
                }
                num += 2;
            }
            return stringBuilder.ToString();
        }
    }
}
