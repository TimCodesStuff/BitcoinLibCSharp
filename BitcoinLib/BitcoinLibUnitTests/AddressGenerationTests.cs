using BitcoinLib;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitcoinLibUnitTests
{
    /// <summary>
    /// Tests that Bitcoin Address generation works correctly and fails when expected. 
    /// WARNING: Do not use the addresses tested here to store bitcoin!
    /// See https://en.bitcoin.it/wiki/Technical_background_of_version_1_Bitcoin_addresses for more info on this address.
    /// </summary>
    [TestClass]
    public class AddressGenerationTests
    {
        // Binary Key Tests
        [TestMethod]
        public void GenerateAddressFromKnownGoodPrivateKey()
        {
            Byte[] key = Encoding.HexStringToByteArray("18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725");
            (BitcoinAddress address, String result) = BitcoinAddress.CreateAddressFromPrivateKeyByteArray(key, NetworkType.Main);
            Assert.IsNotNull(address);
            Assert.AreEqual(address.PrivateKey, "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725");
            Assert.AreEqual(address.PrivateKeyWIF, "Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C");
            Assert.AreEqual(address.P2PKHAddress, "1PMycacnJaSqwwJqjawXBErnLsZ7RkXUAs");
            Assert.AreEqual(address.P2SHAddress, "3BxwGNjvG4CP14tAZodgYyZ7UTjruYDyAM");
        }

        [TestMethod]
        public void InvalidBinaryKeyLengthTest()
        {
            int byteArrayLength = 33;
            Byte[] key = new Byte[byteArrayLength];
            (BitcoinAddress address, String result) = BitcoinAddress.CreateAddressFromPrivateKeyByteArray(key, NetworkType.Main);
            Assert.IsNull(address);
            Assert.AreEqual(result, $"Private key has invalid length of {byteArrayLength}, expected length of 32.");
        }


        // Hexadecimal Tests
        [TestMethod]
        public void GenerateAddressFromKnownGoodPrivateKeyHex()
        {
            (BitcoinAddress address, String result) = BitcoinAddress.CreateAddressFromPrivateKeyHex("18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725", NetworkType.Main);
            Assert.IsNotNull(address);
            Assert.AreEqual(address.PrivateKeyWIF, "Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C");
            Assert.AreEqual(address.P2PKHAddress, "1PMycacnJaSqwwJqjawXBErnLsZ7RkXUAs");
            Assert.AreEqual(address.P2SHAddress, "3BxwGNjvG4CP14tAZodgYyZ7UTjruYDyAM");
        }

        [TestMethod]
        public void InvalidHexadecimalKeyLengthTest()
        {
            string privateKeyHex = "118e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725";
            (BitcoinAddress address, String result) = BitcoinAddress.CreateAddressFromPrivateKeyHex(privateKeyHex, NetworkType.Main);
            Assert.IsNull(address);
            Assert.AreEqual(result, $"Private key hex string has invalid length of {privateKeyHex.Length}, expected length of 64.");
        }

        [TestMethod]
        public void InvalidHexadecimalCharacterTest()
        {
            (BitcoinAddress address, String result) = BitcoinAddress.CreateAddressFromPrivateKeyHex("Z8e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725", NetworkType.Main);
            Assert.IsNull(address);
            Assert.AreEqual(result, "Private key is not in hexadecimal format. Unexpected character in string.");
        }


        // WIF tests
        [TestMethod]
        public void GenerateAddressFromKnownGoodPrivateKeyWIF()
        {
            (BitcoinAddress address, String result) = BitcoinAddress.CreateAddressFromPrivateKeyWIF("Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C", NetworkType.Main);
            Assert.IsNotNull(address);
            Assert.AreEqual(address.PrivateKey, "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725");
            Assert.AreEqual(address.P2PKHAddress, "1PMycacnJaSqwwJqjawXBErnLsZ7RkXUAs");
            Assert.AreEqual(address.P2SHAddress, "3BxwGNjvG4CP14tAZodgYyZ7UTjruYDyAM");
        }

        [TestMethod]
        public void InvalidPrivateKeyWIFChecksum()
        {
            // Changed last character from "C" to "D" to modify checksum.
            (BitcoinAddress address, String result) = BitcoinAddress.CreateAddressFromPrivateKeyWIF("Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3D", NetworkType.Main);
            Assert.IsNull(address);
            Assert.AreEqual(result, "WIF private key did not pass checksum test.");
        }
    }
}
