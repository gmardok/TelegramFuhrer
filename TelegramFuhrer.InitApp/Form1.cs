using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TLSharp.Core;

namespace TelegramFuhrer.InitApp
{
    public partial class Form1 : Form
    {
        private static string ApiHash => ConfigurationManager.AppSettings["ApiHash"];

        private static int ApiId => int.Parse(ConfigurationManager.AppSettings["ApiId"]);

        private TelegramClient _client;

        private string _hash;

        private string _phone;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnGetCode_Click(object sender, EventArgs e)
        {
            txtPhone.Enabled = false;
            _client = new TelegramClient(ApiId, ApiHash);
            await _client.ConnectAsync();
            _phone = txtPhone.Text;
            if (!_phone.StartsWith("+")) _phone = "+" + _phone;
            _hash = await _client.SendCodeRequestAsync(_phone);
            btnRegister.Enabled = true;
            btnGetCode.Enabled = false;
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            await _client.MakeAuthAsync(_phone, _hash, txtCode.Text);
            txtPhone.Enabled = true;
            btnRegister.Enabled = false;
            btnGetCode.Enabled = true;
        }
    }
}
