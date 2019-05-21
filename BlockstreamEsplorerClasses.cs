using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// The latest version of this code can be found here:
// https://github.com/wintercooled/BlockstreamEsplorerAPI

namespace BlockstreamEsplorerAPI
{
    public class Block
    {
        public string id {get;set;} 
        public long height {get;set;}
        public long version {get; set;}
        public long timestamp {get;set;}
        public long tx_count {get;set;}
        public long size {get;set;}
        public long weight {get;set;}
        public string merkle_root {get;set;}
        public string previousblockhash {get;set;}
        public long nonce {get;set;}
        public long bits {get;set;}
        //Elements/Liquid only:
        public Proof proof {get;set;}
    }

    public class BlockStatus
    {
        public bool in_best_chain {get;set;}
        public long height {get;set;}
        public string next_best {get;set;}
    }

    public class Transaction
    {
        public string txid {get;set;}
        public long version {get;set;}
        public long locktime {get;set;}
        public long size {get;set;}
        public long weight {get;set;}
        public long? fee {get;set;}
        public Status status {get;set;}
        public IList<Vin> vin {get;set;}
        public IList<Vout> vout {get;set;} 
    }

    public class Vin
    {
        public string txid {get;set;}
        public long vout {get;set;}
        public string scriptsig {get;set;}
        public string scriptsig_asm {get;set;}
        public bool is_coinbase {get;set;}
        public long sequence {get;set;}
        public Vout prevout {get;set;}    
        public IList<string> witness {get;set;}  
        //Elements/Liquid only:
        public bool? is_pegin {get;set;}
        public Issuance issuance {get;set;}
    }

    public class Vout
    {
        public string scriptpubkey {get;set;}
        public string scriptpubkey_asm {get;set;}
        public string scriptpubkey_address {get;set;}
        public string scriptpubkey_type {get;set;}
        public long value {get;set;}
        //Elements/Liquid only:
        public string valuecommitment {get;set;}
        public string asset  {get;set;}
        public string assetcommitment {get;set;}
        public Pegout pegout {get;set;}
    }

    public class Status
    {
        public bool confirmed {get;set;}
        public long? block_height {get;set;}
        public string block_hash {get;set;}
        public long? block_time {get;set;}
    }

    public class MerkleProof
    {
        public long block_height {get;set;}
        public long pos {get;set;}
        public IList<string> merkle {get;set;}
    }

    public class Outspend
    {
        public bool spent {get;set;}
        public string txid {get;set;}
        public long? vin {get;set;}
        public Status status {get;set;}
    }

    public class Proof
    {
        public string challenge {get;set;}
        public string challenge_asm {get;set;}
        public string solution {get;set;}
        public string solution_asm {get;set;}
    }
    
    public class Issuance
    {
        public bool is_reissuance {get;set;}
        public string asset_blinding_nonce {get;set;}
        public string asset_entropy {get;set;}
        public decimal? assetamount {get;set;}
        public string assetamountcommitment {get;set;}
        public decimal? tokenamount {get;set;}
        public string tokenamountcommitment {get;set;}
    }

    public class Pegout
    {
        public string genesis_hash {get;set;}
        public string scriptpubkey {get;set;}
        public string scriptpubkey_asm {get;set;}
        public string scriptpubkey_address {get;set;}
    }

    public class Address
    {
        public string address {get;set;}
        public ChainStats chain_stats {get;set;}
        public MempoolStats mempool_stats {get;set;}
    }

    public class ScriptHash
    {
        public string scripthash {get;set;}
        public ChainStats chain_stats {get;set;}
        public MempoolStats mempool_stats {get;set;}
    }

    public class ChainStats
    {
        public long funded_txo_count {get;set;}
        public long? funded_txo_sum {get;set;}
        public long spent_txo_count {get;set;}
        public long? spent_txo_sum {get;set;}
        public long tx_count {get;set;}
    }

    public class MempoolStats : ChainStats{}

    public class Utxo
    {
        public string txid {get;set;}        
        public Status status {get;set;}
        public long vout {get;set;}
        public long value {get;set;}
        //Elements/Liquid only:
        public string asset {get;set;}
    }

    public class Mempool
    {
        public long count {get;set;}
        public long vsize {get;set;}
        public long total_fee {get;set;}
        public IList<FeeHistogram> fee_histogram {get;set;}
    }

    [JsonConverter(typeof(FeeHistogramConverter))]
    public class FeeHistogram
    {
        public decimal feerate {get;set;}
        public long vsize {get;set;}
    }

    public class FeeHistogramConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Name.Equals("FeeHistogram");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray array =  JArray.Load(reader);
            return new FeeHistogram { 
                feerate = array[0].ToObject<decimal>(),
                vsize = array[1].ToObject<long>()
            }; 
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var feeHistogram = value as FeeHistogram;
            JArray jArray = new JArray();
            jArray.Add(feeHistogram.feerate);
            jArray.Add(feeHistogram.vsize);
            jArray.WriteTo(writer);
        }
    }

    public class RecentTransaction
    {
        public string txid {get;set;}
        public long fee {get;set;}
        public long vsize {get;set;}
        public long value {get;set;}
    }
    
    public class FeeEstimate
    {
        public long confirmation_target {get;set;}
        public decimal estimated_feerate {get;set;}
    }
}