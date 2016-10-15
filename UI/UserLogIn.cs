using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BLL;

namespace UI
{
    public partial class UserLogIn : Form
    {
        private readonly UserLogic _uv;
        private readonly Form1 _form1;
        private const int HEIGHT = 190;
        private const int WIDTH = 280;
        public UserLogIn(Form1 form1)
        {
            InitializeComponent();
            _uv = new UserLogic();
            this.Height = HEIGHT;
            this.Width = WIDTH;
            BackColor = ColorTranslator.FromHtml(MyColors.GREY_COLOR);
            this.CenterToScreen();
            SignUiElementsToEvents();
            _form1 = form1;
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
        private async void Login_btn_Click(object sender, EventArgs e)
        {
            var username = textBox1.Text;
            Dictionary<string, string> userList = await _uv.GetUser(username);
            if (userList == null)
            {
                DialogResult = DialogResult.None;
                MessageBox.Show("משתמש לא קיים");
                return;
            }
            _form1.EmptyCart();
            _form1.LoadUserList(userList);
            _form1.CurrentUserName = username;
            _form1.username_lbl.Text = username + " שלום";
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        

    }
}
