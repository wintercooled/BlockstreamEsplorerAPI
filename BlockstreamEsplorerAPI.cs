using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

//  Overview:
//    A C# wrapper for the Blockstream Esplorer API for Bitcoin, Liquid and Bitcoin testnet
//      - https://github.com/Blockstream/esplora
//      - https://blockstream.info/api/blocks
//      - https://blockstream.info/liquid/api/blocks
//      - https://blockstream.info/testnet/api/blocks
//      - https://blockstream.info/

//  Developer notes: 
//    An exception will be raised if there is an error raised by the API response.
//    I have used long over a mix of int and long in the response objects (e.g. Block)
//    as the performance v time to analyse each/risk of me using a value that currently
//    fits into an int not fitting in the future isn't big enough to warrant an int/long mix.

//  Versioning:
//    The latest version of this code can be found here:
//      - https://github.com/wintercooled/BlockstreamEsplorerAPI
//    The API is based upon https://github.com/Blockstream/esplora/blob/master/API.md
//    as of 20190516 (last commit: 5f9e73c...).

namespace BlockstreamEsplorerAPI
{
    public enum APITarget { bitcoin, liquid, testnet };
    
    static class Esplorer
    {
        public static APITarget APITarget = APITarget.bitcoin;
        
        private static object CallAPI(string api, Type returnType, string postData = null)
        {
            string apiUrl = "https://blockstream.info/";

            string jsonResponse = string.Empty;
            
            switch (APITarget)
            {
                case APITarget.bitcoin:
                    apiUrl += "api/";
                    break;

                case APITarget.liquid:
                    apiUrl += "liquid/api/";
                    break;

                case APITarget.testnet:
                    apiUrl += "testnet/api/";
                    break;
            }

            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(apiUrl);
                
                HttpResponseMessage response;

                if (null != postData)
                {
                    var content = new StringContent(postData);
                    response = client.PostAsync(api, content).Result;
                }
                else // Default is GET
                {
                    response = client.GetAsync(api).Result;
                }
                
                string r = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    //JsonConvert.DeserializeObject sometimes tries to cast a string to a numeric so we need to work around this.
                    if (typeof(string) == returnType)
                    {
                        return r;
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject(r, returnType);
                    }
                }
                else
                {
                    throw new Exception(r);
                }
            }
        }
        

        //BLOCKS
        /////////////////////////////////////////////////////
        
        //GET /block/:hash
        public static Block Block (string blockHash)
        {
            return (Block)CallAPI("block/" + blockHash, typeof(Block));
        }

        //GET /block/:hash/status
        public static BlockStatus Block_Status (string blockHash)
        {
            return (BlockStatus)CallAPI("block/" + blockHash + "/status",typeof(BlockStatus));
        }

        //GET /block/:hash/txs[/:start_index]
        public static IList<Transaction> Block_Txs(string blockHash, long startIndex = 0)
        {
            return (IList<Transaction>)CallAPI("block/" + blockHash + "/txs/" + startIndex.ToString(), typeof(IList<Transaction>));
        }

        //GET /block/:hash/txids
        public static IList<string> Block_TxIds(string blockHash)
        {
            return (IList<string>)CallAPI("block/" + blockHash + "/txids", typeof(IList<string>));
        }

        //GET /block-height/:height
        public static string Block_Height(long blockHeight)
        {
            return (string)CallAPI("block-height/" + blockHeight.ToString(), typeof(string));
        }

        //GET /blocks[/:start_height]
        public static IList<Block> Blocks(long? startHeight = null)
        {
            string apiPath = "blocks";
            if (null != startHeight) {apiPath += "/" + startHeight.ToString();}
            return (IList<Block>)CallAPI(apiPath, typeof(IList<Block>));
        }

        //GET /blocks/tip/height
        public static long Blocks_Tip_Height()
        {
            return (long)CallAPI("blocks/tip/height", typeof(long));
        }   

        //GET /blocks/tip/hash
        public static string Blocks_Tip_Hash()
        {
            return (string)CallAPI("blocks/tip/hash", typeof(string));
        }  


        // TRANSACTIONS
        /////////////////////////////////////////////////////
        
        //GET /tx/:txid
        public static Transaction Transaction(string txid)
        {
            return (Transaction)CallAPI("tx/" + txid, typeof(Transaction));
        }

        //GET /tx/:txid/status
        public static Status Transaction_Status(string txid)
        {
            return (Status)CallAPI("tx/" + txid + "/status", typeof(Status));
        }

        //GET /tx/:txid/hex
        public static string Transaction_Hex(string txid)
        {
            return (string)CallAPI("tx/" + txid + "/hex", typeof(string));
        }  

        //GET /tx/:txid/merkle-proof
        //Please see note here: https://github.com/Blockstream/esplora/blob/master/API.md
        // - "Will eventually be changed to use bitcoind's merkleblock format".
        public static MerkleProof Transaction_Merkle_Proof(string txid)
        {
            return (MerkleProof)CallAPI("tx/" + txid + "/merkle-proof", typeof(MerkleProof));
        }

        //GET /tx/:txid/outspend/:vout
        public static Outspend Transaction_Outspend(string txid, long vout)
        {
            return (Outspend)CallAPI("tx/" + txid + "/outspend/" + vout.ToString(), typeof(Outspend));
        }

        //GET /tx/:txid/outspends
        public static IList<Outspend> Transaction_Outspends(string txid)
        {
            return (IList<Outspend>)CallAPI("tx/" + txid + "/outspends",typeof(IList<Outspend>));
        }

        //POST /tx
        public static string Post_Transaction(string rawTxHex)
        {
            return (string)CallAPI("/tx", typeof(string), rawTxHex);
        }
        

        // ADDRESSES
        /////////////////////////////////////////////////////
       
        //GET /address/:address
        public static Address Address(string address)
        {
            return (Address)CallAPI("address/" + address, typeof(Address));
        }

        //GET /scripthash/:hash
        public static ScriptHash Scripthash(string scriptHash)
        {
            return (ScriptHash)CallAPI("scripthash/" + scriptHash, typeof(ScriptHash));
        }

        //GET /address/:address/txs
        public static IList<Transaction> Address_Txs(string address)
        {
            return (IList<Transaction>)CallAPI("address/" + address + "/txs", typeof(IList<Transaction>));
        }

        //GET /scripthash/:hash/txs
        public static IList<Transaction> Scripthash_Txs(string scriptHash)
        {
            return (IList<Transaction>)CallAPI("scripthash/" + scriptHash + "/txs", typeof(IList<Transaction>));
        }

        //GET /address/:address/txs/chain[/:last_seen_txid]
        public static IList<Transaction> Address_Txs_Chain(string address, string lastSeenTxid = null)
        {
            string apiPath = "address/" + address + "/txs/chain";
            if (null != lastSeenTxid) {apiPath += "/" + lastSeenTxid;}
            return (IList<Transaction>)CallAPI(apiPath, typeof(IList<Transaction>));
        }

        //GET /scripthash/:hash/txs/chain[/:last_seen_txid]
        public static IList<Transaction> Scripthash_Txs_Chain(string scriptHash, string lastSeenTxid = null)
        {
            string apiPath = "scripthash/" + scriptHash + "/txs/chain";
            if (null != lastSeenTxid) {apiPath += "/" + lastSeenTxid;}
            return (IList<Transaction>)CallAPI(apiPath, typeof(IList<Transaction>));
        }

        //GET /address/:address/txs/mempool
        public static IList<Transaction> Address_Txs_Mempool(string address)
        {
            return (IList<Transaction>)CallAPI("address/" + address + "/txs/mempool", typeof(IList<Transaction>));
        }

        //GET /scripthash/:hash/txs/mempool
        public static IList<Transaction> Scripthash_Txs_Mempool(string scriptHash)
        {
            return (IList<Transaction>)CallAPI("scripthash/" + scriptHash + "/txs/mempool", typeof(IList<Transaction>));
        }

        //GET /address/:address/utxo
        public static IList<Utxo> Address_Utxo(string address)
        {
            return (IList<Utxo>)CallAPI("address/" + address + "/utxo", typeof(IList<Utxo>));
        }

        //GET /scripthash/:hash/utxo
        public static IList<Utxo> Scripthash_Utxo(string scriptHash)
        {
            return (IList<Utxo>)CallAPI("scripthash/" + scriptHash + "/utxo", typeof(IList<Utxo>));
        }


        // ADDRESSES
        /////////////////////////////////////////////////////
       
        //GET /mempool
        public static Mempool Mempool()
        {
            return (Mempool)CallAPI("mempool", typeof(Mempool));
        }

        //GET /mempool/txids
        public static IList<string> Mempool_TxIds()
        {
            return (IList<string>)CallAPI("mempool/txids", typeof(IList<string>));
        }

        //GET /mempool/recent
        public static IList<RecentTransaction> Mempool_Recent()
        {
            return (IList<RecentTransaction>)CallAPI("mempool/recent", typeof(IList<RecentTransaction>));
        }
        

        // FEE ESTIMATES
        /////////////////////////////////////////////////////
        
        //GET /fee-estimates
        public static IList<FeeEstimate> Fee_Estimates()
        {
            string result = (string)CallAPI("fee-estimates", typeof(string));

            var resultObject = JObject.Parse(result);

            IList<FeeEstimate> feeEstimates = new List<FeeEstimate>();

            foreach (var estimate in resultObject)
            {
                FeeEstimate feeEstimate = new FeeEstimate{ confirmation_target = Convert.ToInt64(estimate.Key), estimated_feerate = Convert.ToDecimal(estimate.Value) };
                feeEstimates.Add(feeEstimate);
            }

            return feeEstimates.OrderBy(f=>f.confirmation_target).ToList();
        }
    }
}