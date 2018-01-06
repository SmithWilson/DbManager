using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DbManager.Models;

namespace DbManager.Core.Services.Printing
{
    class PrintData
    {
        /// <summary>
        /// Печать.
        /// </summary>
        /// <param name="facilitys"></param>
        public void Print(DataTable facilitys)
        {
            PrintDialog printDialog = new PrintDialog();

            PrintTicket prntkt = printDialog.PrintTicket;
            prntkt.PageOrientation = PageOrientation.Landscape;
            printDialog.PrintTicket = prntkt;

            if (printDialog.ShowDialog() == true)
            {
                StoreDataSetPaginator paginator = new StoreDataSetPaginator(facilitys,
                    new Typeface("Calibri"), 12, 48 * 0.75,
                    new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));

                printDialog.PrintDocument(paginator, "Печать с помощью классов визуального уровня");
            }
        }
    }
}
