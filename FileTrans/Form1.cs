using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileTrans
{
    public partial class Form1 : Form
    {
        string IP = string.Empty;
        public Form1()
        {
            InitializeComponent();
            Scaler.Http http = new Scaler.Http();
            IP = http.GetHTML("http://oa.sdhuijiu.com/ip.aspx", "", "", "", "GET");
            lbListen.Text = IP + ":8205";
        }

        private void Btn_listen_Click(object sender, EventArgs e)
        {

        }

        private void btn_connect_Click(object sender, EventArgs e)
        {

        }
    }
}
