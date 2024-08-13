using System.Net.Http.Headers;
using Timer = System.Windows.Forms.Timer;

namespace Pkcs11TestWinForms
{
    public partial class Form1 : Form
    {
        readonly HttpClient _client = new();
        private readonly Timer _timer;

        public Form1()
        {
            InitializeComponent();

            _timer = new Timer();
            _timer.Tick += TimerOnTick;
            _timer.Interval = 100;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnInitialize.Enabled = true;
            btnStart.Enabled = false;
            btnStop.Enabled = false;
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            _client.BaseAddress = new Uri(txtURL.Text);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            btnInitialize.Enabled = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private async void btnExecute_Click(object sender, EventArgs e)
        {
            await Execute();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            _timer.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _timer.Stop();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private async void TimerOnTick(object sender, EventArgs e)
        {
            await Execute();
        }

        private async Task Execute()
        {
            var response = await _client.GetAsync("api/generateKeyPair");
            var id = await response.Content.ReadAsStringAsync();
            txtLog.AppendText("OK " + id + Environment.NewLine);
        }
    }
}
