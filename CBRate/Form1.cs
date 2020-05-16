//*****************************************************************************
//* CBRate version 1.00 - 2020-05-16                                          *
//*****************************************************************************

using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Windows.Forms;

namespace CBRate
{
    public partial class Form1 : Form
    {

        string cbRateURL = "";


        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // hide form
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.Location = new System.Drawing.Point(0, -10000);
            this.Icon = CBRate.Properties.Resources.chart;

            // load settings
            LoadConfiguration();
        }


        private void LoadConfiguration()
        {
            // from https://www.c-sharpcorner.com/article/four-ways-to-read-configuration-setting-in-c-sharp/
            var url = ConfigurationManager.AppSettings["url"];
            var currency = ConfigurationManager.AppSettings["currency"];
            var refreshRateInSeconds = ConfigurationManager.AppSettings["refreshRateInSeconds"];

            // cb url
            cbRateURL = url + currency;

            // start the timer
            TheTimer.Interval = Convert.ToInt32(refreshRateInSeconds) * 1000;
            TheTimer.Start();

            // show the rate for the first time before the timer kicks in
            UpdateRate();
        }


        private void TheTimer_Tick(object sender, EventArgs e)
        {
            // update the rate
            UpdateRate();
        }


        private void UpdateRate()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                // try to download json data
                try
                {
                    json_data = w.DownloadString(cbRateURL);
                }
                catch (Exception e) { }

                // if the json data is not empty
                if (!string.IsNullOrEmpty(json_data))
                {
                    // deserialize the json data
                    BtcJson btcJsonData = JsonConvert.DeserializeObject<BtcJson>(json_data);

                    // the currency sign
                    string sign = "";
                    if (btcJsonData.data.currency.ToUpper() == "EUR") { sign = "€"; }
                    if (btcJsonData.data.currency.ToUpper() == "USD") { sign = "$"; }

                    // the rate
                    double rateAsDouble = Convert.ToDouble(btcJsonData.data.amount.Replace('.', ','));
                    string rate = rateAsDouble.ToString("0.00").Replace(',', '.');

                    // display the rate
                    this.Text = sign + rate;
                }

            } // using

        }


    }
}
