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
            Text = "�������� ������ �� ���� " + selectePare + " �� ����� " + selectedExchange + "...";
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
                        message += $"\n��������� ���� �������: {situation.bybit.asksPrice}\n";
                        message += $"\n��������� ���� �������: {situation.bybit.bidsPrice}\n";
                        message += $"\n������� ���� �������: {situation.bybit.avgBuyPrice}\n";
                        message += $"\n������� ���� �������: {situation.bybit.avgSellPrice}\n";
                        message += $"\nC��������������� ����� �������: {situation.bybit.askQuantity}\n";
                        message += $"\nC��������������� ����� �������: {situation.bybit.bidQuantity}\n";
                        break;
                    case "Binance":
                        message += $"\n��������� ���� �������: {situation.binance.asksPrice}\n";
                        message += $"\n��������� ���� �������: {situation.binance.bidsPrice}\n";
                        message += $"\n������� ���� �������: {situation.binance.avgBuyPrice}\n";
                        message += $"\n������� ���� �������: {situation.binance.avgSellPrice}\n";
                        message += $"\nC��������������� ����� �������: {situation.binance.askQuantity}\n";
                        message += $"\nC��������������� ����� �������: {situation.binance.bidQuantity}\n";
                        break;
                    case "Bitget":
                        message += $"��������� ���� �������: {situation.bitget.asksPrice}\n";
                        message += $"��������� ���� �������: {situation.bitget.bidsPrice}\n";
                        message += $"������� ���� �������: {situation.bitget.avgBuyPrice}\n";
                        message += $"������� ���� �������: {situation.bitget.avgSellPrice}\n";
                        message += $"C��������������� ����� �������: {situation.bitget.askQuantity}\n";
                        message += $"C��������������� ����� �������: {situation.bitget.bidQuantity}\n";
                        break;
                    case "Mexc":
                        message += $"\n��������� ���� �������: {situation.mexc.asksPrice}\n";
                        message += $"\n��������� ���� �������: {situation.mexc.bidsPrice}\n";
                        message += $"\n������� ���� �������: {situation.mexc.avgBuyPrice}\n";
                        message += $"\n������� ���� �������: {situation.mexc.avgSellPrice}\n";
                        message += $"\nC��������������� ����� �������: {situation.mexc.askQuantity}\n";
                        message += $"\nC��������������� ����� �������: {situation.mexc.bidQuantity}\n";
                        break;
                    case "GateIo":
                        message += $"\n��������� ���� �������: {situation.gateIo.asksPrice}\n";
                        message += $"\n��������� ���� �������: {situation.gateIo.bidsPrice}\n";
                        message += $"\n������� ���� �������: {situation.gateIo.avgBuyPrice}\n";
                        message += $"\n������� ���� �������: {situation.gateIo.avgSellPrice}\n";
                        message += $"\nC��������������� ����� �������: {situation.gateIo.askQuantity}\n";
                        message += $"\nC��������������� ����� �������: {situation.gateIo.bidQuantity}\n";
                        break;
                    case "Coinex":
                        message += $"\n��������� ���� �������: {situation.coinex.asksPrice}\n";
                        message += $"\n��������� ���� �������: {situation.coinex.bidsPrice}\n";
                        message += $"\n������� ���� �������: {situation.coinex.avgBuyPrice}\n";
                        message += $"\n������� ���� �������: {situation.coinex.avgSellPrice}\n";
                        message += $"\nC��������������� ����� �������: {situation.coinex.askQuantity}\n";
                        message += $"\nC��������������� ����� �������: {situation.coinex.bidQuantity}\n";
                        break;
                    case "Kucoin":
                        message += $"\n��������� ���� �������: {situation.kucoin.asksPrice}\n";
                        message += $"\n��������� ���� �������: {situation.kucoin.bidsPrice}\n";
                        message += $"\n������� ���� �������: {situation.kucoin.avgBuyPrice}\n";
                        message += $"\n������� ���� �������: {situation.kucoin.avgSellPrice}\n";
                        message += $"\nC��������������� ����� �������: {situation.kucoin.askQuantity}\n";
                        message += $"\nC��������������� ����� �������: {situation.kucoin.bidQuantity}\n";
                        break;
                    case "OKX":
                        message += $"\n��������� ���� �������: {situation.okx.asksPrice}\n";
                        message += $"\n��������� ���� �������: {situation.okx.bidsPrice}\n";
                        message += $"\n������� ���� �������: {situation.okx.avgBuyPrice}\n";
                        message += $"\n������� ���� �������: {situation.okx.avgSellPrice}\n";
                        message += $"\nC��������������� ����� �������: {situation.okx.askQuantity}\n";
                        message += $"\nC��������������� ����� �������: {situation.okx.bidQuantity}\n";
                        break;
                    case "BingX":
                        message += $"\n��������� ���� �������: {situation.bingX.asksPrice}\n";
                        message += $"\n��������� ���� �������: {situation.bingX.bidsPrice}\n";
                        message += $"\n������� ���� �������: {situation.bingX.avgBuyPrice}\n";
                        message += $"\n������� ���� �������: {situation.bingX.avgSellPrice}\n";
                        message += $"\nC��������������� ����� �������: {situation.bingX.askQuantity}\n";
                        message += $"\nC��������������� ����� �������: {situation.bingX.bidQuantity}\n";
                        break;
                }
                textBox3.Text = message;
            });
        }
        public async void AwaitPares(object sender, EventArgs e)
        {
            Text = "�������� ����";
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
