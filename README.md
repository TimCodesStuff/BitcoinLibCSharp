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

```c#
using BitcoinLib;
...
(BitcoinAddress address, string result) = BitcoinAddress.CreateAddressFromPrivateKeyHex("18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725", NetworkType.Main);
if (address == null)
{
	// Error generating address.
	Console.WriteLine(result);
}
else
{
	Console.WriteLine(address.AddressInfo());
}
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)