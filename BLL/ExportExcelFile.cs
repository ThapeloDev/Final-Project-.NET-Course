using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel; 


namespace BLL
{
    /*
     * Separating the implementation of the core logic from exporting to excel is a very good idea
     * Way to go!
     */
    public class ExportExcelFile
    {

        private int _row = 1;
        private int _col = 2;

        //Declaring constants is a good practice, way to go!
        private const int LEFT = 250;
        private const int TOP = 10;
        private const int WIDTH = 70;
        private const int HEIGHT = 250;
        public class Cell
        {
            public int row, col;
            public double price;

            public Cell(int r, int c, double p)
            {
                row = r;
                col = c;
                price = p;
            }
        }
        public void ExportExcel(ShoppingCartLogic ccp)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                Console.WriteLine("EXCEL could not be started. Check that your office installation and project references are correct.");
                return;
            }
            xlApp.Visible = true;

            var wb = xlApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            var ws = (Worksheet)wb.Worksheets[1];

            if (ws == null)
            {
                Console.WriteLine("Worksheet could not be created. Check that your office installation and project references are correct.");
            }

            var lst = new Dictionary<string, Cell>();

            /* It is much better to divide a method into smaller "centered" methods.. It is a lot more readable, understandable, maintaiable, debuggable and scalable.
             * Consider: Each block of code which does some operation can be extracted into a method.
             */
            #region Build Table
            foreach (var chain in ccp.Chains.Values)
            {
                ws.Cells[_row, _col] = chain.ChainName;
                foreach (var item in chain.Items.Values.OrderBy(item => item.ItemName))
                {
                    ++_row;
                    ws.Cells[_row, 1] = item.ItemName;
                    ws.Cells[_row, _col] = item.Price;

                    if (lst.ContainsKey(item.ItemName))
                    {
                        if (lst[item.ItemName].price > double.Parse(item.Price))
                        {
                            lst[item.ItemName].price = double.Parse(item.Price);
                            lst[item.ItemName].row = _row;
                            lst[item.ItemName].col = _col;
                        }
                    }
                    else
                    {
                        lst.Add(item.ItemName, new Cell(_row, _col, double.Parse(item.Price)));
                    }

                }
                _row = 1;
                ++_col;
            }
            #endregion
            
            //mark lowest price
            _col = 2;
            foreach (var cell in lst)
            {
                ws.Cells[cell.Value.row, cell.Value.col].Interior.Color = XlRgbColor.rgbLightGreen;
            }

            #region Build Histogram
            Microsoft.Office.Interop.Excel.Range chartRange;
            Excel.ChartObjects xlCharts = (Excel.ChartObjects)ws.ChartObjects(Type.Missing);
            Excel.ChartObject myChart = (Excel.ChartObject)xlCharts.Add(LEFT, TOP, WIDTH * (ccp.Chains.Values.First().Items.Count + 1), HEIGHT);
            Excel.Chart chartPage = myChart.Chart;

            chartRange = ws.get_Range("A1", "D" + (ccp.Chains.Values.First().Items.Count + 1));
            chartPage.SetSourceData(chartRange, Type.Missing);
            chartPage.ChartType = Excel.XlChartType.xlColumnClustered;
            #endregion
        }
        
    }
}
