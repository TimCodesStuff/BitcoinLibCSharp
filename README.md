# BitcoinLibCSharp

This C# library provides the ability to generate bitcoin addresses using a randomly generated private key or your own private key. Your own private key can be supplied in raw binary form, hexadecimal formal, or Wallet Input Format (WIF).

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