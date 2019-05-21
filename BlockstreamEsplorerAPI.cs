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
        
        private static object CallAPI(string api, string postData = null)
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
                    return r;
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
            string result = CallAPI("block/" + blockHash).ToString();
            Block r = JsonConvert.DeserializeObject<Block>(result);
            return r;
        }

        //GET /block/:hash/status
        //TODO [ ] - could pass object type into CallAPI and let that handle deserialize - and null if string needed or custom filling
        public static BlockStatus Block_Status (string blockHash)
        {
            string result = CallAPI("block/" + blockHash + "/status").ToString();
            BlockStatus r = JsonConvert.DeserializeObject<BlockStatus>(result);
            return r;
        }

        //GET /block/:hash/txs[/:start_index]
        public static IList<Transaction> Block_Txs(string blockHash, long startIndex = 0)
        {
            string result = CallAPI("block/" + blockHash + "/txs/" + startIndex.ToString()).ToString();
            IList<Transaction> r = JsonConvert.DeserializeObject<IList<Transaction>>(result);
            return r;
        }

        //GET /block/:hash/txids
        public static IList<string> Block_TxIds(string blockHash)
        {
            string result = CallAPI("block/" + blockHash + "/txids").ToString();
            IList<string> r = JsonConvert.DeserializeObject<IList<string>>(result);
            return r;
        }

        //GET /block-height/:height
        public static string Block_Height(long blockHeight)
        {
            string result = CallAPI("block-height/" + blockHeight.ToString()).ToString();
            return result;
        }

        //GET /blocks[/:start_height]
        public static IList<Block> Blocks(long? startHeight = null)
        {
            string apiPath = "blocks";
            if (null != startHeight) {apiPath += "/" + startHeight.ToString();}
            string result = CallAPI(apiPath).ToString();
            IList<Block> r = JsonConvert.DeserializeObject<IList<Block>>(result);
            return r;
        }

        //GET /blocks/tip/height
        public static long Blocks_Tip_Height()
        {
            string result = CallAPI("blocks/tip/height").ToString();
            return long.Parse(result);
        }   

        //GET /blocks/tip/hash
        public static string Blocks_Tip_Hash()
        {
            string result = CallAPI("blocks/tip/hash").ToString();
            return result;
        }  


        // TRANSACTIONS
        /////////////////////////////////////////////////////
        
        //GET /tx/:txid
        public static Transaction Transaction(string txid)
        {
            string result = CallAPI("tx/" + txid).ToString();
            Transaction r = JsonConvert.DeserializeObject<Transaction>(result);
            return r;
        }

        //GET /tx/:txid/status
        public static Status Transaction_Status(string txid)
        {
            string result = CallAPI("tx/" + txid + "/status").ToString();
            Status r = JsonConvert.DeserializeObject<Status>(result);
            return r;
        }

        //GET /tx/:txid/hex
        public static string Transaction_Hex(string txid)
        {
            string result = CallAPI("tx/" + txid + "/hex").ToString();
            return result;
        }  

        //GET /tx/:txid/merkle-proof
        //Please see note here: https://github.com/Blockstream/esplora/blob/master/API.md
        // - "Will eventually be changed to use bitcoind's merkleblock format".
        public static MerkleProof Transaction_Merkle_Proof(string txid)
        {
            string result = CallAPI("tx/" + txid + "/merkle-proof").ToString();
            MerkleProof r = JsonConvert.DeserializeObject<MerkleProof>(result);
            return r;
        }

        //GET /tx/:txid/outspend/:vout
        public static Outspend Transaction_Outspend(string txid, long vout)
        {
            string result = CallAPI("tx/" + txid + "/outspend/" + vout.ToString()).ToString();
            Outspend r = JsonConvert.DeserializeObject<Outspend>(result);
            return r;
        }

        //GET /tx/:txid/outspends
        public static IList<Outspend> Transaction_Outspends(string txid)
        {
            string result = CallAPI("tx/" + txid + "/outspends").ToString();
            IList<Outspend> r = JsonConvert.DeserializeObject<IList<Outspend>>(result);
            return r;
        }

        //POST /tx
        public static string Post_Transaction(string rawTxHex)
        {
            string result = CallAPI("/tx", rawTxHex).ToString();
            return result;
        }
        

        // ADDRESSES
        /////////////////////////////////////////////////////
       
        //GET /address/:address
        public static Address Address(string address)
        {
            string result = CallAPI("address/" + address).ToString();
            Address r = JsonConvert.DeserializeObject<Address>(result);
            return r;
        }

        //GET /scripthash/:hash
        public static ScriptHash Scripthash(string scriptHash)
        {
            string result = CallAPI("scripthash/" + scriptHash).ToString();
            ScriptHash r = JsonConvert.DeserializeObject<ScriptHash>(result);
            return r;
        }

        //GET /address/:address/txs
        public static IList<Transaction> Address_Txs(string address)
        {
            string result = CallAPI("address/" + address + "/txs").ToString();
            IList<Transaction> r = JsonConvert.DeserializeObject<IList<Transaction>>(result);
            return r;
        }

        //GET /scripthash/:hash/txs
        public static IList<Transaction> Scripthash_Txs(string scriptHash)
        {
            string result = CallAPI("scripthash/" + scriptHash + "/txs").ToString();
            IList<Transaction> r = JsonConvert.DeserializeObject<IList<Transaction>>(result);
            return r;
        }

        //GET /address/:address/txs/chain[/:last_seen_txid]
        public static IList<Transaction> Address_Txs_Chain(string address, string lastSeenTxid = null)
        {
            string apiPath = "address/" + address + "/txs/chain";
            if (null != lastSeenTxid) {apiPath += "/" + lastSeenTxid;}
            string result = CallAPI(apiPath).ToString();
            IList<Transaction> r = JsonConvert.DeserializeObject<IList<Transaction>>(result);
            return r;
        }

        //GET /scripthash/:hash/txs/chain[/:last_seen_txid]
        public static IList<Transaction> Scripthash_Txs_Chain(string scriptHash, string lastSeenTxid = null)
        {
            string apiPath = "scripthash/" + scriptHash + "/txs/chain";
            if (null != lastSeenTxid) {apiPath += "/" + lastSeenTxid;}
            string result = CallAPI(apiPath).ToString();
            IList<Transaction> r = JsonConvert.DeserializeObject<IList<Transaction>>(result);
            return r;
        }

        //GET /address/:address/txs/mempool
        public static IList<Transaction> Address_Txs_Mempool(string address)
        {
            string result = CallAPI("address/" + address + "/txs/mempool").ToString();
            IList<Transaction> r = JsonConvert.DeserializeObject<IList<Transaction>>(result);
            return r;
        }

        //GET /scripthash/:hash/txs/mempool
        public static IList<Transaction> Scripthash_Txs_Mempool(string scriptHash)
        {
            string result = CallAPI("scripthash/" + scriptHash + "/txs/mempool").ToString();
            IList<Transaction> r = JsonConvert.DeserializeObject<IList<Transaction>>(result);
            return r;
        }

        //GET /address/:address/utxo
        public static IList<Utxo> Address_Utxo(string address)
        {
            string result = CallAPI("address/" + address + "/utxo").ToString();
            IList<Utxo> r = JsonConvert.DeserializeObject<IList<Utxo>>(result);
            return r;
        }

        //GET /scripthash/:hash/utxo
        public static IList<Utxo> Scripthash_Utxo(string scriptHash)
        {
            string result = CallAPI("scripthash/" + scriptHash + "/utxo").ToString();
            IList<Utxo> r = JsonConvert.DeserializeObject<IList<Utxo>>(result);
            return r;
        }


        // ADDRESSES
        /////////////////////////////////////////////////////
       
        //GET /mempool
        public static Mempool Mempool()
        {
            string result = CallAPI("mempool").ToString();
            Mempool r = JsonConvert.DeserializeObject<Mempool>(result);
            return r;
        }

        //GET /mempool/txids
        public static IList<string> Mempool_TxIds()
        {
            string result = CallAPI("mempool/txids").ToString();
            IList<string> r = JsonConvert.DeserializeObject<IList<string>>(result);
            return r;
        }

        //GET /mempool/recent
        public static IList<RecentTransaction> Mempool_Recent()
        {
            string result = CallAPI("mempool/recent").ToString();
            IList<RecentTransaction> r = JsonConvert.DeserializeObject<IList<RecentTransaction>>(result);
            return r;
        }
        

        // FEE ESTIMATES
        /////////////////////////////////////////////////////
        
        //GET /fee-estimates
        public static IList<FeeEstimate> Fee_Estimates()
        {
            string result = CallAPI("fee-estimates").ToString();
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