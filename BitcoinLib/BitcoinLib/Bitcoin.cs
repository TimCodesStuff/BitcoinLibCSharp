using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System.Linq;

namespace BitcoinLib
{
    public class Bitcoin
    {
        public static byte[] GenerateCompressedPublicKey(byte[] publicKeyX, byte[] publicKeyY)
        {
            byte[] publicKey = new byte[33];
            if ((int)publicKeyY[31] % 2 == 0)
            {
                publicKey[0] = 0x02;
            }
            else
            {
                publicKey[0] = 0x03;
            }

            publicKeyX.CopyTo(publicKey, 1);
            return publicKey;
        }

        public static (byte[], byte[]) GetSecp256k1PublicKey(byte[] privateKey)
        {
            X9ECParameters curve = SecNamedCurves.GetByName("secp256k1");
            ECDomainParameters domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            BigInteger d = new BigInteger(+1, privateKey);

            var publicKey = new ECPublicKeyParameters(domain.G.Multiply(d), domain);
            return (publicKey.Q.XCoord.GetEncoded(), publicKey.Q.YCoord.GetEncoded());
        }

        public static byte[] GenerateChecksum(byte[] address)
        {
            byte[] doubleSha = Crypto.Sha256(Crypto.Sha256(address));
            return doubleSha.Take(4).ToArray();
        }
    }
}
