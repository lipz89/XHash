using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TestHashGen
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            var publicKey = this.txtPublicKey.Text.Trim();
            var hash = Run(publicKey);
            this.txtHash.Text = hash;
        }

        private string Run(string publicKey)
        {
            return ExecuteDos("HashGen.exe", publicKey);
        }


        private static string ExecuteDos(string exe, params string[] args)
        {
            var cmd = exe + " " + string.Join(" ", args);
            return ExecuteDos(cmd);
        }

        private static string RemoveStart(string text, string start)
        {
            if (text.StartsWith(start))
            {
                text = text.Substring(start.Length);
            }
            return text.Trim();
        }
        private static string RemoveEnd(string text, string end)
        {
            if (text.EndsWith(end))
            {
                text = text.Substring(0, text.Length - end.Length);
            }
            return text.Trim();
        }
        private static string ExecuteDos(string cmd)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.StandardInput.WriteLine("@echo off");
                process.StandardInput.WriteLine(cmd);
                process.StandardInput.WriteLine("exit");
                string text = process.StandardOutput.ReadToEnd();
                text = text.Substring(text.IndexOf(cmd, StringComparison.Ordinal));
                return RemoveEnd(RemoveStart(text, cmd), "exit");
            }
        }
    }
}
