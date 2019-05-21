using System;
using System.Collections.Generic;

// The latest version of this code can be found here:
// https://github.com/wintercooled/BlockstreamEsplorerAPI

//API based upon 20190516 (last commit: 5f9e73c...)
//https://github.com/Blockstream/esplora/blob/master/API.md

namespace BlockstreamEsplorerAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            //  This code demonstrates the use of BlocksteamEsplorerAPI.cs to query the Blockstream Esplorer API.
            //  You can switch between bitcoin, liquid and bitcoin testnet using Esplorer.APITarget.
            //  The demo below uses bitcoin as the target API.

            //  Note: The addresses, txs etc used have no significance other than providing suitable test data.
            
            //  Developer notes: An exception will be raised if the API returns an HTTP Response Status Code error.

            //Switch API Target as needed
            // - Supports 'liquid' and 'testnet', default is 'bitcoin'
            // - Tesdata used in this demo is for bitcoin mainnet
            Esplorer.APITarget = APITarget.bitcoin;
            
            try
            {
                // BLOCKS
                /////////////////////////////////////////////////////
                
                // Blocks test parameters (bitcoin)...
                // A randomly selected block: (liquid block with pegout tx in: 2713)
                long testBlock = Esplorer.Blocks_Tip_Height() - 10; 

                //https://blockstream.info/api/block-height/576000
                string blockHash = Esplorer.Block_Height(testBlock);
            
                //https://blockstream.info/api/block/0000000000000000001b4ea439db0fc00026501b66df2e5ae3ead52b32828c46
                Block block = Esplorer.Block(blockHash);
                
                //https://blockstream.info/api/block/0000000000000000001b4ea439db0fc00026501b66df2e5ae3ead52b32828c46/status
                BlockStatus blockStatus = Esplorer.Block_Status(blockHash);
                
                //https://blockstream.info/api/block/0000000000000000001b4ea439db0fc00026501b66df2e5ae3ead52b32828c46/txs/25
                IList<Transaction> blockTransactions = Esplorer.Block_Txs(blockHash);

                //https://blockstream.info/api/block/0000000000000000001b4ea439db0fc00026501b66df2e5ae3ead52b32828c46/txids
                IList<string> blockTransactionIds = Esplorer.Block_TxIds(blockHash);

                //https://blockstream.info/api/blocks
                IList<Block> blocks = Esplorer.Blocks();

                //https://blockstream.info/api/blocks/tip/height
                long blocksTipHeight = Esplorer.Blocks_Tip_Height();

                //https://blockstream.info/api/blocks/tip/hash
                string blocksTipHash = Esplorer.Blocks_Tip_Hash();
                

                // TRANSACTIONS
                /////////////////////////////////////////////////////
                
                // Transactions test parameters (bitcoin)...
                // A transaction from our our randomly selected block:
                string txid = blockTransactionIds[blockTransactionIds.Count - 1];
                long vout = 0;

                //https://blockstream.info/api/tx/e0c50e93bebf5de47a4aff0b982e4b01fc514f55fd873e3bb68c6a75cde9b3e3
                Transaction transaction = Esplorer.Transaction(txid);

                //https://blockstream.info/api/tx/e0c50e93bebf5de47a4aff0b982e4b01fc514f55fd873e3bb68c6a75cde9b3e3/status
                Status transactionStatus = Esplorer.Transaction_Status(txid); 

                //https://blockstream.info/api/tx/e0c50e93bebf5de47a4aff0b982e4b01fc514f55fd873e3bb68c6a75cde9b3e3/hex
                string transactionHex = Esplorer.Transaction_Hex(txid);

                //https://blockstream.info/api/tx/e0c50e93bebf5de47a4aff0b982e4b01fc514f55fd873e3bb68c6a75cde9b3e3/merkle-proof
                MerkleProof merkleProof = Esplorer.Transaction_Merkle_Proof(txid);

                //https://blockstream.info/api/tx/e0c50e93bebf5de47a4aff0b982e4b01fc514f55fd873e3bb68c6a75cde9b3e3/outspend/1
                Outspend outspend = Esplorer.Transaction_Outspend(txid, vout);

                //https://blockstream.info/api/tx/e0c50e93bebf5de47a4aff0b982e4b01fc514f55fd873e3bb68c6a75cde9b3e3/outspends
                IList<Outspend> outspends = Esplorer.Transaction_Outspends(txid);

                //POST: https://blockstream.info/api/tx/your-raw-tx-hex-here
                //string postTxId = Esplorer.Post_Transaction("your-raw-tx-hex-here"); 

                
                // ADDRESSES
                /////////////////////////////////////////////////////
                
                // Addresses test parameters (bitcoin)...
                // A randomly selected address and associated scripthash:
                // (look at this to see how to get a scripthash: https://github.com/Blockstream/electrs/pull/7)
                string addressTest = "3H2jfVoiS4uSq6R7ExqhzSDxNjBBAqRqVy";
                string addressLastSeenTxIdTest = "24720d8e1c30f12c14b928fa98f82fb58ea977d5ae119e8dec44ed8fde0e4242";
                string scriptHashTest = "301d2d316ddca340604180fecc34b98cbcc13f96aa5d05aa3f846045db30ef0d";
                string scriptHashLastSeenTxIdTest = "540a7e54fd64478554519f1b2d643ecc888c5030631487f9cfc530b71d281309";

                //https://blockstream.info/api/address/3H2jfVoiS4uSq6R7ExqhzSDxNjBBAqRqVy
                Address address = Esplorer.Address(addressTest);
                
                //https://blockstream.info/api/scripthash/24720d8e1c30f12c14b928fa98f82fb58ea977d5ae119e8dec44ed8fde0e4242
                ScriptHash scriptHash = Esplorer.Scripthash(scriptHashTest);

                //https://blockstream.info/api/address/3H2jfVoiS4uSq6R7ExqhzSDxNjBBAqRqVy/txs
                IList<Transaction> addressTxs = Esplorer.Address_Txs(addressTest);

                //https://blockstream.info/api/scripthash/24720d8e1c30f12c14b928fa98f82fb58ea977d5ae119e8dec44ed8fde0e4242/txs
                IList<Transaction> scripthashTxs = Esplorer.Scripthash_Txs(scriptHashTest);

                //https://blockstream.info/api/address/3H2jfVoiS4uSq6R7ExqhzSDxNjBBAqRqVy/txs/chain/301d2d316ddca340604180fecc34b98cbcc13f96aa5d05aa3f846045db30ef0d
                IList<Transaction> addressTxsChain = Esplorer.Address_Txs_Chain(addressTest, addressLastSeenTxIdTest);

                //https://blockstream.info/api/scripthash/24720d8e1c30f12c14b928fa98f82fb58ea977d5ae119e8dec44ed8fde0e4242/txs/chain/540a7e54fd64478554519f1b2d643ecc888c5030631487f9cfc530b71d281309
                IList<Transaction> scriptHashTxsChain = Esplorer.Scripthash_Txs_Chain(scriptHashTest, scriptHashLastSeenTxIdTest);

                //https://blockstream.info/api/address/3H2jfVoiS4uSq6R7ExqhzSDxNjBBAqRqVy/txs/mempool
                IList<Transaction> addressTxsMempool = Esplorer.Address_Txs_Mempool(addressTest);

                //https://blockstream.info/api/scripthash/24720d8e1c30f12c14b928fa98f82fb58ea977d5ae119e8dec44ed8fde0e4242/txs/mempool
                IList<Transaction> scriptHashTxsMempool = Esplorer.Scripthash_Txs_Mempool(scriptHashTest);
                
                //https://blockstream.info/api/address/3H2jfVoiS4uSq6R7ExqhzSDxNjBBAqRqVy/utxo
                IList<Utxo> addressUtxo = Esplorer.Address_Utxo(addressTest);

                //https://blockstream.info/api/scripthash/24720d8e1c30f12c14b928fa98f82fb58ea977d5ae119e8dec44ed8fde0e4242/utxo
                IList<Utxo> scriptHashUtxo = Esplorer.Scripthash_Utxo(scriptHashTest);
                
                // MEMPOOL
                /////////////////////////////////////////////////////
                
                //https://blockstream.info/api/mempool
                Mempool mempool = Esplorer.Mempool();

                //https://blockstream.info/api/mempool/txids
                IList<string> mempoolTxIds = Esplorer.Mempool_TxIds();

                //https://blockstream.info/api/mempool/recent
                IList<RecentTransaction> recentTxs = Esplorer.Mempool_Recent();


                // FEE ESITMATES
                /////////////////////////////////////////////////////
                
                //https://blockstream.info/api/fee-estimates
                IList<FeeEstimate> feeEstimates = Esplorer.Fee_Estimates();

                Console.WriteLine("Demo completed with no errors");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
    }
}