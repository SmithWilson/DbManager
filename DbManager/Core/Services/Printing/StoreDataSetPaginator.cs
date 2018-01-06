using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DbManager.Core.Services.Printing
{
    /// <summary>
    /// Конфигурация печати.
    /// </summary>
    public class StoreDataSetPaginator : DocumentPaginator
    {
        private DataTable _table;
        private Typeface _typeFace;
        private double _fontSize;
        private double _margin;
        private Size _size;

        private int _pageCount;
        private int _rowsPerPage;

        public StoreDataSetPaginator(DataTable dt, Typeface typeface, double fontSize, double margin, Size pageSize)
        {
            this._table = dt;
            this._typeFace = typeface;
            this._fontSize = fontSize;
            this._margin = margin;
            this._size = pageSize;
            PaginateData();
        }
        public override Size PageSize
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                PaginateData();
            }
        }

        public void PaginateData()
        {
            //FormattedText text = GetFormattedText("A\nA");

            _rowsPerPage = (int)((_size.Height - _margin * 2) / 40);

            _rowsPerPage -= 1;

            _pageCount = (int)Math.Ceiling((double)_table.Rows.Count / _rowsPerPage);
        }

        private FormattedText GetFormattedText(string text)
        {
            return GetFormattedText(text, _typeFace);
        }

        private FormattedText GetFormattedText(string text, Typeface typeface)
        {
            return new FormattedText(
                 text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                      typeface, _fontSize, Brushes.Black);
        }

        public override bool IsPageCountValid
        {
            get
            {
                return true;
            }
        }

        public override int PageCount
        {
            get
            {
                return _pageCount;
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get
            {
                return null;
            }
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            FormattedText text = GetFormattedText("A");

            double col1_X = _margin;
            double col2_X = col1_X + text.Width * 20;
            double col3_X = col1_X + text.Width * 100;
            double col4_X = col1_X + text.Width * 130;

            int minRow = pageNumber * _rowsPerPage;
            int maxRow = minRow + _rowsPerPage;

            // Создать визуальный элемент для страницы
            DrawingVisual visual = new DrawingVisual();

            // Установить позицию в верхний левый угол печатаемой области
            Point point = new Point(_margin, _margin);

            using (DrawingContext dc = visual.RenderOpen())
            {
                // Нарисовать заголовки столбцов
                Typeface columnHeaderTypeface = new Typeface(_typeFace.FontFamily, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                point.X = col1_X;
                text = GetFormattedText("Архивный Номер", columnHeaderTypeface);
                dc.DrawText(text, point);
                text = GetFormattedText("Название", columnHeaderTypeface);
                point.X = col2_X;
                dc.DrawText(text, point);
                text = GetFormattedText("Заказчик", columnHeaderTypeface);
                point.X = col3_X;
                dc.DrawText(text, point);
                text = GetFormattedText("Договор", columnHeaderTypeface);
                point.X = col4_X;
                dc.DrawText(text, point);

                // Нарисовать линию подчеркивания
                dc.DrawLine(new Pen(Brushes.Black, 2),
                    new Point(_margin, _margin + text.Height),
                    new Point(_size.Width - _margin, _margin + text.Height));

                point.Y += 40;

                // Нарисовать значения столбцов
                for (int i = minRow; i < maxRow; i++)
                {
                    // Проверить конец последней (частично заполненной) страницы
                    if (i > (_table.Rows.Count - 1)) break;

                    point.X = col1_X;
                    text = GetFormattedText(_table.Rows[i]["ArchiveNumber"].ToString());
                    dc.DrawText(text, point);

                    text = GetFormattedText(_table.Rows[i]["Name"].ToString());
                    text.MaxTextHeight = 40;
                    text.MaxTextWidth = 520;
                    point.X = col2_X;
                    dc.DrawText(text, point);

                    text = GetFormattedText(_table.Rows[i]["Treaty"].ToString());
                    text.MaxTextWidth = 200;
                    point.X = col4_X;
                    dc.DrawText(text, point);

                    text = GetFormattedText(_table.Rows[i]["Client"].ToString());
                    text.MaxTextWidth = 200;
                    point.X = col3_X;
                    dc.DrawText(text, point);

                    text = GetFormattedText("A\nA");
                    point.Y += 40;
                }
            }
            return new DocumentPage(visual, _size, new Rect(_size), new Rect(_size));
        }
    }
}
