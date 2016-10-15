using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Extensions;

namespace UI
{
    public partial class AddNewItemForm : Form
    {
        private readonly Form1 _form1;
        private const int HEIGHT = 190;
        private const int WIDTH = 280;
        public AddNewItemForm(string itemName, Form1 form1)
        {
            InitializeComponent();
            SignUiElementsToEvents();

            BackColor = ColorTranslator.FromHtml(MyColors.GREY_COLOR);

            if (itemName.AllowBuyInWeight())
            {
                numericUpDown.DecimalPlaces = 1;
                numericUpDown.Increment = (decimal) 0.1;
            }

            this.Height = HEIGHT;
            this.Width = WIDTH;
            this._form1 = form1;
            label1.Text = itemName;
        }
        private void SignUiElementsToEvents()
        {
            button1.FlatAppearance.BorderSize = 0;
            button2.FlatAppearance.BorderSize = 0;
            button1.MouseEnter += (s, e) =>
            {
                button1.BackColor = ColorTranslator.FromHtml(MyColors.GREEN_COLOR);
            };
            button1.MouseLeave += (s, e) =>
            {
                button1.BackColor = ColorTranslator.FromHtml(MyColors.GREY_COLOR);
            };
            button2.MouseEnter += (s, e) =>
            {
                button2.BackColor = ColorTranslator.FromHtml(MyColors.RED_COLOR);
            };
            button2.MouseLeave += (s, e) =>
            {
                button2.BackColor = ColorTranslator.FromHtml(MyColors.GREY_COLOR);
            };
        }
        private void button1_Click(object sender, EventArgs e)
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
            _form1.NewItemAmount = newAmount;
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            _form1.NewItemAmount = null;
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
