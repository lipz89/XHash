﻿using System;
using System.Drawing;
using System.Windows.Forms;
using XHashGen;

namespace XHashChecker
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            this.txtPublicKey.TextChanged += Txt_TextChanged;
            this.txtHash.TextChanged += Txt_TextChanged;
        }

        private void Txt_TextChanged(object sender, EventArgs e)
        {
            lblResult.Text = null;
            lblResult.BackColor = this.BackColor;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            var input = this.txtPublicKey.Text.Trim();
            var hash = this.txtHash.Text.Trim();

            var result = XHash.Validate(input, hash);
            if (result)
            {
                lblResult.Text = "验证成功。";
                lblResult.BackColor = Color.Chartreuse;
            }
            else
            {
                lblResult.Text = "验证失败！";
                lblResult.BackColor = Color.Pink;
            }
        }
    }
}
