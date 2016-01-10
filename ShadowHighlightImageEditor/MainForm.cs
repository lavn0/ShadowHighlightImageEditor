using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShadowHighlightImageEditor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void 開くToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var image = Image.FromFile(openFileDialog.FileName);
                this.pictureBox1.Image?.Dispose();
                this.pictureBox1.Image = image;
            }
        }

        private void クリップボードの画像を読み込むToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var image = Clipboard.GetImage();
            if (image == null)
            {
                MessageBox.Show("クリップボードに画像がありません。");
                return;
            }

            this.pictureBox1.Image?.Dispose();
            this.pictureBox1.Image = image;
        }
    }
}
