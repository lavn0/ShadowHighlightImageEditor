using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShadowHighlightImageEditor
{
    public partial class MainForm : Form
    {
        private readonly Brush grayBrush = new SolidBrush(Color.FromArgb(0x55, 0x00, 0x00, 0x00));
        private Point? pointStart;
        private Point? pointEnd;

        public MainForm()
        {
            InitializeComponent();
            this.pictureBox2.Parent = this.pictureBox1;
        }

        private void 開くToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var image = Image.FromFile(openFileDialog.FileName);
                this.pictureBox1.Image?.Dispose();
                this.pictureBox1.Image = image;
                this.pictureBox2.Image?.Dispose();
                this.pictureBox2.Image = new Bitmap(image.Width, image.Height);
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
            this.pictureBox2.Image?.Dispose();
            this.pictureBox2.Image = new Bitmap(image.Width, image.Height);
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.pictureBox2.Capture = true;
                this.pointStart = new Point(e.X, e.Y);
                this.pointEnd = new Point(e.X, e.Y);
                this.pictureBox2.Refresh();
            }
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.pictureBox2.Capture)
            {
                this.pointEnd = new Point(e.X, e.Y);
                this.pictureBox2.Refresh();
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (this.pointStart.HasValue && this.pointEnd.HasValue)
            {
                var path = new GraphicsPath(FillMode.Alternate);
                path.AddRectangle(new Rectangle(0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height));
                path.AddRectangle(CreateRectangle(this.pointStart.Value, this.pointEnd.Value));
                e.Graphics.FillPath(grayBrush, path);
            }
        }

        private static Rectangle CreateRectangle(Point point1, Point point2)
        {
            return new Rectangle(
                Math.Min(point1.X, point2.X),
                Math.Min(point1.Y, point2.Y),
                Math.Abs(point1.X - point2.X),
                Math.Abs(point1.Y - point2.Y));
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.pictureBox1.Image == null)
            {
                MessageBox.Show("元画像が読み込まれていません。");
                return;
            }

            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var bmp = new Bitmap(this.pictureBox1.Image))
                using (var g = Graphics.FromImage(bmp))
                {
                    var path = new GraphicsPath(FillMode.Alternate);
                    path.AddRectangle(new Rectangle(0, 0, this.pictureBox1.Image.Width, this.pictureBox1.Image.Height));
                    path.AddRectangle(CreateRectangle(this.pointStart.Value, this.pointEnd.Value));
                    g.FillPath(this.grayBrush, path);
                    bmp.Save(this.saveFileDialog.FileName);
                }
            }
        }
    }
}
