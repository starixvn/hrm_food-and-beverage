namespace NexFB.Core.Utilities.NPOIExtensions
{
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Row extensions.
    /// </summary>
    public static class HSSFRowExtensions
    {
        /// <summary>
        /// Creates cell at a specific column.
        /// </summary>
        /// <param name="row">Current row.</param>
        /// <param name="colIndex">Index of column where the cell is created.</param>
        /// <param name="cellStyle">Style that will apply for the cell.</param>
        /// <returns>New cell instance.</returns>
        public static ICell CreateCell(this IRow row, int colIndex, ICellStyle cellStyle)
        {
            ICell cell = row.CreateCell(colIndex);

            cell.CellStyle = cellStyle;

            return cell;
        }

        /// <summary>
        /// Fill values to row, start at a specific column.
        /// </summary>
        /// <param name="row">Current row.</param>
        /// <param name="colIndex">Start column index.</param>
        /// <param name="values">List of values.</param>
        public static void WriteCells(this IRow row, int colIndex, object[] values)
        {
            int id = colIndex;
            foreach (var value in values)
            {
                row.GetCell(id).SetCellValue(value);
                id++;
            }
        }

        public static void WriteCellsMainDishPerMeal(this IRow row, int colIndex, object[] values, string redOrder, string greenOrder)
        {
            List<int> listRedOrder = redOrder != "" ? redOrder.Split(',').Select(int.Parse).ToList() : new List<int>();
            List<int> listGreenOrder = greenOrder != "" ? greenOrder.Split(',').Select(int.Parse).ToList() : new List<int>();
            int numberColumnOfInfoEmployee = 7;
            int id = colIndex;

            foreach (var value in values)
            {
                int dayInMonth = id - numberColumnOfInfoEmployee;
                var cell = row.GetCell(id);
                bool isColorCell = listRedOrder.Contains(dayInMonth) || listGreenOrder.Contains(dayInMonth);
                if (isColorCell)
                {
                    var cellStyle = row.Sheet.Workbook.CreateCellStyle();
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cellStyle.FillPattern = FillPattern.SolidForeground;
                    cellStyle.BorderTop = cellStyle.BorderLeft = cellStyle.BorderBottom = cellStyle.BorderRight = BorderStyle.Thin;
                    cellStyle.TopBorderColor = cellStyle.LeftBorderColor = cellStyle.BottomBorderColor = cellStyle.RightBorderColor = HSSFColor.Black.Index;
                    cellStyle.FillForegroundColor = listRedOrder.Contains(dayInMonth) ? HSSFColor.Orange.Index : HSSFColor.LightGreen.Index;
                    cell.CellStyle = cellStyle;
                }

                cell.SetCellValue(value);
                id++;
            }
        }
    }
}
