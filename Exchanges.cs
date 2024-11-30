using Binance.Net.Clients;
using BingX.Net.Clients;
using Bitget.Net.Clients;
using Bybit.Net.Clients;
using CoinEx.Net.Clients;
using GateIo.Net.Clients;
using HTX.Net.Clients;
using Kucoin.Net.Clients;
using Mexc.Net.Clients;
using OKX.Net.Clients;

using CryptoExchange.Net.Authentication;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoHelper
{
    abstract class Exchange
    {
        public decimal bidsPrice { get; set; } = 0;
        public decimal avgBuyPrice { get; set; } = 0;
        public decimal asksPrice { get; set; } = 0;
        public decimal avgSellPrice { get; set; } = 0;
        public decimal bidQuantity { get; set; } = 0;
        public decimal askQuantity { get; set; } = 0;
        public decimal takerFeeRate { get; set; }
        public decimal makerFeeRate { get; set; }
        public List<decimal> blockChainWithdrawFee { get; set; }
        public List<string> blockChainFullName { get; set; }
        public List<string> blockChain { get; set; }
        public List<bool> blockChainIsWithdrawable { get; set; }
        public List<bool> blockChainIsDepozitable { get; set; }
        public abstract void GetOrderBook(string pareName);
        public abstract void GetFeeRate(string pareName);
        public Exchange()
        {
            avgSellPrice = 0;
            avgBuyPrice = 0;
            bidsPrice = 0;
            asksPrice = 0;
            bidQuantity = 0;
            askQuantity = 0;
        }
    }
    class BybitExchange : Exchange
    {
        const string api = "";
        const string apiSecret = "";

        private BybitRestClient client;
        public BybitExchange() : base()
        {
            client = new BybitRestClient(options => { options.ApiCredentials = new ApiCredentials(api, apiSecret); });
            takerFeeRate = (decimal)0.18;
            makerFeeRate = (decimal)0.1;
        }
        public override void GetFeeRate(string pareName)
        {
            blockChainFullName = new List<string>();
            blockChainIsDepozitable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            blockChain = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            try
            {
                var assetInfo = client.V5Api.Account.GetAssetInfoAsync().Result;
                if (assetInfo.Success)
                {
                    foreach (var asset in assetInfo.Data.Assets.ToList())
                    {
                        if (asset.Asset == pareName.Replace("USDT", string.Empty))
                        {
                            foreach (var network in asset.Networks.ToList())
                            {
                                blockChainFullName.Add(network.Network);
                                blockChain.Add(network.Network);
                                blockChainWithdrawFee.Add((decimal)network.WithdrawFee);
                                blockChainIsWithdrawable.Add((bool)network.NetworkWithdraw);
                                blockChainIsDepozitable.Add((bool)network.NetworkDeposit);
                            }
                        }
                    }
                }
                else return;
            }
            catch { }
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            var orderBook = client.V5Api.ExchangeData.GetOrderbookAsync(Bybit.Net.Enums.Category.Spot, pareName, 5).Result;
            if (orderBook.Success)
            {
                var bids = orderBook.Data.Bids.ToList();
                var asks = orderBook.Data.Asks.ToList();
                List<decimal> tmpBuy = new List<decimal>();
                List<decimal> tmpSell = new List<decimal>();
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        bidQuantity += bids[i].Quantity * bids[i].Price;
                        tmpSell.Add(bids[i].Price);
                    }
                    avgSellPrice = tmpSell.Average();
                    bidsPrice = bids[0].Price;
                }
                catch { bidsPrice = 0; bidQuantity = 0; avgBuyPrice = 0; }
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        askQuantity += asks[i].Quantity * asks[i].Price;
                        tmpBuy.Add(asks[i].Price);
                    }
                    avgBuyPrice = tmpBuy.Average();
                    asksPrice = asks[0].Price;
                }
                catch { asksPrice = 0; askQuantity = 0; avgSellPrice = 0; }
            }
            else
            {
                bidsPrice = 0;
                bidQuantity = 0;
                asksPrice = 0;
                askQuantity = 0;
                avgBuyPrice = 0;
                avgSellPrice = 0;
            }
        }
    }
    class MexcExchange : Exchange
    {
        const string api = "";
        const string apiSecret = "";
        private MexcRestClient client;
        public MexcExchange() : base()
        {
            takerFeeRate = (decimal)0.01;
            makerFeeRate = (decimal)0;
            client = new MexcRestClient(options => { options.ApiCredentials = new ApiCredentials(api, apiSecret); });
        }
        public override void GetFeeRate(string pareName)
        {
            blockChainFullName = new List<string>();
            blockChainIsDepozitable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            blockChain = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            try
            {
                var networkList = client.SpotApi.Account.GetUserAssetsAsync().Result;
                if (networkList.Success)
                {
                    foreach (var network in networkList.Data.ToList())
                    {
                        if (network.Asset == pareName.Replace("USDT", string.Empty))
                        {
                            foreach (var a in network.Networks.ToList())
                            {
                                blockChainFullName.Add(a.Network);
                                blockChain.Add(a.Network);
                                blockChainWithdrawFee.Add((decimal)a.WithdrawFee);
                                blockChainIsWithdrawable.Add(a.WithdrawEnabled);
                                blockChainIsDepozitable.Add(a.DepositEnabled);
                            }
                        }
                    }
                }
                else return;
            }
            catch { }
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName, 5).Result;

            if (orderBook.Success)
            {
                var bids = orderBook.Data.Bids.ToList();
                var asks = orderBook.Data.Asks.ToList();
                List<decimal> tmpBuy = new List<decimal>();
                List<decimal> tmpSell = new List<decimal>();
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        bidQuantity += bids[i].Quantity * bids[i].Price;
                        tmpSell.Add(bids[i].Price);
                    }
                    avgSellPrice = tmpSell.Average();
                    bidsPrice = bids[0].Price;
                }
                catch { bidsPrice = 0; bidQuantity = 0; avgBuyPrice = 0; }
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        askQuantity += asks[i].Quantity * asks[i].Price;
                        tmpBuy.Add(asks[i].Price);
                    }
                    avgBuyPrice = tmpBuy.Average();
                    asksPrice = asks[0].Price;
                }
                catch { asksPrice = 0; askQuantity = 0; avgSellPrice = 0; }
            }
            else
            {
                bidsPrice = 0;
                bidQuantity = 0;
                asksPrice = 0;
                askQuantity = 0;
                avgBuyPrice = 0;
                avgSellPrice = 0;
            }
        }
    }
    class BinanceExchange : Exchange
    {
        const string api = "";
        const string apiSecret = "";

        private BinanceRestClient client;

        public BinanceExchange()
        {
            client = new BinanceRestClient(options => { options.ApiCredentials = new ApiCredentials(api, apiSecret); });
            makerFeeRate = (decimal)0.015;
            takerFeeRate = (decimal)0.015;
        }

        public override void GetFeeRate(string pareName)
        {
            blockChainFullName = new List<string>();
            blockChainIsDepozitable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            blockChain = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            var assetInfo = client.SpotApi.Account.GetUserAssetsAsync().Result;
            try
            {
                if (assetInfo.Success)
                {
                    foreach (var asset in assetInfo.Data.ToList())
                    {
                        if (asset.Asset == pareName.Replace("USDT", string.Empty))
                        {
                            foreach (var network in asset.NetworkList)
                            {
                                blockChainFullName.Add(network.Network);
                                blockChain.Add(network.Network.ToUpper());
                                blockChainWithdrawFee.Add(network.WithdrawFee);
                                blockChainIsWithdrawable.Add(network.WithdrawEnabled);
                                blockChainIsDepozitable.Add(network.DepositEnabled);

                            }
                        }
                    }
                }
            }
            catch { }

        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName, 5).Result;
            if (orderBook.Success)
            {
                var bids = orderBook.Data.Bids.ToList();
                var asks = orderBook.Data.Asks.ToList();
                List<decimal> tmpBuy = new List<decimal>();
                List<decimal> tmpSell = new List<decimal>();
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        bidQuantity += bids[i].Quantity * bids[i].Price;
                        tmpSell.Add(bids[i].Price);
                    }
                    avgSellPrice = tmpSell.Average();
                    bidsPrice = bids[0].Price;
                }
                catch { bidsPrice = 0; bidQuantity = 0; avgBuyPrice = 0; }
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        askQuantity += asks[i].Quantity * asks[i].Price;
                        tmpBuy.Add(asks[i].Price);
                    }
                    avgBuyPrice = tmpBuy.Average();
                    asksPrice = asks[0].Price;
                }
                catch { asksPrice = 0; askQuantity = 0; avgSellPrice = 0; }
            }
            else
            {
                bidsPrice = 0;
                bidQuantity = 0;
                asksPrice = 0;
                askQuantity = 0;
                avgBuyPrice = 0;
                avgSellPrice = 0;
            }
        }
    }
    class OKXExchange : Exchange
    {
        private OKXRestClient client;
        const string api = "";
        const string apiSecret = "";
        public OKXExchange()
        {
            client = new OKXRestClient(options => { options.ApiCredentials = new OKX.Net.Objects.OKXApiCredentials(api, apiSecret, "Arbitop1)"); });
            makerFeeRate = (decimal)0.02;
            takerFeeRate = (decimal)0.05;
        }
        public override void GetFeeRate(string pareName)
        {
            blockChainFullName = new List<string>();
            blockChainIsDepozitable = new List<bool>();
            blockChain = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            try
            {
                var assetInfo = client.UnifiedApi.Account.GetAssetsAsync().Result;
                if (assetInfo.Success)
                {
                    foreach (var asset in assetInfo.Data.ToList())
                    {
                        if (asset.Asset == pareName.Replace("USDT", string.Empty))
                        {
                            blockChain.Add(asset.Network.Split('-')[1].ToUpper().Replace(" ", string.Empty));
                            blockChainFullName.Add(asset.Network.Split('-')[1]);
                            blockChainIsWithdrawable.Add(asset.AllowWithdrawal);
                            blockChainIsDepozitable.Add(asset.AllowDeposit);
                            blockChainWithdrawFee.Add(asset.MinimumWithdrawalFee);
                        }
                    }
                }
            }
            catch { }
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            var orderBook = client.UnifiedApi.ExchangeData.GetOrderBookAsync(pareName.Replace("USDT", string.Empty) + "-USDT", 5).Result;
            Console.WriteLine(orderBook.Success);
            if (orderBook.Success)
            {
                var bids = orderBook.Data.Bids.ToList();
                var asks = orderBook.Data.Asks.ToList();
                List<decimal> tmpBuy = new List<decimal>();
                List<decimal> tmpSell = new List<decimal>();
                try
                {

                    for (int i = 0; i < 5; i++)
                    {
                        bidQuantity += bids[i].Quantity * bids[i].Price;
                        tmpSell.Add(bids[i].Price);
                    }
                    avgSellPrice = tmpSell.Average();
                    bidsPrice = bids[0].Price;
                }
                catch { bidsPrice = 0; bidQuantity = 0; avgBuyPrice = 0; }
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        askQuantity += asks[i].Quantity * asks[i].Price;
                        tmpBuy.Add(asks[i].Price);
                    }
                    avgBuyPrice = tmpBuy.Average();
                    asksPrice = asks.First().Price;
                }
                catch { asksPrice = 0; askQuantity = 0; avgSellPrice = 0; }
            }
            else
            {
                bidsPrice = 0;
                bidQuantity = 0;
                asksPrice = 0;
                askQuantity = 0;
                avgBuyPrice = 0;
                avgSellPrice = 0;
            }
        }
    }
    class BitGetExchange : Exchange
    {
        private BitgetRestClient client;

        public BitGetExchange()
        {
            client = new BitgetRestClient();
            makerFeeRate = (decimal)0.1;
            takerFeeRate = (decimal)0.1;
        }
        public override void GetFeeRate(string pareName)
        {
            blockChainFullName = new List<string>();
            blockChainIsDepozitable = new List<bool>();
            blockChain = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            try
            {
                var assetInfo = client.SpotApi.ExchangeData.GetAssetsAsync().Result;
                if (assetInfo.Success)
                {
                    foreach (var asset in assetInfo.Data.ToList())
                    {
                        if (asset.AssetName == pareName.Replace("USDT", string.Empty))
                        {
                            foreach (var network in asset.Networks)
                            {
                                blockChainFullName.Add(network.Name);
                                blockChain.Add(network.Name.ToUpper());
                                blockChainIsWithdrawable.Add(network.Withdrawable);
                                blockChainIsDepozitable.Add(network.Depositable);
                                blockChainWithdrawFee.Add(network.WithdrawFee);
                            }
                        }
                    }
                }
                else return;
            }
            catch { }
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName + "_SPBL", null, 5).Result;
            if (orderBook.Success)
            {
                var bids = orderBook.Data.Bids.ToList();
                var asks = orderBook.Data.Asks.ToList();
                List<decimal> tmpBuy = new List<decimal>();
                List<decimal> tmpSell = new List<decimal>();
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        bidQuantity += bids[i].Quantity * bids[i].Price;
                        tmpSell.Add(bids[i].Price);
                    }
                    avgSellPrice = tmpSell.Average();
                    bidsPrice = bids[0].Price;
                }
                catch { bidsPrice = 0; bidQuantity = 0; avgBuyPrice = 0; }
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        askQuantity += asks[i].Quantity * asks[i].Price;
                        tmpBuy.Add(asks[i].Price);
                    }
                    avgBuyPrice = tmpBuy.Average();
                    asksPrice = asks[0].Price;
                }
                catch { asksPrice = 0; askQuantity = 0; avgSellPrice = 0; }
            }
            else
            {
                bidsPrice = 0;
                bidQuantity = 0;
                asksPrice = 0;
                askQuantity = 0;
                avgBuyPrice = 0;
                avgSellPrice = 0;
            }
        }
    }
    class GateIoExchange : Exchange
    {
        private GateIoRestClient client;
        const string api = "";
        const string apiSecret = "";

        public GateIoExchange()
        {
            client = new GateIoRestClient(options => { options.ApiCredentials = new ApiCredentials(api, apiSecret); });
            makerFeeRate = (decimal)0.015;
            takerFeeRate = (decimal)0.05;
        }

        public override void GetFeeRate(string pareName)
        {
            blockChainFullName = new List<string>();
            blockChainIsDepozitable = new List<bool>();
            blockChain = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            try
            {
                var networkList = client.SpotApi.ExchangeData.GetNetworksAsync(pareName.Replace("USDT", string.Empty)).Result;
                var withdrawFee = client.SpotApi.Account.GetWithdrawStatusAsync(pareName.Replace("USDT", string.Empty)).Result;

                if (networkList.Success && withdrawFee.Success)
                {
                    foreach (var network in networkList.Data.ToList())
                    {
                        blockChain.Add(network.Network);
                        blockChainFullName.Add(network.NetworkEn);
                        blockChainIsWithdrawable.Add(!network.IsWithdrawalDisabled);
                        blockChainIsDepozitable.Add(!network.IsDepositDisabled);
                        blockChainWithdrawFee.Add(withdrawFee.Data.ToList().First().WithdrawalFeeFixed);
                    }
                }
                else return;
            }
            catch { }
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName.Replace("USDT", "_") + "USDT", 0, 5).Result;
            if (orderBook.Success)
            {
                var bids = orderBook.Data.Bids.ToList();
                var asks = orderBook.Data.Asks.ToList();
                List<decimal> tmpBuy = new List<decimal>();
                List<decimal> tmpSell = new List<decimal>();
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        bidQuantity += bids[i].Quantity * bids[i].Price;
                        tmpSell.Add(bids[i].Price);
                    }
                    avgSellPrice = tmpSell.Average();
                    bidsPrice = bids[0].Price;
                }
                catch { bidsPrice = 0; bidQuantity = 0; avgBuyPrice = 0; }
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        askQuantity += asks[i].Quantity * asks[i].Price;
                        tmpBuy.Add(asks[i].Price);
                    }
                    avgBuyPrice = tmpBuy.Average();
                    asksPrice = asks[0].Price;
                }
                catch { asksPrice = 0; askQuantity = 0; avgSellPrice = 0; }
            }
            else
            {
                bidsPrice = 0;
                bidQuantity = 0;
                asksPrice = 0;
                askQuantity = 0;
                avgBuyPrice = 0;
                avgSellPrice = 0;
            }

        }
    }
    class CoinExExchange : Exchange
    {
        private CoinExRestClient client;
        public CoinExExchange()
        {
            client = new CoinExRestClient();
            makerFeeRate = (decimal)0.3;
            takerFeeRate = (decimal)0.3;
        }
        public override void GetFeeRate(string pareName)
        {
            blockChainFullName = new List<string>();
            blockChainIsDepozitable = new List<bool>();
            blockChain = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();

            var assetInfo = client.SpotApi.ExchangeData.GetAssetsAsync(pareName.Replace("USDT", string.Empty)).Result;
            if (assetInfo.Success)
            {
                foreach (var asset in assetInfo.Data.ToList())
                {
                    if (asset.Value.WithdrawFee != 0)
                    {
                        blockChainFullName.Add(asset.Value.Network);
                        blockChain.Add(asset.Value.Network.ToUpper());
                        blockChainIsWithdrawable.Add(asset.Value.CanWithdraw);
                        blockChainWithdrawFee.Add(asset.Value.WithdrawFee);
                        blockChainIsDepozitable.Add(asset.Value.CanDeposit);
                    }
                }
            }
            else return;
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName, 0, 20).Result;
            if (orderBook.Success)
            {
                var bids = orderBook.Data.Bids.ToList();
                var asks = orderBook.Data.Asks.ToList();
                List<decimal> tmpBuy = new List<decimal>();
                List<decimal> tmpSell = new List<decimal>();
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        bidQuantity += bids[i].Quantity * bids[i].Price;
                        tmpSell.Add(bids[i].Price);
                    }
                    avgSellPrice = tmpSell.Average();
                    bidsPrice = bids[0].Price;
                }
                catch { bidsPrice = 0; bidQuantity = 0; avgBuyPrice = 0; }
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        askQuantity += asks[i].Quantity * asks[i].Price;
                        tmpBuy.Add(asks[i].Price);
                    }
                    avgBuyPrice = tmpBuy.Average();
                    asksPrice = asks[0].Price;
                }
                catch { asksPrice = 0; askQuantity = 0; avgSellPrice = 0; }
            }
            else
            {
                bidsPrice = 0;
                bidQuantity = 0;
                asksPrice = 0;
                askQuantity = 0;
                avgBuyPrice = 0;
                avgSellPrice = 0;
            }
        }
    }
    class BingXExchange : Exchange
    {
        const string api = "";
        const string apiSecret = "";


        private BingXRestClient client;
        public BingXExchange() : base()
        {
            client = new BingXRestClient(options => { options.ApiCredentials = new ApiCredentials(api, apiSecret); });
            takerFeeRate = (decimal)0.1;
            makerFeeRate = (decimal)0.1;
        }
        public override void GetFeeRate(string pareName)
        {
            blockChainFullName = new List<string>();
            blockChainIsDepozitable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            blockChain = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            try
            {
                var assetInfo = client.SpotApi.Account.GetAssetsAsync().Result;
                if (assetInfo.Success)
                {
                    foreach (var asset in assetInfo.Data.ToList())
                    {
                        if (asset.Asset == pareName.Replace("USDT", string.Empty))
                        {
                            foreach (var network in asset.Networks)
                            {
                                blockChain.Add(network.Network.ToUpper());
                                blockChainFullName.Add(network.Network);
                                blockChainIsWithdrawable.Add(network.WithdrawEnabled);
                                blockChainWithdrawFee.Add(network.WithdrawFee);
                                blockChainIsDepozitable.Add(network.DepositEnabled);
                            }
                        }
                    }
                }
                else return;
            }
            catch { }
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            var orderBook = client.SpotApi.ExchangeData.GetOrderBookAsync(pareName.Replace("USDT", string.Empty) + "-USDT", 5).Result;
            if (orderBook.Success)
            {
                var bids = orderBook.Data.Bids.ToList();
                var asks = orderBook.Data.Asks.ToList();
                List<decimal> tmpBuy = new List<decimal>();
                List<decimal> tmpSell = new List<decimal>();
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        bidQuantity += bids[i].Quantity * bids[i].Price;
                        tmpSell.Add(bids[i].Price);
                    }
                    avgSellPrice = tmpSell.Average();
                    bidsPrice = bids[0].Price;
                }
                catch { bidsPrice = 0; bidQuantity = 0; avgBuyPrice = 0; }
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        askQuantity += asks[i].Quantity * asks[i].Price;
                        tmpBuy.Add(asks[i].Price);
                    }
                    avgBuyPrice = tmpBuy.Average();
                    asksPrice = asks[0].Price;
                }
                catch { asksPrice = 0; askQuantity = 0; avgSellPrice = 0; }
            }
            else
            {
                bidsPrice = 0;
                bidQuantity = 0;
                asksPrice = 0;
                askQuantity = 0;
                avgBuyPrice = 0;
                avgSellPrice = 0;
            }
        }
    }
    class KucoinExchange : Exchange
    {
        private KucoinRestClient client;
        public KucoinExchange()
        {
            client = new KucoinRestClient();
            takerFeeRate = (decimal)0.16;
            makerFeeRate = (decimal)0.24;
        }
        public override void GetFeeRate(string pareName)
        {
            blockChainIsDepozitable = new List<bool>();
            blockChainWithdrawFee = new List<decimal>();
            blockChain = new List<string>();
            blockChainFullName = new List<string>();
            blockChainIsWithdrawable = new List<bool>();
            try
            {
                var assetInfo = client.SpotApi.ExchangeData.GetAssetAsync(pareName.Replace("USDT", string.Empty)).Result;
                if (assetInfo.Success)
                {
                    foreach (var network in assetInfo.Data.Networks)
                    {
                        blockChain.Add(network.NetworkName.ToUpper());
                        blockChainFullName.Add(network.NetworkName);
                        blockChainIsDepozitable.Add(network.IsDepositEnabled);
                        blockChainIsWithdrawable.Add(network.IsWithdrawEnabled);
                        blockChainWithdrawFee.Add((decimal)network.WithdrawalMinFee);
                    }
                }
                else return;
            }
            catch { }
        }
        public override void GetOrderBook(string pareName)
        {
            askQuantity = 0;
            bidQuantity = 0;
            avgBuyPrice = 0;
            avgSellPrice = 0;
            try
            {
                var orderBook = client.SpotApi.ExchangeData.GetAggregatedPartialOrderBookAsync(pareName.Replace("USDT", string.Empty) + "-USDT", 20).Result;
                if (orderBook.Success)
                {
                    var bids = orderBook.Data.Bids.ToList();
                    var asks = orderBook.Data.Asks.ToList();
                    List<decimal> tmpBuy = new List<decimal>();
                    List<decimal> tmpSell = new List<decimal>();
                    try
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            bidQuantity += bids[i].Quantity * bids[i].Price;
                            tmpSell.Add(bids[i].Price);
                        }
                        avgSellPrice = tmpSell.Average();
                        bidsPrice = bids[0].Price;
                    }
                    catch { bidsPrice = 0; bidQuantity = 0; avgBuyPrice = 0; }
                    try
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            askQuantity += asks[i].Quantity * asks[i].Price;
                            tmpBuy.Add(asks[i].Price);
                        }
                        avgBuyPrice = tmpBuy.Average();
                        asksPrice = asks[0].Price;
                    }
                    catch { asksPrice = 0; askQuantity = 0; avgSellPrice = 0; }
                }
                else
                {
                    bidsPrice = 0;
                    bidQuantity = 0;
                    asksPrice = 0;
                    askQuantity = 0;
                    avgBuyPrice = 0;
                    avgSellPrice = 0;
                }
            }
            catch { }

        }
    }
}
