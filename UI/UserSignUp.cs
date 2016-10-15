using System;
using System.Drawing;
using System.Windows.Forms;
using BLL;

namespace UI
{
    public partial class UserSignUp : Form
    {
        private readonly Form1 _form1;
        private const int HEIGHT = 190;
        private const int WIDTH = 280;
        public UserSignUp(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1;
            this.Height = HEIGHT;
            this.Width = WIDTH;
            BackColor = ColorTranslator.FromHtml(MyColors.GREY_COLOR);
            SignUiElementsToEvents();
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

        private async void button1_Click(object sender, EventArgs e)
        {
            var username = textBox1.Text;
            var uv = new UserLogic();

            if (await uv.IsUsernameExist(username))
            {
                label2.Visible = true;
                return;
            }
            else
            {
                if (await uv.SignNewUserToDb(username))
                {
                    _form1.SuccessLogin();
                    _form1.CurrentUserName = username;
                    _form1.username_lbl.Text = username + " שלום";
                    Close();
                }
                else
                {
                    label2.Visible = true;
                    label2.Text = "אופס, משהו קרה";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
