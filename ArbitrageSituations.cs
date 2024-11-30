using Binance.Net;
using BingX.Net.Clients;
using BingX.Net;
using Bitget.Net.Clients;
using Bitget.Net;
using Bybit.Net.Clients;
using Bybit.Net;
using CoinEx.Net.Clients;
using CoinEx.Net;
using GateIo.Net.Clients;
using GateIo.Net;
using Kucoin.Net.Clients;
using Kucoin.Net;
using Mexc.Net;
using OKX.Net.Clients;
using OKX.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoHelper
{
    class ArbitrageSituations
    {
        public BybitExchange bybit;
        public BinanceExchange binance;
        public OKXExchange okx;
        public BitGetExchange bitget;
        public MexcExchange mexc;
        public CoinExExchange coinex;
        public GateIoExchange gateIo;
        public BingXExchange bingX;
        public KucoinExchange kucoin;
        public ArbitrageSituations()
        {
            bybit = new BybitExchange();
            binance = new BinanceExchange();
            okx = new OKXExchange();
            bitget = new BitGetExchange();
            mexc = new MexcExchange();
            gateIo = new GateIoExchange();
            bingX = new BingXExchange();
            kucoin = new KucoinExchange();
        }
        public void GetApiData(string p)
        {
            Task[] tasks = new Task[]
            {
            Task.Run(() => bybit.GetOrderBook(p)),
            Task.Run(() => binance.GetOrderBook(p)),
            Task.Run(() => mexc.GetOrderBook(p)),
            Task.Run(() => bitget.GetOrderBook(p)),
            Task.Run(() => gateIo.GetOrderBook(p)),
            Task.Run(() => okx.GetOrderBook(p)),
            Task.Run(() => coinex.GetOrderBook(p)),
            Task.Run(() => bingX.GetOrderBook(p)),
            Task.Run(() => kucoin.GetOrderBook(p)),

            Task.Run(() => bybit.GetFeeRate(p)),
            Task.Run(() => binance.GetFeeRate(p)),
            Task.Run(() => mexc.GetFeeRate(p)),
            Task.Run(() => bitget.GetFeeRate(p)),
            Task.Run(() => gateIo.GetFeeRate(p)),
            Task.Run(() => okx.GetFeeRate(p)),
            Task.Run(() => coinex.GetFeeRate(p)),
            Task.Run(() => bingX.GetFeeRate(p)),
            Task.Run(() => kucoin.GetFeeRate(p)),
            };
            try
            {
                Task.WaitAll(tasks);
            }
            catch { }
        }
        public List<string> GetTokens()
        {
            var tokens = new List<string>();

            Random randomShuffleNumber = new Random();

            BybitRestClient bybitClient = new BybitRestClient();
            OKXRestClient okxClient = new OKXRestClient();
            GateIoRestClient gateIoClient = new GateIoRestClient();
            BingXRestClient bingxClient = new BingXRestClient();
            KucoinRestClient kucoinClient = new KucoinRestClient();
            CoinExRestClient coinexClient = new CoinExRestClient();
            BitgetRestClient bitgetClient = new BitgetRestClient();

            try
            {
                var coinexSymbols = coinexClient.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList();
                var okxSymbols = okxClient.UnifiedApi.ExchangeData.GetSymbolsAsync(OKX.Net.Enums.InstrumentType.Spot).Result.Data.ToList();
                var kucoinSymbols = kucoinClient.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList();
                var bingXSymbols = bingxClient.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList();
                var bybitSymbols = bybitClient.V5Api.ExchangeData.GetSpotSymbolsAsync().Result.Data.List;
                var gateIoSymbols = gateIoClient.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList();
                var bitgetSymbols = bitgetClient.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList();

                foreach (var pare in bybitSymbols) if (pare.Name.Contains("USDT") && !tokens.Contains(pare.Name)) tokens.Add(pare.Name);
                foreach (var pare in gateIoSymbols) if (pare.Name.Replace("_USDT", "USDT").Contains("USDT") && !tokens.Contains(pare.Name.Replace("_USDT", "USDT"))) tokens.Add(pare.Name.Replace("_USDT", "USDT"));
                foreach (var pare in okxSymbols) if (pare.BaseAsset.Contains("USDT") && !tokens.Contains(pare.BaseAsset + "USDT")) tokens.Add(pare.BaseAsset + "USDT");
                foreach (var pare in bitgetSymbols) if (pare.Name.Contains("USDT") && !tokens.Contains(pare.Name)) tokens.Add(pare.Name);
                foreach (var pare in coinexSymbols) if (pare.Contains("USDT") && !tokens.Contains(pare)) tokens.Add(pare);
                foreach (var pare in bingXSymbols) if (pare.Name.Replace("-", string.Empty).Contains("USDT") && !tokens.Contains(pare.Name.Replace("-", string.Empty))) tokens.Add(pare.Name.Replace("-", string.Empty));
                foreach (var pare in kucoinSymbols) if (pare.Name.Replace("-", string.Empty).Contains("USDT") && !tokens.Contains(pare.Name.Replace("-", string.Empty))) tokens.Add(pare.Name.Replace("-", string.Empty));

                List<string> shuffledTokensList = tokens.OrderBy(a => randomShuffleNumber.Next()).ToList();

                return shuffledTokensList;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return null;
        }
        public int GetBuyIndex(decimal[] buyPrices)
        {
            try
            {
                decimal buyPrice = 0;
                for (int i = 0; i < buyPrices.Length; i++)
                {
                    if (buyPrices[i] != 0)
                    {
                        buyPrice = buyPrices[i];
                        break;
                    }
                }
                for (int i = Array.IndexOf(buyPrices, buyPrice); i < buyPrices.Length; i++)
                {
                    if (buyPrice > buyPrices[i] && buyPrices[i] != 0) buyPrice = buyPrices[i];
                }
                if (buyPrice == 0) return -1;
                else return Array.IndexOf(buyPrices, buyPrice);
            }
            catch { return -1; }
        }
        public int GetSellIndex(decimal[] sellPrices)
        {
            try
            {
                decimal sellPrice = 0;
                for (int i = 0; i < sellPrices.Length; i++)
                {
                    if (sellPrices[i] != 0)
                    {
                        sellPrice = sellPrices[i];
                        break;
                    }
                }
                for (int i = Array.IndexOf(sellPrices, sellPrice); i < sellPrices.Length; i++)
                {
                    if (sellPrice < sellPrices[i] && sellPrices[i] != 0) sellPrice = sellPrices[i];
                }
                if (sellPrice == 0) return -1;
                else return Array.IndexOf(sellPrices, sellPrice);
            }
            catch { return -1; }
        }
        static decimal GetWithdrawalFeeRate(List<decimal> wFees)
        {
            try
            {
                decimal withdrawFee = 0;
                for (int i = 0; i < wFees.Count; i++)
                {
                    if (wFees[i] != 0)
                    {
                        withdrawFee = wFees[i];
                        break;
                    }
                }
                for (int i = 0; i < wFees.Count; i++)
                {
                    if (withdrawFee > wFees[i] && wFees[i] != 0)
                    {
                        withdrawFee = wFees[i];
                    }
                }
                return withdrawFee;
            }
            catch { return -1; }
        }

        public string GetArbitrageSituation(string p)
        {

            decimal tempQuant = 0;

            int buyIndex = 0;
            int sellIndex = 0;

            decimal fullFee = 0;
            decimal withdrawFee = 0;
            string chain = string.Empty;
            string chainFullName = string.Empty;
            decimal spread = 0;
            decimal profit = 0;
            decimal maxProfit = 0;

            string tradePlaceMin = string.Empty;
            string tradePlaceMax = string.Empty;

            decimal bidQuantity = 0;
            decimal askQuantity = 0;

            decimal avgBuyPrice = 0;
            decimal avgSellPrice = 0;

            decimal buyPrice = 0;
            decimal sellPrice = 0;

            List<decimal> wFees = new List<decimal>();

            decimal tFee = 0;
            decimal mFee = 0;

            List<string> firstChains = new List<string>();
            List<string> secondChains = new List<string>();
            List<string> chainFullName1 = new List<string>();
            List<string> chainFullName2 = new List<string>();
            List<bool> isWithdraw = new List<bool>();
            List<bool> isDepozit = new List<bool>();

            string buyLink;
            string sellLink;

            decimal depozit = 300;
            decimal tempDepozit = depozit;

            Console.WriteLine($"------------------{p}------------------");

            string[] names = new string[] { "Bybit", "Binance", "Mexc", "Bitget", "GateIo", "OKX", "CoinEx", "BingX", "Kucoin" };
            decimal[] asks = new decimal[] { bybit.asksPrice, binance.asksPrice, mexc.asksPrice, bitget.asksPrice, gateIo.asksPrice, okx.asksPrice, coinex.asksPrice, bingX.asksPrice, kucoin.asksPrice };
            decimal[] bids = new decimal[] { bybit.bidsPrice, binance.bidsPrice, mexc.bidsPrice, bitget.bidsPrice, gateIo.bidsPrice, okx.bidsPrice, coinex.bidsPrice, bingX.bidsPrice, kucoin.bidsPrice };
            decimal[] avgBuyPrices = new decimal[] { bybit.avgBuyPrice, binance.avgBuyPrice, mexc.avgBuyPrice, bitget.avgBuyPrice, gateIo.avgBuyPrice, okx.avgBuyPrice, coinex.avgBuyPrice, bingX.avgBuyPrice, kucoin.avgBuyPrice };
            decimal[] avgSellPrices = new decimal[] { bybit.avgSellPrice, binance.avgSellPrice, mexc.avgSellPrice, bitget.avgSellPrice, gateIo.avgSellPrice, okx.avgSellPrice, coinex.avgSellPrice, bingX.avgSellPrice, kucoin.avgSellPrice };
            decimal[] bidQuantities = new decimal[] { bybit.bidQuantity, binance.bidQuantity, mexc.bidQuantity, bitget.bidQuantity, gateIo.bidQuantity, okx.bidQuantity, coinex.bidQuantity, bingX.bidQuantity, kucoin.bidQuantity };
            decimal[] askQuantities = new decimal[] { bybit.askQuantity, binance.askQuantity, mexc.askQuantity, bitget.askQuantity, gateIo.askQuantity, okx.askQuantity, coinex.askQuantity, bingX.askQuantity, kucoin.askQuantity };
            decimal[] tFees = new decimal[] { bybit.takerFeeRate, binance.takerFeeRate, mexc.takerFeeRate, bitget.takerFeeRate, gateIo.takerFeeRate, okx.takerFeeRate, coinex.takerFeeRate, bingX.takerFeeRate, kucoin.takerFeeRate };
            decimal[] mFees = new decimal[] { bybit.makerFeeRate, binance.makerFeeRate, mexc.makerFeeRate, bitget.makerFeeRate, gateIo.makerFeeRate, okx.makerFeeRate, coinex.makerFeeRate, bingX.makerFeeRate, kucoin.makerFeeRate };
            List<decimal>[] fees = new List<decimal>[] { bybit.blockChainWithdrawFee, binance.blockChainWithdrawFee, mexc.blockChainWithdrawFee, bitget.blockChainWithdrawFee, gateIo.blockChainWithdrawFee, okx.blockChainWithdrawFee, coinex.blockChainWithdrawFee, bingX.blockChainWithdrawFee, kucoin.blockChainWithdrawFee };
            List<string>[] chains = new List<string>[] { bybit.blockChain, binance.blockChain, mexc.blockChain, bitget.blockChain, gateIo.blockChain, okx.blockChain, coinex.blockChain, bingX.blockChain, kucoin.blockChain };
            List<bool>[] isWithdrawable = new List<bool>[] { bybit.blockChainIsWithdrawable, binance.blockChainIsWithdrawable, mexc.blockChainIsWithdrawable, bitget.blockChainIsWithdrawable, gateIo.blockChainIsWithdrawable, okx.blockChainIsWithdrawable, coinex.blockChainIsWithdrawable, bingX.blockChainIsWithdrawable, kucoin.blockChainIsWithdrawable };
            List<bool>[] isDepozitable = new List<bool>[] { bybit.blockChainIsDepozitable, binance.blockChainIsDepozitable, mexc.blockChainIsDepozitable, bitget.blockChainIsDepozitable, gateIo.blockChainIsDepozitable, okx.blockChainIsDepozitable, coinex.blockChainIsDepozitable, bingX.blockChainIsDepozitable, kucoin.blockChainIsDepozitable };
            List<string>[] fullNames = new List<string>[] { bybit.blockChainFullName, binance.blockChainFullName, mexc.blockChainFullName, bitget.blockChainFullName, gateIo.blockChainFullName, okx.blockChainFullName, coinex.blockChainFullName, bingX.blockChainFullName, kucoin.blockChainFullName };
            for (int i = 0; i < fullNames.Length; i++)
            {
                Console.WriteLine($"{names[i]} {chains[i].Count}");
            }
            string[] links = new string[]
            {
            $"https://www.bybit.com/ru-RU/trade/spot/{p.Replace("USDT",string.Empty)}/USDT",
            $"https://www.binance.com/ru/trade/{p.Replace("USDT",string.Empty)}_USDT?type=spot",
            $"https://www.mexc.com/ru-RU/exchange/{p.Replace("USDT",string.Empty)}_USDT",
            $"https://www.bitget.com/ru/spot/{p}",
            $"https://www.gate.io/ru/trade/{p.Replace("USDT",string.Empty)}_USDT",
            $"https://www.okx.com/ru/trade-spot/{p.Replace("USDT",string.Empty).ToLower()}-usdt",
            $"https://www.coinex.com/en/exchange/{p.Replace("USDT",string.Empty).ToLower()}-usdt",
            $"https://bingx.com/en/spot/{p}/",
            $"https://www.kucoin.com/trade/{p.Replace("USDT",string.Empty)}-USDT"
            };

            sellIndex = GetSellIndex(avgSellPrices);
            buyIndex = GetBuyIndex(avgBuyPrices);

            if (sellIndex == -1 || buyIndex == -1) return string.Empty;

            try
            {
                avgBuyPrice = avgBuyPrices[buyIndex];
                buyPrice = asks[buyIndex];
                tradePlaceMin = names[buyIndex];
                bidQuantity = bidQuantities[buyIndex];
                tFee = tFees[buyIndex];
                firstChains = chains[buyIndex];
                wFees = fees[buyIndex];
                isWithdraw = isWithdrawable[buyIndex];
                chainFullName1 = fullNames[buyIndex];

                avgSellPrice = avgSellPrices[sellIndex];
                sellPrice = bids[sellIndex];
                tradePlaceMax = names[sellIndex];
                askQuantity = askQuantities[sellIndex];
                tFee = tFees[sellIndex];
                secondChains = chains[sellIndex];
                isDepozit = isDepozitable[sellIndex];
                chainFullName2 = fullNames[sellIndex];
            }
            catch { return string.Empty; }

            if (askQuantity == 0 || bidQuantity == 0) { return string.Empty; }
            if (tradePlaceMin == tradePlaceMax) { return string.Empty; }
            if (firstChains.Count == 0 || secondChains.Count == 0) { return string.Empty; }
            if (!isWithdraw.Contains(true) || !isDepozit.Contains(true)) { return string.Empty; }

            buyLink = $"<a href=\"{links[buyIndex]}\">{tradePlaceMin}</a>";
            sellLink = $"<a href=\"{links[sellIndex]}\">{tradePlaceMax}</a>";

            if (bidQuantity > askQuantity) tempQuant = askQuantity;
            else if (askQuantity > bidQuantity) tempQuant = bidQuantity;
            else if (askQuantity == bidQuantity) tempQuant = bidQuantity;


            withdrawFee = GetWithdrawalFeeRate(wFees);

            if (withdrawFee == -1) return string.Empty;

            if (tempQuant < depozit) depozit = tempQuant;
            else if (tempQuant > depozit) depozit = 300;

            try
            {
                spread = Math.Round(((avgSellPrice - avgBuyPrice) / avgSellPrice) * 100, 2);

                fullFee = Math.Abs(((depozit / avgBuyPrice) - ((depozit / avgBuyPrice) * tFee / 100) * avgSellPrice) * (mFee / 100) + (withdrawFee * avgBuyPrice));

                profit = Math.Round(((depozit / avgBuyPrice) * avgSellPrice - depozit) - fullFee, 4);

                maxProfit = Math.Round(((tempQuant / avgBuyPrice) * avgSellPrice - tempQuant) - fullFee, 4);
            }
            catch { return string.Empty; }

            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            if (spread >= (decimal)0.5 && spread <= 10 && maxProfit >= 1)
            {
                string message = string.Empty;
                try
                {
                    message += $"📊Пара: <b><u>{p.Replace("USDT", string.Empty)}/USDT📊</u></b>\n\n";
                    message += $"<b>{buyLink} --> {sellLink}</b>\n";
                    message += $"📈Покупка📈\n";
                    message += $"Объем: <b>{Math.Round(askQuantity, 8)}</b>\n";
                    message += $"💵Средняя цена ≈ <b>{avgBuyPrice} USDT</b>💵\n\n";

                    message += $"📉Продажа📉\n";
                    message += $"Объем: <b>{Math.Round(bidQuantity, 8)} USDT</b>\n";
                    message += $"💵Средняя цена ≈ <b>{avgSellPrice} USDT</b>💵\n\n";

                    message += $"(Деньги в сделке: {depozit}$)\n\n";

                    message += $"🔥<u>Спред</u>: <b>{spread} %</b>\n";
                    message += $"🔥<u>Профит с учетом комиссии</u>: <b>{profit} USDT</b> \n";
                    message += $"🔥<u>Максимально возможный профиt</u>: <b>{maxProfit} USDT</b> (Если торгуем на {tempQuant}) \n\n";
                    message += $"<u>❗️Комиссия сделки</u>: <b>{Math.Round(fullFee, 4)} USDT❗️</b> \n";
                    message += $"<b><u>⛓Сети⛓</u></b>\n";
                    if (firstChains.Count == 0 && secondChains.Count != 0)
                    {
                        message += $"<b>Сети на <u>{tradePlaceMin}</u></b>\n";
                        message += $"<b>❗️Доступных сетей нет❗️</b>\n";
                        message += $"<b>Сети на <u>{tradePlaceMax}</u></b>\n";
                        for (int i = 0; i < secondChains.Count; i++)
                        {
                            if (isDepozit[i] == true)
                            {
                                message += $"⚡️<u><b>{secondChains[i]}</b>, депозит есть🟢</u>\n";
                            }
                            else
                            {
                                message += $"⚡️<u><b>{secondChains[i]}</b>, депозита нет🔴</u>\n";
                            }

                        }
                    }
                    else if (firstChains.Count != 0 && secondChains.Count == 0)
                    {
                        message += $"<b>Сети на <u>{tradePlaceMin}</u></b>\n";
                        for (int i = 0; i < firstChains.Count; i++)
                        {
                            if (isWithdraw[i] == true)
                            {
                                message += $"⚡️<u><b>{firstChains[i]}</b>, комиссия <b>{wFees[i]} {myTI.ToTitleCase(p.Replace("USDT", string.Empty).ToLower())}</b>, вывод есть🟢</u>\n";
                            }
                            else
                            {
                                message += $"⚡️<u><b>{firstChains[i]}</b>, комиссия <b>{wFees[i]} {myTI.ToTitleCase(p.Replace("USDT", string.Empty).ToLower())}</b>, вывода нет🔴</u>\n";
                            }
                        }
                        message += $"<b>Сети на <u>{tradePlaceMax}</u></b>\n";
                        message += $"<b>❗️Доступных сетей нет❗️</b>\n";
                    }
                    else if (firstChains.Count == 0 && secondChains.Count == 0)
                    {
                        message += $"<b>Сети на <u>{tradePlaceMin}</u></b>\n";
                        message += $"<b>❗️Доступных сетей нет❗️</b>\n";
                        message += $"<b>Сети на <u>{tradePlaceMax}</u></b>\n";
                        message += $"<b>❗️Доступных сетей нет❗️</b>\n";
                    }
                    else if (firstChains.Count != 0 && secondChains.Count != 0)
                    {
                        message += $"<b>Сети на <u>{tradePlaceMin}</u></b>\n";
                        for (int i = 0; i < firstChains.Count; i++)
                        {
                            if (isWithdraw[i] == true)
                            {
                                message += $"⚡️<u><b>{firstChains[i]}</b>, комиссия <b>{wFees[i]} {myTI.ToTitleCase(p.Replace("USDT", string.Empty).ToLower())}</b>, вывод есть🟢</u>\n";
                            }
                            else
                            {
                                message += $"⚡️<u><b>{firstChains[i]}</b>, комиссия <b>{wFees[i]} {myTI.ToTitleCase(p.Replace("USDT", string.Empty).ToLower())}</b>, вывода нет🔴</u>\n";
                            }
                        }
                        message += $"<b>Сети на <u>{tradePlaceMax}</u></b>\n";
                        for (int i = 0; i < secondChains.Count; i++)
                        {
                            if (isWithdraw[i] == true)
                            {
                                message += $"⚡️<u><b>{secondChains[i]}</b>, депозит есть🟢</u>\n";
                            }
                            else
                            {
                                message += $"⚡️<u><b>{secondChains[i]}</b>, депозита нет🔴</u>\n";
                            }
                        }
                    }
                    message += $"\n<b>Наш банк: <u>{Math.Round(tempDepozit, 3)} --> {Math.Round(tempDepozit + profit, 3)} USDT</u></b>💵\n\n\n";
                    return message;
                }
                catch { return string.Empty; }
            }
            return string.Empty;
        }
    }
}
