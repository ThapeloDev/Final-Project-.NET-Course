using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using UI.Extensions;

namespace UI
{
    public partial class EditItemForm : Form
    {
        public EditItemForm()
        {
            InitializeComponent();
            
        }
        private readonly Form1 _form1;
        private const int HEIGHT = 190;
        private const int WIDTH = 280;
        public EditItemForm(string itemName, Form1 form1)
        {
            InitializeComponent();
            BackColor = ColorTranslator.FromHtml(MyColors.GREY_COLOR);
            SignUiElementsToEvents();
            if (itemName.AllowBuyInWeight())
            {
                numericUpDown.DecimalPlaces = 1;
                numericUpDown.Increment = (decimal)0.1;
            }
            this.Height = HEIGHT;
            this.Width = WIDTH;
            this.CenterToScreen();
            this._form1 = form1;
            label1.Text = itemName;
        }
        private void SignUiElementsToEvents()
        {
            button1.FlatAppearance.BorderSize = 0;
            button3.FlatAppearance.BorderSize = 0;
            button1.MouseEnter += (s, e) =>
            {
                button1.BackColor = ColorTranslator.FromHtml(MyColors.GREEN_COLOR);
            };
            button1.MouseLeave += (s, e) =>
            {
                button1.BackColor = ColorTranslator.FromHtml(MyColors.GREY_COLOR);
            };
            button3.MouseEnter += (s, e) =>
            {
                button3.BackColor = ColorTranslator.FromHtml(MyColors.RED_COLOR);
            };
            button3.MouseLeave += (s, e) =>
            {
                button3.BackColor = ColorTranslator.FromHtml(MyColors.GREY_COLOR);
            };
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            double ansOut;
            var newAmount = numericUpDown.Text;
            DialogResult = DialogResult.OK;

            if (!newAmount.StringToDouble(out ansOut))
            {
                DialogResult = DialogResult.Ignore;
                Close();
                return;
            }
            _form1.Cart[label1.Text] = ansOut;
            _form1.NewItemAmount = newAmount;
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
