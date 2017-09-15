using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataLogger
{
    [DefaultEvent("Click")]
    public partial class FlatButton : UserControl
    {
        public FlatButton()
        {
            this.ColorBoderActive = Color.Green;
            this.ColorBoderHover = Color.Gray;
            this.ColorBoderNormal = Color.Silver;
            this.BoderWidthNormal = 1;
            this.BoderWidthActive = 2;
            this.IsActive = false;
            InitializeComponent();
        }

        public Color ColorBoderNormal { get; set; }
        public Color ColorBoderHover { get; set; }
        public Color ColorBoderActive { get; set; }
        public int BoderWidthNormal { get; set; }
        public int BoderWidthActive { get; set; }

        private bool _isActive = false;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                this.Invalidate();
            }
        }

        private bool isHover = false;

        public string ToolTipHint
        {
            get
            {
                return toolTipToolItem.GetToolTip(this);
            }
            set
            {
                toolTipToolItem.SetToolTip(this, value);
            }
        }

        public void FlatButton_Refresh()
        {
            if (_isActive)
            {
                foreach (var pb in this.Parent.Controls.OfType<FlatButton>())
                {
                    pb.IsActive = false;
                }
                this.IsActive = true;
            }
        }

        private void FlatButton_MouseEnter(object sender, EventArgs e)
        {
            this.isHover = true;
            this.Invalidate();
        }

        private void FlatButton_MouseLeave(object sender, EventArgs e)
        {
            this.isHover = false;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //===========================================
            //if (_icon != null)
            //{
            //    //var pic = new Bitmap(this.BackgroundImage, new Size(this.Width, this.Height));
            //    this.BackgroundImage = _icon;
            //    //this.DoubleBuffered = true;
            //    //this.SetStyle(ControlStyles.ResizeRedraw, true);
            //    //e.Graphics.DrawImage(_icon, (this.Width - _icon.Width) / 2, (this.Height - _icon.Height) / 2, new Rectangle(0, 0, _icon.Width, _icon.Height), GraphicsUnit.Pixel);
            //}
            //===========================================
            Color _bcolor = Color.Gray;

            if (this.IsActive)
            {
                _bcolor = this.ColorBoderActive;
            }
            else if (this.isHover)
            {
                _bcolor = this.ColorBoderHover;
            }
            else
            {
                _bcolor = this.ColorBoderNormal;
            }

            int _bwidth = this.BoderWidthNormal;
            if (this.isHover || this.IsActive)
            {
                _bwidth = this.BoderWidthActive;
            }


            ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                                         _bcolor, _bwidth, ButtonBorderStyle.Solid,
                                         _bcolor, _bwidth, ButtonBorderStyle.Solid,
                                         _bcolor, _bwidth, ButtonBorderStyle.Solid,
                                         _bcolor, _bwidth, ButtonBorderStyle.Solid);
        }
        private void FlatButton_MouseClick(object sender, MouseEventArgs e)
        {
            this.IsActive = true;
            this.FlatButton_Refresh();
        }

    }
}
