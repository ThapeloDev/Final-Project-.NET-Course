using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.Extensions
{
    static class Extensions
    {
        private const string BUY_IN_WEIGHT = "במשקל";
        public static void RemoveRow(this TableLayoutPanel panel, int rowIndexToRemove)
        {
            if (rowIndexToRemove >= panel.RowCount)
            {
                return;
            }

            // delete all controls of row that we want to delete
            for (var i = 0; i < panel.ColumnCount; i++)
            {
                var control = panel.GetControlFromPosition(i, rowIndexToRemove);
                panel.Controls.Remove(control);
            }

            // move up row controls that comes after row we want to remove
            for (var i = rowIndexToRemove + 1; i < panel.RowCount; i++)
            {
                for (var j = 0; j < panel.ColumnCount; j++)
                {
                    var control = panel.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        panel.SetRow(control, i - 1);
                    }
                }
            }

            // remove last row
            panel.RowStyles.RemoveAt(panel.RowCount - 1);
            panel.RowCount--;
        }
        public static void AddNewItemToTable(this TableLayoutPanel panel, string itemName, string amount, EventHandler editSelectedItem)
        {
            panel.Visible = true;
            ++panel.RowCount;
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            Label newItemAmount = new Label()
            {
                Text = amount,
                TextAlign = ContentAlignment.MiddleCenter
            };
            Label newItemName = new Label()
            {
                Text = itemName,
                TextAlign = ContentAlignment.MiddleRight
            };
            newItemName.Click += editSelectedItem;
            newItemName.MouseEnter += (s, e) =>
            {
                newItemName.ForeColor = Color.Red;
                newItemName.Cursor = Cursors.Hand;
            };
            newItemName.MouseLeave += (s, e) =>
            {
                newItemName.ForeColor = Color.Black;
            };

            panel.Controls.Add(newItemAmount, 0, panel.RowCount - 1);
            panel.Controls.Add(newItemName, 1, panel.RowCount - 1);

        }
        public static bool AllowBuyInWeight(this String itemName)
        {
            return itemName.Contains(BUY_IN_WEIGHT);
        }
        public static bool StringToDouble(this String amount, out double outAmount)
        {
            if (double.TryParse(amount, out outAmount) && outAmount != 0)
            {
                return true;
            }
            return false;
        }
        public static bool IsDecimal(this double amount)
        {
            return !(amount % 1 == 0);
        }
        public static void RemoveAllItems(this TreeView tree)
        {
            foreach (TreeNode category in tree.Nodes)
            {
                category.Nodes.Clear();
            }
        }
        public static void CustomButton(this Button btn, Panel pnl, string color)
        {
            btn.MouseEnter += (s, e) =>
            {
                pnl.BackColor = ColorTranslator.FromHtml(color);
            };
            btn.MouseLeave += (s, e) =>
            {
                pnl.BackColor = Color.Transparent;
            };
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
        }
    }



}
