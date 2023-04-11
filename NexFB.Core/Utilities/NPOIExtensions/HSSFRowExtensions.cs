// ----------------------------------------------------------------------
// <copyright file="HSSFRowExtensions.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/27/2014 3:33:29 PM">Created.</change>
// </history>

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
                var dayInMonth = id - numberColumnOfInfoEmployee;
                var cell = row.GetCell(id);
                if (listRedOrder.Contains(dayInMonth))
                {
                    var cellStyle = row.Sheet.Workbook.CreateCellStyle();
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cellStyle.FillForegroundColor = HSSFColor.Orange.Index;
                    cellStyle.FillPattern = FillPattern.SolidForeground;
                    cellStyle.BorderTop = BorderStyle.Thin;
                    cellStyle.BorderLeft = BorderStyle.Thin;
                    cellStyle.BorderBottom = BorderStyle.Thin;
                    cellStyle.BorderRight = BorderStyle.Thin;
                    cellStyle.TopBorderColor = HSSFColor.Black.Index;
                    cellStyle.LeftBorderColor = HSSFColor.Black.Index;
                    cellStyle.BottomBorderColor = HSSFColor.Black.Index;
                    cellStyle.RightBorderColor = HSSFColor.Black.Index;
                    cell.CellStyle = cellStyle;
                }
                else if (listGreenOrder.Contains(dayInMonth))
                {
                    var cellStyle = row.Sheet.Workbook.CreateCellStyle();
                    cellStyle.VerticalAlignment = VerticalAlignment.Center;
                    cellStyle.FillForegroundColor = HSSFColor.LightGreen.Index;
                    cellStyle.FillPattern = FillPattern.SolidForeground;
                    cellStyle.BorderTop = BorderStyle.Thin;
                    cellStyle.BorderLeft = BorderStyle.Thin;
                    cellStyle.BorderBottom = BorderStyle.Thin;
                    cellStyle.BorderRight = BorderStyle.Thin;
                    cellStyle.TopBorderColor = HSSFColor.Black.Index;
                    cellStyle.LeftBorderColor = HSSFColor.Black.Index;
                    cellStyle.BottomBorderColor = HSSFColor.Black.Index;
                    cellStyle.RightBorderColor = HSSFColor.Black.Index;
                    cell.CellStyle = cellStyle;
                }

                cell.SetCellValue(value);
                id++;
            }
        }
    }
}