using Telegram.Bot;

namespace CryptoHelper
{
    partial class Form1 : Form
    {
        public Action<string> showInfo;
        public Action<string> showCounter;
        public ExchangeAnalytic analyst;
        public ArbitrageSituations situation;
        public string selectedExchange = string.Empty;
        public string selectePare = string.Empty;
        public List<int> telegramUIDs;
        public Form1()
        {
            InitializeComponent();
            telegramUIDs = new List<int>();
            analyst = new ExchangeAnalytic();
            situation = new ArbitrageSituations();
            showCounter = (string message) => { label5.Text = message; };
        }
        public async void ShowInfoPerPare(object sender, EventArgs e)
        {
            Text = "Получаем данные по паре " + selectePare + " на бирже " + selectedExchange + "...";
            GetInfoPerPare();
        }
        public async Task GetInfoPerPare()
        {
            await Task.Run(() =>
            {
                string message = string.Empty;
                situation.GetApiData(selectePare);
                switch (selectedExchange)
                {
                    case "Bybit":
                        message += $"\nПоследняя цена покупки: {situation.bybit.asksPrice}\n";
                        message += $"\nПоследняя цена продажи: {situation.bybit.bidsPrice}\n";
                        message += $"\nСредняя цена покупки: {situation.bybit.avgBuyPrice}\n";
                        message += $"\nСредняя цена продажи: {situation.bybit.avgSellPrice}\n";
                        message += $"\nCредневзвешенный объем покупки: {situation.bybit.askQuantity}\n";
                        message += $"\nCредневзвешенный объем продажи: {situation.bybit.bidQuantity}\n";
                        break;
                    case "Binance":
                        message += $"\nПоследняя цена покупки: {situation.binance.asksPrice}\n";
                        message += $"\nПоследняя цена продажи: {situation.binance.bidsPrice}\n";
                        message += $"\nСредняя цена покупки: {situation.binance.avgBuyPrice}\n";
                        message += $"\nСредняя цена продажи: {situation.binance.avgSellPrice}\n";
                        message += $"\nCредневзвешенный объем покупки: {situation.binance.askQuantity}\n";
                        message += $"\nCредневзвешенный объем продажи: {situation.binance.bidQuantity}\n";
                        break;
                    case "Bitget":
                        message += $"Последняя цена покупки: {situation.bitget.asksPrice}\n";
                        message += $"Последняя цена продажи: {situation.bitget.bidsPrice}\n";
                        message += $"Средняя цена покупки: {situation.bitget.avgBuyPrice}\n";
                        message += $"Средняя цена продажи: {situation.bitget.avgSellPrice}\n";
                        message += $"Cредневзвешенный объем покупки: {situation.bitget.askQuantity}\n";
                        message += $"Cредневзвешенный объем продажи: {situation.bitget.bidQuantity}\n";
                        break;
                    case "Mexc":
                        message += $"\nПоследняя цена покупки: {situation.mexc.asksPrice}\n";
                        message += $"\nПоследняя цена продажи: {situation.mexc.bidsPrice}\n";
                        message += $"\nСредняя цена покупки: {situation.mexc.avgBuyPrice}\n";
                        message += $"\nСредняя цена продажи: {situation.mexc.avgSellPrice}\n";
                        message += $"\nCредневзвешенный объем покупки: {situation.mexc.askQuantity}\n";
                        message += $"\nCредневзвешенный объем продажи: {situation.mexc.bidQuantity}\n";
                        break;
                    case "GateIo":
                        message += $"\nПоследняя цена покупки: {situation.gateIo.asksPrice}\n";
                        message += $"\nПоследняя цена продажи: {situation.gateIo.bidsPrice}\n";
                        message += $"\nСредняя цена покупки: {situation.gateIo.avgBuyPrice}\n";
                        message += $"\nСредняя цена продажи: {situation.gateIo.avgSellPrice}\n";
                        message += $"\nCредневзвешенный объем покупки: {situation.gateIo.askQuantity}\n";
                        message += $"\nCредневзвешенный объем продажи: {situation.gateIo.bidQuantity}\n";
                        break;
                    case "Coinex":
                        message += $"\nПоследняя цена покупки: {situation.coinex.asksPrice}\n";
                        message += $"\nПоследняя цена продажи: {situation.coinex.bidsPrice}\n";
                        message += $"\nСредняя цена покупки: {situation.coinex.avgBuyPrice}\n";
                        message += $"\nСредняя цена продажи: {situation.coinex.avgSellPrice}\n";
                        message += $"\nCредневзвешенный объем покупки: {situation.coinex.askQuantity}\n";
                        message += $"\nCредневзвешенный объем продажи: {situation.coinex.bidQuantity}\n";
                        break;
                    case "Kucoin":
                        message += $"\nПоследняя цена покупки: {situation.kucoin.asksPrice}\n";
                        message += $"\nПоследняя цена продажи: {situation.kucoin.bidsPrice}\n";
                        message += $"\nСредняя цена покупки: {situation.kucoin.avgBuyPrice}\n";
                        message += $"\nСредняя цена продажи: {situation.kucoin.avgSellPrice}\n";
                        message += $"\nCредневзвешенный объем покупки: {situation.kucoin.askQuantity}\n";
                        message += $"\nCредневзвешенный объем продажи: {situation.kucoin.bidQuantity}\n";
                        break;
                    case "OKX":
                        message += $"\nПоследняя цена покупки: {situation.okx.asksPrice}\n";
                        message += $"\nПоследняя цена продажи: {situation.okx.bidsPrice}\n";
                        message += $"\nСредняя цена покупки: {situation.okx.avgBuyPrice}\n";
                        message += $"\nСредняя цена продажи: {situation.okx.avgSellPrice}\n";
                        message += $"\nCредневзвешенный объем покупки: {situation.okx.askQuantity}\n";
                        message += $"\nCредневзвешенный объем продажи: {situation.okx.bidQuantity}\n";
                        break;
                    case "BingX":
                        message += $"\nПоследняя цена покупки: {situation.bingX.asksPrice}\n";
                        message += $"\nПоследняя цена продажи: {situation.bingX.bidsPrice}\n";
                        message += $"\nСредняя цена покупки: {situation.bingX.avgBuyPrice}\n";
                        message += $"\nСредняя цена продажи: {situation.bingX.avgSellPrice}\n";
                        message += $"\nCредневзвешенный объем покупки: {situation.bingX.askQuantity}\n";
                        message += $"\nCредневзвешенный объем продажи: {situation.bingX.bidQuantity}\n";
                        break;
                }
                textBox3.Text = message;
            });
        }
        public async void AwaitPares(object sender, EventArgs e)
        {
            Text = "Получаем пары";
            analyst.GetParesAsyncTask(showCounter);
        }
        public async void UpdatePares(object sender, EventArgs e)
        {
            Text = analyst.countOfPares.ToString();
            comboBox2.Items.AddRange(analyst.tokens.ToArray());
        }
        public async void ChangeExchange(object sender, EventArgs e)
        {
            selectedExchange = comboBox1.SelectedItem as string;
            textBox1.Text = selectedExchange;
        }
        public async void ChangePare(object sender, EventArgs e)
        {
            selectePare = comboBox2.SelectedItem as string;
            textBox2.Text = selectePare;
        }
        public async void UpdateClassObjects()
        {
            situation.bybit = new BybitExchange();
            situation.binance = new BinanceExchange();
            situation.okx = new OKXExchange();
            situation.bitget = new BitGetExchange();
            situation.mexc = new MexcExchange();
            situation.coinex = new CoinExExchange();
            situation.gateIo = new GateIoExchange();
            situation.bingX = new BingXExchange();
            situation.kucoin = new KucoinExchange();
        }
        public async Task StartArbitrage()
        {
            await Task.Run(() =>
            {
                UpdateClassObjects();
                TelegramBotClient botClient = new TelegramBotClient("6334123432:AAG_O81CCRmk-WvxBgUGRgK1mr9a2PWAyQo");
                List<string> tokens = situation.GetTokens();
                while (true)
                {

                    foreach (string pare in tokens)
                    {
                        string message = string.Empty;
                        if (pare == "USDTUSDT") continue;
                        if (pare.Contains("BTC")) continue;
                        if (!pare.Contains("USDT")) continue;
                        UpdateClassObjects();
                        situation.GetApiData(pare);
                        message = situation.GetArbitrageSituation(pare);
                        if (message != string.Empty)
                        {
                            botClient.SendTextMessageAsync(-1002302745075, message, null, Telegram.Bot.Types.Enums.ParseMode.Html);
                        }
                    }
                }
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
