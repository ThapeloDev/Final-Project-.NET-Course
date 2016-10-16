using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using BLL;
using UI.Extensions;

namespace UI
{
    /**
    * When creating a UI application- consider one of the following paradigms: MVC, MVP or MVVM
    * It is best to refrain from coding in the codebehind of the UI class.
    * This enables better testability and separation of UI from User interaction and Business Logic.
    * 
    * Consider :
    * a) https://he.wikipedia.org/wiki/Model_View_Controller
    * b) https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93presenter
    * c) https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel
    */
    public partial class Form1 : Form
    {
        #region Consts
        private const int TREE_NODE_CHILD = 0;
        private const int NUM_ITEMS_TO_SHOW = 3;
        private const int HEIGHT = 650;
        private const int WIDTH = 960;
        #endregion

        #region Properties
        public ShoppingCartLogic CCP { get; private set; }
        public Dictionary<string, double> Cart { get; private set; }
        private Dictionary<string, List<string>> itemsList;
        private ExportExcelFile EEF;
        private RssReader RssReader;
        private RssMessage _rssMsg;
        private delegate void SetLabelsText();
        private BackgroundWorker _bw;
        private PictureBox[] _chainStatsPic;
        public bool IsLoggedIn { get; set; }
        public string NewItemAmount { get; set; }
        public string CurrentUserName { get; set; }
        #endregion

        #region Initialization
        public Form1()
        {
            RssReader = new RssReader();
            CCP = new ShoppingCartLogic();
            GetItemsFromDb();
            InitializeComponent();
            Load += Initialize;
        }
        private void Initialize(object sender, EventArgs e)
        {
            Cart = new Dictionary<string, double>();
            EEF = new ExportExcelFile();
            _bw = new BackgroundWorker();

            RightTopPanel.BackColor = ColorTranslator.FromHtml(MyColors.BLUE_COLOR);
            RssPanel.BackColor = ColorTranslator.FromHtml(MyColors.BLUE_COLOR);
            rightPanel.BackColor = ColorTranslator.FromHtml(MyColors.GREY_COLOR);
            TopPanel.BackColor = ColorTranslator.FromHtml(MyColors.GREY_COLOR);
            amountInTable.ForeColor = ColorTranslator.FromHtml(MyColors.BLUE_COLOR);
            itemNameInTable.ForeColor = ColorTranslator.FromHtml(MyColors.BLUE_COLOR);

            tableLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;
            tableLayoutPanel1.Padding = new Padding(0, 0, vertScrollWidth, 0);

            toolTip.SetToolTip(fav_img, "שמור רשימת קניות");

            _bw.DoWork += BackgroundMethod;

            _chainStatsPic = new PictureBox[3];
            _chainStatsPic[0] = RamiLevi;
            _chainStatsPic[1] = Shufersal;
            _chainStatsPic[2] = Victory;

            close_pnl.BackColor = ColorTranslator.FromHtml(MyColors.RED_COLOR);

            SignUiElementsToEvents();
            GetNewRssMsg();
            UpdaeRssLabel();

            Height = HEIGHT;
            Width = WIDTH;
            CenterToScreen();
        }
        private async void GetItemsFromDb()
        {
            itemsList = await CCP.GetAllItemsFromDb();
            AddItemsToTreeView(itemsList);
        }
        private void AddItemsToTreeView(Dictionary<string, List<string>> itemsList)
        {
            foreach (var category in itemsList)
            {
                var node = treeView1.Nodes.Find(category.Key, false).First();
                foreach (var item in category.Value)
                {
                    var newNode = new TreeNode
                    {
                        Name = item,
                        Text = item
                    };
                    node.Nodes.Add(newNode);
                }
            }
        }
        private void SignUiElementsToEvents()
        {
            compatePrice_btn.CustomButton(compareP_pnl, MyColors.BLUE_COLOR);
            emptyCartBTN.CustomButton(emptyCart_pnl, MyColors.BLUE_COLOR);
            toExcel.CustomButton(excel_pnl, MyColors.BLUE_COLOR);
            updateItems.CustomButton(refreshItems_pnl, MyColors.BLUE_COLOR);
            login_btn.CustomButton(LogIn_pnl, MyColors.BLUE_COLOR);
            signup_btn.CustomButton(signin_pnl, MyColors.BLUE_COLOR);
            logout_btn.CustomButton(logout_pnl, MyColors.RED_COLOR);

            treeView1.NodeMouseClick += TreeView1_AfterSelect;

            RamiLevi.MouseClick += ShowStatistics;
            Shufersal.MouseClick += ShowStatistics;
            Victory.MouseClick += ShowStatistics;

            rss_lbl.MouseEnter += (s, e) =>
            {
                rss_lbl.ForeColor = ColorTranslator.FromHtml(MyColors.GREY_COLOR);
                timer.Stop();
                rss_lbl.Cursor = Cursors.Hand;
            };
            rss_lbl.MouseLeave += (s, e) =>
            {
                rss_lbl.ForeColor = Color.White;
                timer.Start();
            };
        }
        #endregion

        #region Right Panel
        #region Calc Cart Price
        private void button1_Click(object sender, EventArgs e)
        {
            if (!_bw.IsBusy)
            {
                _bw.RunWorkerAsync();
            }
        }
        private void BackgroundMethod(object sender, DoWorkEventArgs e)
        {
            CCP.DeleteAllItemsFromChains();
            CCP.GetCartPrice(Cart);
            SetTotalPrice();
        }
        private void SetTotalPrice()
        {
            if (shufersalLabel.InvokeRequired)
            {
                var act = new SetLabelsText(SetTotalPrice);
                Invoke(act);
            }
            else
            {
                shufersalLabel.Visible = true;
                ramiLeviLabel.Visible = true;
                victoryLabel.Visible = true;
                shufersalLabel.Text = CCP.GetChainTotalCartPrice("Shufersal") + "₪";
                ramiLeviLabel.Text = CCP.GetChainTotalCartPrice("RamiLevi") + "₪";
                victoryLabel.Text = CCP.GetChainTotalCartPrice("Victory") + "₪";
            }
        }
        #endregion
        private void emptyCartBTN_Click(object sender, EventArgs e)
        {
            EmptyCart();
            CleanStats();
        }
        private void toExcel_Click(object sender, EventArgs e)
        {
            if (ChainStats_lbl.Visible)
            {
                EEF.ExportExcel(CCP);
            }
            else
            {
                MessageBox.Show("כדי ליצור פעולה זו ראשית בחר סל מוצרים ולאחר מכן השווה מחירים ");
            }
        }
        private void UpdateTreeItems_Click(object sender, EventArgs e)
        {
            treeView1.RemoveAllItems();
            GetItemsFromDb();
        }
        
        public void EmptyCart()
        {
            for (var i = Cart.Count; i > 0; i--)
            {
                tableLayoutPanel1.RemoveRow(i);
            }
            Cart.Clear();
            tableLayoutPanel1.Visible = false;
            fav_img.Visible = false;
        }
        #endregion

        #region Main Panel
        #region Statistics
        private void ShowStatistics(object sender, MouseEventArgs e)
        {
            var pic = sender as PictureBox;
            foreach (var Chainpic in _chainStatsPic)
            {
                Chainpic.BorderStyle = BorderStyle.None;
            }
            pic.BorderStyle = BorderStyle.FixedSingle;
            ChainStats_lbl.Text = " שלושת המוצרים הזולים והיקרים ביותר";
            ChainStats_lbl.Visible = true;
            ShowStatisticsItemsLowPrice(pic.Name);
            ShowStatisticsItemsHighPrice(pic.Name);
        }
        private void CleanStats()
        {
            lowCost.Visible = false;
            highCost.Visible = false;
            ChainStats_lbl.Visible = false;
            shufersalLabel.Visible = false;
            ramiLeviLabel.Visible = false;
            victoryLabel.Visible = false;
        }
        private void ShowStatisticsItemsLowPrice(string chainName)
        {
            var count = 0;
            lowCost.Visible = true;
            var itemsLow = CCP.GetChainItemsOrderByPrice(chainName);
            lowCost.Items.Clear();
            foreach (var item in itemsLow)
            {
                lowCost.Items.Add(item + Environment.NewLine);
                ++count;
                if (count == NUM_ITEMS_TO_SHOW) break;
            }
        }
        private void ShowStatisticsItemsHighPrice(string chainName)
        {
            var count = 0;
            highCost.Visible = true;
            var itemsLow = CCP.GetChainItemsOrderByPriceDescending(chainName);
            highCost.Items.Clear();
            foreach (var item in itemsLow)
            {
                highCost.Items.Add(item + Environment.NewLine);
                ++count;
                if (count == NUM_ITEMS_TO_SHOW) break;
            }
        }
        #endregion
        #region Edit User Selection
        private void EditSelectedItem(object sender, EventArgs e)
        {
            var label = sender as Label;
            var itemName = label.Text;
            var eif = new EditItemForm(itemName, this);
            var res = eif.ShowDialog();
            if (res == DialogResult.No)//Delete
            {
                var index = Cart.Keys.ToList().IndexOf(itemName);
                tableLayoutPanel1.RemoveRow(index + 1);
                Cart.Remove(itemName);
                treeView1.Nodes.Find(itemName, true).First().Checked = false;
            }
            else if (res == DialogResult.OK)
            {
                var amountLabel = tableLayoutPanel1.GetControlFromPosition(0, Cart.Keys.ToList().IndexOf(itemName) + 1);
                amountLabel.Text = NewItemAmount;
            }
        }
        #endregion
        #region Add New Item to cart
        private void TreeView1_AfterSelect(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.BackColor = Color.Transparent;
            if (e.Node.Nodes.Count == TREE_NODE_CHILD)
            {
                var iaf = new AddNewItemForm(e.Node.Text, this);
                var res = iaf.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }
                AddNewItemToTable(e.Node.Text, NewItemAmount);
            }
        }
        private void AddNewItemToTable(string itemName, string amount)
        {
            if (Cart.ContainsKey(itemName))
            {
                MessageBox.Show(itemName + " כבר נמצא ברשימה.", "לא ניתן להוסיף את אותו מוצר פעמיים.");
                return;
            }
            else
            {
                Cart.Add(itemName, double.Parse(amount));
                tableLayoutPanel1.AddNewItemToTable(itemName, amount, EditSelectedItem);
                fav_img.Visible = true;
            }
        }
        #endregion
        private void timer3_Tick(object sender, EventArgs e)
        {
            rss_lbl.Location = new Point(rss_lbl.Location.X - 3, rss_lbl.Location.Y);
            if ((rss_lbl.Location.X + rss_lbl.Text.Length * 6) < 0)
            {
                rss_lbl.Location = new Point(666, rss_lbl.Location.Y);
                GetNewRssMsg();
                UpdaeRssLabel();
            }
        }
        private void UpdaeRssLabel()
        {
            rss_lbl.Text = _rssMsg._title;
        }
        private void GetNewRssMsg()
        {
            _rssMsg = RssReader.GetRssMessage();
        }
        private void rss_lbl_Click(object sender, EventArgs e)
        {
            _rssMsg.OpenLink();
        }
        private async void fav_img_Click(object sender, EventArgs e)
        {
            if (IsLoggedIn == false)
            {
                MessageBox.Show("אינך מחובר, אנא התחבר כדי לשמור סל קניות זה");
                return;
            }

            var ul = new UserLogic();
            await ul.SavefavouriteCart(CurrentUserName, Cart);
        }
        #endregion

        #region Top Panel
        internal void LoadUserList(Dictionary<string, string> userList)
        {
            Cart.Clear();
            foreach (var item in userList)
            {
                Cart.Add(item.Key, double.Parse(item.Value));
                tableLayoutPanel1.AddNewItemToTable(item.Key, item.Value, EditSelectedItem);
            }
        }
        private void login_btn_Click(object sender, EventArgs e)
        {
            if (IsLoggedIn)
            {
                return;
            }

            var uli = new UserLogIn(this);
            var res = uli.ShowDialog();
            if (res == DialogResult.OK)
            {
                SuccessLogin();
            }
        }
        private void logout_btn_Click(object sender, EventArgs e)
        {
            if (IsLoggedIn)
            {
                Logout();
                CleanStats();
            }
        }

        private void signup_btn_Click(object sender, EventArgs e)
        {
            var signUserForm = new UserSignUp(this);
            signUserForm.ShowDialog();
        }
        public void SuccessLogin()
        {
            login_btn.Visible = false;
            logout_btn.Visible = true;
            logout_pic.Visible = true;
            fav_img.Visible = true;
            pictureBox9.Visible = false;
            IsLoggedIn = true;
            CleanStats();
        }
        public void Logout()
        {
            IsLoggedIn = false;
            logout_btn.Visible = false;
            logout_pic.Visible = false;
            username_lbl.Text = "שלום אורח";
            fav_img.Visible = false;
            pictureBox9.Visible = true;
            login_btn.Visible = true;
            EmptyCart();
        }
        private void pictureBox13_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

    }
}
