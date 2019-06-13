# BitcoinLibCSharp

This C# library provides the ability to generate bitcoin addresses using a randomly generated private key or your own private key. Your own private key can be supplied in raw binary form, hexadecimal format, or Wallet Input Format (WIF). The library provides information about the address, including: 
* Private Key in Hexadecimal
* Private Key in Wallet Input Format
* Public Key Compressed
* Public Key Full
* Public Key Hash160
* Pay To Public Key Hash (V1) Address in Hexadecimal
* Pay To Public Key Hash (V1) Address in Base58
* Pay To Script Hash Address in Hexadecimal
* Pay To Script Hash Address in Base58

## Usage

To generate a new address, use the `BitcoinAddress.CreateRandomAddress()` method. You can specify whether you'd like to create a real address on the main Bitcoin network, or an address on the test Bitcoin network using the `NetworkType` enum.

```c#
using BitcoinLib;
...
BitcoinAddress address = BitcoinAddress.CreateRandomAddress(NetworkType.Main);
```

You can get all of the information about this address, including public/private key, using the `AddressInfo` method.

```c#
Console.WriteLine(address.AddressInfo());
```

This will print out the following:

```
Private key:                  18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725
Private key WIF:              Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C
Public key X:                 50863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352
Public key Y:                 2cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6
Public key hex (full):        0450863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b23522cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6
Public key hex (compressed):  0250863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352
Public key comp sha256:       0b7c28c9b7290c98d7438e70b3d3f7c848fbd7d1dc194ff83f4f7cc9b1378e98
Public key hash160 hex:       f54a5851e9372b87810a8e60cdd2e7cfd80b6e31
Network Byte:                 Main (0x0)
P2PKH Pub key w network byte: 00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31
P2PKH Checksum:               c7f18fe8
P2PKH Address Hex:            00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31c7f18fe8
P2PKH Address Base58:         1PMycacnJaSqwwJqjawXBErnLsZ7RkXUAs
P2SH Pub key w network byte:  05f54a5851e9372b87810a8e60cdd2e7cfd80b6e31
P2SH Checksum:                09f2ae6a
P2SH Address Hex:             05f54a5851e9372b87810a8e60cdd2e7cfd80b6e3109f2ae6a
P2SH Address Base58:          3Q3zY87DrUmE371Grgc7bsDiVPqpu4mN1f
```

You can also generate an address using your own private key. The examples below show how to create an address using a key in Hexadecimal format and also one in WIF format.
If an invalid key is supplied then you will get a null BitcoinAddress object back with a string explaining why the key was invalid.
```c#
using BitcoinLib;
...
(BitcoinAddress addressFromHex, string resultFromHex) = BitcoinAddress.CreateAddressFromPrivateKeyHex("18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725", NetworkType.Main);
(BitcoinAddress addressFromWif, string resultFromWif) = BitcoinAddress.CreateAddressFromPrivateKeyWIF("Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C", NetworkType.Main);
if (addressFromHex == null)
{
	// Error generating address.
	Console.WriteLine(resultFromHex);
}
else
{
	Console.WriteLine(addressFromHex.AddressInfo());
}
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)