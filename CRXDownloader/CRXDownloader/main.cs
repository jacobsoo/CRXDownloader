using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;

namespace CRXDownloader
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            char[] delimiter = new char[] { '/' };
            char[] delimiter2 = new char[] { '?' };
            string szPath = txtURL.Text;
            string[] tokens = szPath.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            int iLen = tokens.Length;
            string[] szExtensionID = tokens[tokens.Length - 1].Split(delimiter2, StringSplitOptions.None);
            string szInput = "";
            string szVersion = "";
            using (WebClient client = new WebClient())
            {
                szInput = client.DownloadString(szPath);
            }
            string pattern = @"version\W content=\W(.*?)\W /";
            Match m = Regex.Match(szInput, pattern);
            while (m.Success)
            {
                szVersion = m.Groups[1].Value;
                m = m.NextMatch();
            }
            szVersion = szVersion.Replace('.', '_');
            string szDownloadPath = "https://clients2.google.com/service/update2/crx?response=redirect&prodversion=38.0&x=id%3D" + szExtensionID[0] + "%26installsource%3Dondemand%26uc";
            WebClient webClient = new WebClient();
            string szFile = tokens[tokens.Length-2] + "." +szVersion + ".crx";
            webClient.DownloadFile(szDownloadPath, szFile);
            MessageBox.Show("Download Completed.");
        }
    }
}
