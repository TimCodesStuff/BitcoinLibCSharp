using System.Security.Cryptography;

namespace BitcoinLib
{
    public class Crypto
    {
        public static byte[] Sha256(byte[] array)
        {
            return new SHA256Managed().ComputeHash(array);
        }

        public static byte[] RipeMD160(byte[] array)
        {
            return new RIPEMD160Managed().ComputeHash(array);
        }

        public static byte[] GenerateRand256BitKey()
        {
            byte[] key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            return key;
        }

        public static byte[] GenerateRandECDSACompliant256BitKey()
        {
            string upperLimit = "fffffffffffffffffffffffffffffffebaaedce6af48a03bbfd25e8cd0364140";
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            byte[] key = new byte[32];
            string keyHex;
            do
            {
                rng.GetBytes(key);
                keyHex = Encoding.ByteArrayToHexString(key).ToLower();

            } while (string.Compare(keyHex, upperLimit, true) >= 0);

            return key;
        }
    }
}
