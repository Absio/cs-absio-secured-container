using System;

namespace KeyManagementUtilityUI.Utils
{
    public class Hex
    {
        public static string ToHex(byte[] data)
        {
            return BitConverter.ToString(data).ToLower().Replace("-", "");
        }

        public static byte[] FromHex(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}