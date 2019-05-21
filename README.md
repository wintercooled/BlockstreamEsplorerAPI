# BlockstreamEsplorerAPI

## A C# .NET Core wrapper for the [Blockstream Esplorer](https://github.com/Blockstream/esplora) API with full API coverage. Allows you to call Esplorer's [Bitcoin](https://github.com/bitcoin/bitcoin) (main and testnet) and [Liquid](https://blockstream.com/liquid/) API from within your project.

## Runs on Linux, Windows and Mac OS.

## An example of how to use the BlockstreamEsplorerAPI class within a .NET Core app can be found in Program.cs.

Note: If you want to actually use C# to issue commands to a Bitcoin or Elements/Liquid node itself (e.g. to create transactions, check wallet balance, issue assets etc) then check out the [dotnetcoreDynamicJSON-RPC](https://github.com/wintercooled/dotnetcoreDynamicJSON-RPC) code instead. As that uses dynamic types to send RPC commands, the code is brief and you can call whatever is listed here: https://en.bitcoin.it/wiki/Original_Bitcoin_client/API_calls_list#Full_list, and also the extra RPC calls used by Elements/Liquid, with little effort.

#### Easy to use:

~~~~
//Bitcoin
Esplorer.APITarget = APITarget.bitcoin;

long testBlock = Esplorer.Blocks_Tip_Height(); 
string blockHash = Esplorer.Block_Height(testBlock);

Block block = Esplorer.Block(blockHash);
Console.WriteLine(block.height);
~~~~

~~~~
//Liquid
Esplorer.APITarget = APITarget.liquid;

long testBlock = Esplorer.Blocks_Tip_Height(); 
string blockHash = Esplorer.Block_Height(testBlock);

Block block = Esplorer.Block(blockHash);

//Liquid specific field:
Proof proof = block.proof;
Console.WriteLine(proof.challenge);
~~~~

Examples of the Esplorer API:

[https://blockstream.info/api/blocks](https://blockstream.info/api/blocks)

[https://blockstream.info/liquid/api/blocks](https://blockstream.info/liquid/api/blocks)

[https://blockstream.info/testnet/api/blocks](https://blockstream.info/testnet/api/blocks)


The Blockstream Esplorer website:

[https://blockstream.info/](https://blockstream.info/)


### How to use BlockstreamEsplorerAPI in an existing project:

To use in your own project:

Copy **BlockstreamEsplorerAPI.cs** into your project.

Copy **BlockstreamEsplorerClasses.cs** into your project.

Reference the BlockstreamEsplorerAPI namepsace (using namespace BlockstreamEsplorerAPI;)

Reference the Newtonsoft.Json package in your .csproj file as done [here](https://github.com/wintercooled/BlockstreamEsplorerAPI/blob/master/BlockstreamEsplorerAPI.csproj).

You can switch to using Bitcoin testnet or Liquid using the Esplorer.APITarget property.

**Program.cs** contains a working example of all API calls using Esplorer's Bitcoin API.


### How to use it if you have not set up .NET Core already

**If you don't have the .Net Core SDK:**

The code targets version 2.1 of the .NET Core framework.

Register the Microsoft key and feed by choosing the Linux distribution you are using from the [dotnet download site](https://dotnet.microsoft.com/download/linux-package-manager/ubuntu18-04/sdk-current). It will give you the code to run and will look similar to this:

```
wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
```

Install the .NET SDK:

```
sudo add-apt-repository universe
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install dotnet-sdk-2.1
```

**Using Visual Studio Code:**

You don't need Visual Studio Code to edit the code, you can use any text editor. Visual Studio Code is a nice IDE and debugging in it is easy though, so it is easy to recommend. [https://code.visualstudio.com](https://code.visualstudio.com)

After installing Visual Studio Code you will need to add the C# language extension: 

Open Visual Studio Code and click the "Tools and languages" box on the welcome screen. Select C# from the available extensions (id: ms-vscode.csharp). 

Prerequisites and set up guides are listed and linked to here: [https://docs.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio-code](https://docs.microsoft.com/en-us/dotnet/core/tutorials/with-visual-studio-code)


**If you already have the .NET Core SDK and Visual Studio Code with C# set up:**

Note: The code targets version 2.1 of the .NET Core framework.

Clone this repository and then open the folder using Visual Studio Code's 'File/Open folder' option.

You will see two prompts:

"Required assets to build and debug are missing. Add them?"

- Click the 'Yes' buton.

"There are unresolved dependancies. Please execute the restore command to continue"

- Click the 'Restore' button.

**Visual Studio 2017 (Windows only):**

Clone this repository and then open the .csproj project file using Visual Studio's 'File/Open Project/Solution' option.

* * * 

Questions or Issues? [https://github.com/wintercooled/BlockstreamEsplorerAPI/issues](https://github.com/wintercooled/BlockstreamEsplorerAPI/issues)

I'm on Twitter: [https://twitter.com/wintercooled](https://twitter.com/wintercooled)
