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
            FormattedText text = GetFormattedText("A");

            _rowsPerPage = (int)((_size.Height - _margin * 2) / text.Height);

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
            double col2_X = col1_X + text.Width * 15;
            double col3_X = col1_X + text.Width * 90;
            double col4_X = col1_X + text.Width * 120;

            int minRow = pageNumber * _rowsPerPage;
            int maxRow = minRow + _rowsPerPage;

            // Создать визуальный элемент для страницы
            DrawingVisual visual = new DrawingVisual();

            // Установить позицию в верхний левый угол печатаемой области
            Point point = new Point(_margin, _margin);

            TextFormatFlags
            using (DrawingContext dc = visual.RenderOpen())
            {
                // Нарисовать заголовки столбцов
                Typeface columnHeaderTypeface = new Typeface(_typeFace.FontFamily, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                point.X = col1_X;
                text = GetFormattedText("ArchiveNumber", columnHeaderTypeface);
                dc.DrawText(text, point);
                text = GetFormattedText("Name", columnHeaderTypeface);
                point.X = col2_X;
                dc.DrawText(text, point);
                text = GetFormattedText("Client", columnHeaderTypeface);
                point.X = col3_X;
                dc.DrawText(text, point);
                text = GetFormattedText("Treaty", columnHeaderTypeface);
                point.X = col4_X;
                dc.DrawText(text, point);

                // Нарисовать линию подчеркивания
                dc.DrawLine(new Pen(Brushes.Black, 2),
                    new Point(_margin, _margin + text.Height),
                    new Point(_size.Width - _margin, _margin + text.Height));

                point.Y += text.Height;

                // Нарисовать значения столбцов
                for (int i = minRow; i < maxRow; i++)
                {
                    // Проверить конец последней (частично заполненной) страницы
                    if (i > (_table.Rows.Count - 1)) break;

                    point.X = col1_X;
                    text = GetFormattedText(_table.Rows[i]["ArchiveNumber"].ToString());
                    dc.DrawText(text, point);

                    
                    // Добавить второй столбец
                    text = GetFormattedText(_table.Rows[i]["Name"].ToString());
                    
                    point.X = col2_X;
                    dc.DrawText(text, point);

                    text = GetFormattedText(_table.Rows[i]["Client"].ToString());
                    point.X = col3_X;
                    dc.DrawText(text, point);


                    text = GetFormattedText(_table.Rows[i]["Treaty"].ToString());
                    point.X = col4_X;
                    dc.DrawText(text, point);
                    point.Y += text.Height;
                }
            }
            return new DocumentPage(visual, _size, new Rect(_size), new Rect(_size));
        }
    }
}
