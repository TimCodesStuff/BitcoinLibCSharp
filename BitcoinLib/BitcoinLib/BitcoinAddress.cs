namespace BitcoinLib
{
    public class BitcoinAddress
    {
        byte[] privateKey;
        byte[] publicKeyX;
        byte[] publicKeyY;
        byte[] publicKeyCompressed;
        byte[] publicKeySha256;
        byte[] publicKeySha256Ripe;
        // pay to public key hash
        byte[] p2pkh_publicKeyWithNetworkByte;
        byte[] p2pkh_checksum;
        byte[] p2pkh_addressWithChecksum;
        // pay to script hash
        byte[] p2sh_publicKeyWithNetworkByte;
        byte[] p2sh_checksum;
        byte[] p2sh_addressWithChecksum;

        NetworkType NetworkByte;

        byte[] PublicKeyFull
        {
            get
            {
                byte[] publicKey = new byte[65];
                publicKey[0] = 0x04;
                publicKeyX.CopyTo(publicKey, 1);
                publicKeyY.CopyTo(publicKey, 33);
                return publicKey;
            }
        }

        public string PrivateKey { get; private set; }
        public string PrivateKeyWIF { get; private set; }
        public string P2PKHAddress { get; private set; }
        public string P2SHAddress { get; private set; }
    }

    public enum NetworkType
    {
        Main = 0x00,
        Test = 0x6f
    }
}
