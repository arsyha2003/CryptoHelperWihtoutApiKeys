using BingX.Net.Clients;
using Bitget.Net.Clients;
using Bybit.Net.Clients;
using CoinEx.Net.Clients;
using GateIo.Net.Clients;
using Kucoin.Net.Clients;
using OKX.Net.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoHelper
{
    public class ExchangeAnalytic
    {
        private BybitRestClient bybitClient;
        private OKXRestClient okxClient;
        private GateIoRestClient gateIoClient;
        private BingXRestClient bingxClient;
        private KucoinRestClient kucoinClient;
        private CoinExRestClient coinexClient;
        private BitgetRestClient bitgetClient;
        public List<string> tokens;
        public int countOfPares = 0;
        public ExchangeAnalytic()
        {
            bybitClient = new BybitRestClient();
            okxClient = new OKXRestClient();
            gateIoClient = new GateIoRestClient();
            bingxClient = new BingXRestClient();
            kucoinClient = new KucoinRestClient();
            coinexClient = new CoinExRestClient();
            bitgetClient = new BitgetRestClient();
            tokens = new List<string>();
        }
        public async Task GetParesAsyncTask(Action<string> show)
        {
            await Task.Run(() =>
            {
                countOfPares = 0;
                tokens = new List<string>();
                try
                {
                    var coinexSymbols = coinexClient.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList(); show.Invoke($"Получен ответ от coinex");
                    var okxSymbols = okxClient.UnifiedApi.ExchangeData.GetSymbolsAsync(OKX.Net.Enums.InstrumentType.Spot).Result.Data.ToList(); show.Invoke($"Получен ответ от okx");
                    var kucoinSymbols = kucoinClient.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList(); show.Invoke($"Получен ответ от kucoin");
                    var bingXSymbols = bingxClient.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList(); show.Invoke($"Получен ответ от bingX");
                    var bybitSymbols = bybitClient.V5Api.ExchangeData.GetSpotSymbolsAsync().Result.Data.List; show.Invoke($"Получен ответ от bybit");
                    var gateIoSymbols = gateIoClient.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList(); show.Invoke($"Получен ответ от gateio");
                    var bitgetSymbols = bitgetClient.SpotApi.ExchangeData.GetSymbolsAsync().Result.Data.ToList(); show.Invoke($"Получен ответ от bitget");
                    foreach (var pare in bybitSymbols)
                    {
                        try
                        {
                            if (pare.Name.Contains("USDT") && !tokens.Contains(pare.Name)) tokens.Add(pare.Name); countOfPares++; show.Invoke($"Получено пар: {countOfPares}");
                        }
                        catch { continue; }
                    }
                    foreach (var pare in gateIoSymbols)
                    {
                        try
                        {
                            if (pare.Name.Replace("_USDT", "USDT").Contains("USDT") && !tokens.Contains(pare.Name.Replace("_USDT", "USDT"))) tokens.Add(pare.Name.Replace("_USDT", "USDT")); countOfPares++; show.Invoke($"Получено пар: {countOfPares}");

                        }
                        catch { continue; }
                    }
                    foreach (var pare in okxSymbols)
                    {
                        try
                        {
                            if (pare.BaseAsset.Contains("USDT") && !tokens.Contains(pare.BaseAsset + "USDT")) tokens.Add(pare.BaseAsset + "USDT"); countOfPares++; show.Invoke($"Получено пар: {countOfPares}");

                        }
                        catch { continue; }
                    }
                    foreach (var pare in bitgetSymbols)
                    {
                        try
                        {
                            if (pare.Name.Contains("USDT") && !tokens.Contains(pare.Name)) tokens.Add(pare.Name); countOfPares++; show.Invoke($"Получено пар: {countOfPares}");

                        }
                        catch { continue; }
                    }
                    foreach (var pare in coinexSymbols)
                    {
                        try
                        {
                            if (pare.Contains("USDT") && !tokens.Contains(pare)) tokens.Add(pare); countOfPares++; show.Invoke($"Получено пар: {countOfPares}");
                        }
                        catch { continue; }
                    }
                    foreach (var pare in bingXSymbols)
                    {
                        try
                        {
                            if (pare.Name.Replace("-", string.Empty).Contains("USDT") && !tokens.Contains(pare.Name.Replace("-", string.Empty))) tokens.Add(pare.Name.Replace("-", string.Empty)); countOfPares++; show.Invoke($"Получено пар: {countOfPares}");

                        }
                        catch { continue; }
                    }
                    foreach (var pare in kucoinSymbols)
                    {
                        try
                        {
                            if (pare.Name.Replace("-", string.Empty).Contains("USDT") && !tokens.Contains(pare.Name.Replace("-", string.Empty))) tokens.Add(pare.Name.Replace("-", string.Empty)); countOfPares++; show.Invoke($"Получено пар: {countOfPares}");

                        }
                        catch { continue; }
                    }
                }
                catch { }
                show.Invoke(string.Empty);
            });
        }
    }
}
