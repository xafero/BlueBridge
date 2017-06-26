using BlueBridgeNet.API;
using BlueBridgeNet.Json;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace BlueBridgeNet
{
    public partial class MainForm : Form
    {
        private UdpClient client;
        private IBridge bridge;
        private JsonSerializerSettings jsonCfg;

        public MainForm()
        {
            InitializeComponent();
            startBtn.Enabled = true;
            stopBtn.Enabled = false;
            quitBtn.Enabled = true;
        }

        private void quitBtn_Click(object sender, System.EventArgs e)
        {
            Close();
            bridge?.Dispose();
        }

        private void startBtn_Click(object sender, System.EventArgs e)
        {
            startBtn.Enabled = false;
            stopBtn.Enabled = true;
            bridge.Start();
        }

        private void stopBtn_Click(object sender, System.EventArgs e)
        {
            startBtn.Enabled = true;
            stopBtn.Enabled = false;
            bridge.Stop();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            var config = ConfigurationManager.AppSettings;
            var multiAddr = config["addr"];
            var multiPort = int.Parse(config["port"]);
            client = new UdpClient(multiAddr, multiPort)
            {
                EnableBroadcast = true
            };
            jsonCfg = JsonConfig.GetDefault();
            bridge = (new BridgeFactory()).Build();
            bridge.OnBlueEvent += Bridge_OnBlueEvent;
            if (bridge == null)
            {
                startBtn.Enabled = false;
                stopBtn.Enabled = false;
                MessageBox.Show("Could not find a bridge for your platform, sorry!");
            }
        }

        private void Bridge_OnBlueEvent(object sender, IBlueEvent e)
        {
            var bridge = (IBridge)sender;
            var advertise = e.Advertisement;
            var json = JsonConvert.SerializeObject(advertise, jsonCfg);
            var message = Encoding.UTF8.GetBytes(json);
            client.Send(message, message.Length);
        }
    }
}