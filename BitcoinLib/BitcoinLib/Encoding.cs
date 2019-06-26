using System.Numerics;
using System;
using System.Linq;

namespace BitcoinLib
{
    public class Encoding
    {
        public static byte[] HexStringToByteArray(string hexString)
        {
            return Enumerable.Range(0, hexString.Length) // Generate an enumerable of ints from 1 to hexString length
                             .Where(x => x % 2 == 0) // Filter out all odd numbers from enumerable
                             .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16)) // Convert each set of 2 hex characters to byte
                             .ToArray();
        }

        public static string ByteArrayToHexString(byte[] input, bool includeDashes = false)
        {
            if (includeDashes)
                return BitConverter.ToString(input).ToLower();
            else
                return BitConverter.ToString(input).Replace("-", "").ToLower();
        }

        /// <summary> Takes a Wallet Import Format string and converts it to a Hexadecimal string. See https://en.bitcoin.it/wiki/Wallet_import_format. </summary>
        public static string WiftoHex(string wifKey)
        {
            // Base58 decode
            string hex = ByteArrayToHexString(Base58Decode(wifKey));

            // Remove first byte (main network byte) and last 4 bytes (checksum)
            return hex.Substring(2, 64);
        }

        public static string HexToWif(string hex, NetworkType network, bool isCompressedPublicKey = true)
        {
            //TODO: Verify string is Hex and correct length.
            hex = (network == NetworkType.Main) ? "80" + hex : "EF" + hex;

            if (isCompressedPublicKey)
            {
                hex += "01";
            }
            byte[] doubleSha = Crypto.Sha256(Crypto.Sha256(HexStringToByteArray(hex)));
            string checksum = ByteArrayToHexString(doubleSha).Substring(0, 8);
            hex += checksum;
            return Base58Encode(HexStringToByteArray(hex));
        }

        public static (string, string, string) DecomposeWifAddress(string wifKey)
        {
            // Base58 decode
            string hex = Encoding.ByteArrayToHexString(Base58Decode(wifKey));

            //TODO: Check if WIF was used for compressed public key.

            string network = hex.Substring(0, 2);
            string wif = hex.Substring(2, 64);
            string checksum = hex.Substring(68, 8);

            return (network, wif, checksum);
        }

        public static bool ValidateWifAddressChecksum(string wifKey)
        {
            (string network, string hex, string wifChecksum) = Encoding.DecomposeWifAddress(wifKey);
            byte[] doubleSha = Crypto.Sha256(Crypto.Sha256(HexStringToByteArray(network + hex + "01")));
            string checksum = ByteArrayToHexString(doubleSha).Substring(0, 8);
            return checksum == wifChecksum;
        }

        // Base 58 contains no 0, O, l, or I.
        private static string Base58characterSet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        /// <param name="array">An array of bytes to be encoded as base58.</param>
        /// <returns>A string of base58 characters.</returns>
        public static string Base58Encode(byte[] array)
        {
            BigInteger arrayToInt = 0;
            for (int i = 0; i < array.Length; ++i)
            {
                arrayToInt = arrayToInt * 256 + array[i];
            }

            string retString = string.Empty;
            while (arrayToInt > 0)
            {
                int rem = (int)(arrayToInt % 58);
                arrayToInt /= 58;
                retString = Base58characterSet[rem] + retString;
            }
            for (int i = 0; i < array.Length && array[i] == 0; ++i)
                retString = Base58characterSet[0] + retString;

            return retString;
        }

        // Taken from https://gist.github.com/CodesInChaos/3175971#file-base58encoding-cs
        public static byte[] Base58Decode(string b58string)
        {
            // Decode Base58 string to BigInteger
            BigInteger intData = 0;
            for (int i = 0; i < b58string.Length; i++)
            {
                int digit = Base58characterSet.IndexOf(b58string[i]);
                // TODO: Handle invalid character.
                intData = intData * 58 + digit;
            }

            // Encode BigInteger to byte[]
            // Leading zero bytes get encoded as leading `1` characters
            int leadingZeroCount = b58string.TakeWhile(c => c == '1').Count();
            var leadingZeros = Enumerable.Repeat((byte)0, leadingZeroCount);
            var bytesWithoutLeadingZeros =
                intData.ToByteArray()
                .Reverse()// to big endian
                .SkipWhile(b => b == 0);//strip sign byte
            var result = leadingZeros.Concat(bytesWithoutLeadingZeros).ToArray();
            return result;
        }
    }
}
