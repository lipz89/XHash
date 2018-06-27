using System;
using System.Drawing;
using System.Windows.Forms;

namespace XHashChecker
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            this.txtPublicKey.TextChanged += Txt_TextChanged;
            this.txtTick.TextChanged += Txt_TextChanged;
            this.txtHash.TextChanged += Txt_TextChanged;
        }

        private void Txt_TextChanged(object sender, EventArgs e)
        {
            lblResult.Text = null;
            lblResult.BackColor = this.BackColor;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            var publicKey = this.txtPublicKey.Text.Trim();
            var tick = this.txtTick.Text.Trim();
            var hash = this.txtHash.Text.Trim();

            var result = XHash.ValidateAuthorize(tick, publicKey, hash);
            if (result)
            {
                lblResult.Text = "验证成功。";
                lblResult.BackColor = Color.Chartreuse;
            }
            else
            {
                lblResult.Text = "验证失败，请检查算法。";
                lblResult.BackColor = Color.Pink;
            }
        }
    }
}
