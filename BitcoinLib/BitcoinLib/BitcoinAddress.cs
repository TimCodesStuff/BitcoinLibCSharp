using System;
using System.Text.RegularExpressions;

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

        private BitcoinAddress(byte[] privateKey, NetworkType networkByte)
        {
            NetworkByte = networkByte;

            this.privateKey = privateKey;
            PrivateKey = Encoding.ByteArrayToHexString(privateKey);
            PrivateKeyWIF = Encoding.HexToWif(Encoding.ByteArrayToHexString(privateKey));

            (publicKeyX, publicKeyY) = Bitcoin.GetSecp256k1PublicKey(privateKey);

            publicKeyCompressed = Bitcoin.GenerateCompressedPublicKey(publicKeyX, publicKeyY);
            publicKeySha256 = Crypto.Sha256(publicKeyCompressed);
            publicKeySha256Ripe = Crypto.RipeMD160(publicKeySha256);

            // Build p2pkh address
            p2pkh_publicKeyWithNetworkByte = new byte[21];
            p2pkh_publicKeyWithNetworkByte[0] = (byte)networkByte;
            publicKeySha256Ripe.CopyTo(p2pkh_publicKeyWithNetworkByte, 1);

            p2pkh_checksum = Bitcoin.GenerateChecksum(p2pkh_publicKeyWithNetworkByte);

            p2pkh_addressWithChecksum = new byte[25];
            p2pkh_publicKeyWithNetworkByte.CopyTo(p2pkh_addressWithChecksum, 0);
            p2pkh_checksum.CopyTo(p2pkh_addressWithChecksum, 21);

            P2PKHAddress = Encoding.Base58Encode(p2pkh_addressWithChecksum);

            // Build p2sh address
            p2sh_publicKeyWithNetworkByte = new byte[21];
            if (networkByte == NetworkType.Main)
            {
                p2sh_publicKeyWithNetworkByte[0] = 0x05;
            }
            else
            {
                p2sh_publicKeyWithNetworkByte[0] = 0xc4;
            }

            publicKeySha256Ripe.CopyTo(p2sh_publicKeyWithNetworkByte, 1);
            p2sh_checksum = Bitcoin.GenerateChecksum(p2sh_publicKeyWithNetworkByte);

            p2sh_addressWithChecksum = new byte[25];
            p2sh_publicKeyWithNetworkByte.CopyTo(p2sh_addressWithChecksum, 0);
            p2sh_checksum.CopyTo(p2sh_addressWithChecksum, 21);

            P2SHAddress = Encoding.Base58Encode(p2sh_addressWithChecksum);

        }

        public string AddressInfo()
        {
            return
            $"Private key:                  {Encoding.ByteArrayToHexString(privateKey)}" + Environment.NewLine +
            $"Private key WIF:              {PrivateKeyWIF}" + Environment.NewLine +
            $"Public key X:                 {Encoding.ByteArrayToHexString(publicKeyX)}" + Environment.NewLine +
            $"Public key Y:                 {Encoding.ByteArrayToHexString(publicKeyY)}" + Environment.NewLine +
            $"Public key hex (full):        {Encoding.ByteArrayToHexString(PublicKeyFull)}" + Environment.NewLine +
            $"Public key hex (compressed):  {Encoding.ByteArrayToHexString(publicKeyCompressed)}" + Environment.NewLine +
            $"Public key comp sha256:       {Encoding.ByteArrayToHexString(publicKeySha256)}" + Environment.NewLine +
            $"Public key hash160 hex:       {Encoding.ByteArrayToHexString(publicKeySha256Ripe)}" + Environment.NewLine +
            $"Network Byte:                 {NetworkByte} (0x{((byte)NetworkByte)})" + Environment.NewLine +
            $"P2PKH Pub key w network byte: {Encoding.ByteArrayToHexString(p2pkh_publicKeyWithNetworkByte)}" + Environment.NewLine +
            $"P2PKH Checksum:               {Encoding.ByteArrayToHexString(p2pkh_checksum)}" + Environment.NewLine +
            $"P2PKH Address Hex:            {Encoding.ByteArrayToHexString(p2pkh_addressWithChecksum)}" + Environment.NewLine +
            $"P2PKH Address Base58:         {P2PKHAddress}" + Environment.NewLine +
            $"P2SH Pub key w network byte:  {Encoding.ByteArrayToHexString(p2sh_publicKeyWithNetworkByte)}" + Environment.NewLine +
            $"P2SH Checksum:                {Encoding.ByteArrayToHexString(p2sh_checksum)}" + Environment.NewLine +
            $"P2SH Address Hex:             {Encoding.ByteArrayToHexString(p2sh_addressWithChecksum)}" + Environment.NewLine +
            $"P2SH Address Base58:          {P2SHAddress}" + Environment.NewLine;
        }

        public static (BitcoinAddress, string) CreateAddressFromPrivateKeyByteArray(byte[] privateKey, NetworkType network)
        {
            if(privateKey.Length != 32) {
                return (null, $"Private key has invalid length of {privateKey.Length}, expected length of 32.");
            }
            return (new BitcoinAddress(privateKey, network), "Success.");
        }

        public static (BitcoinAddress, string) CreateAddressFromPrivateKeyHex(string privateKeyHex, NetworkType network)
        {
            if (privateKeyHex.Length != 64){
                return (null, $"Private key hex string has invalid length of {privateKeyHex.Length}, expected length of 64.");
            }
            if (!(new Regex(@"^[0-9a-f]+$").Match(privateKeyHex.ToLowerInvariant()).Success))
            {
                return (null, "Private key is not in hexadecimal format. Unexpected character in string.");
            }
            return (new BitcoinAddress(Encoding.HexStringToByteArray(privateKeyHex), network), "Success.");
        }

        public static (BitcoinAddress, string) CreateAddressFromPrivateKeyWIF(string privateKeyWIF, NetworkType network)
        {
            //TODO: Check that the WIF format is correct using the checksum
            return (new BitcoinAddress(Encoding.HexStringToByteArray(Encoding.WIFtoHex(privateKeyWIF)), network), "Success.");
        }
    }

    public enum NetworkType
    {
        Main = 0x00,
        Test = 0x6f
    }
}
