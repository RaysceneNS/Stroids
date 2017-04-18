using System;
using System.Collections.Generic;
using System.Drawing;
using Pen = System.Drawing.Pen;

namespace Stroids
{
    public class ScreenCanvas
    {
        private readonly List<Pen> _pensLineColor;
        private readonly List<Pen> _pensPolyColor;

        private readonly List<Point> _points;
        private readonly List<Point[]> _polygons;
        private readonly List<Point[]> _polyLines;

        public ScreenCanvas()
        {
            _points = new List<Point>();
            _pensLineColor = new List<Pen>();

            _polygons = new List<Point[]>();
            _pensPolyColor = new List<Pen>();
            _polyLines = new List<Point[]>();
        }

        public void AddLine(Point ptStart, Point ptEnd, Pen penColor)
        {
            _points.Add(ptStart);
            _points.Add(ptEnd);
            _pensLineColor.Add(penColor);
        }

        private void AddPolyLine(params Point[] pts)
        {
            _polyLines.Add(pts);
        }

        private void AddLetter(char chDraw, int letterLeft, int letterTop,
            int letterWidth, int letterHeight)
        {
            var leftMargin = (int)(letterLeft + letterWidth * 0.2);
            var topMargin = (int)(letterTop + letterHeight * 0.1);
            var centerX = (leftMargin + letterLeft + letterWidth) / 2;
            var centerY = (topMargin + letterTop + letterHeight) / 2;
            var letterRight = letterLeft + letterWidth;
            var letterBottom = letterTop + letterHeight;

            switch (chDraw)
            {
                case '0':
                case 'O':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(letterRight, topMargin)
                        , new Point(letterRight, letterBottom)
                        , new Point(leftMargin, letterBottom)
                        , new Point(leftMargin, topMargin));
                    break;
                case '1':
                case 'I':
                    AddPolyLine(new Point(centerX, topMargin), new Point(centerX, letterBottom));
                    break;
                case '2':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(letterRight, topMargin)
                    , new Point(letterRight, centerY)
                    , new Point(leftMargin, centerY)
                    , new Point(leftMargin, letterBottom)
                    , new Point(letterRight, letterBottom));
                    break;
                case '3':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(letterRight, topMargin)
                    , new Point(letterRight, letterBottom)
                    , new Point(leftMargin, letterBottom));
                    AddPolyLine(new Point(leftMargin, centerY), new Point(letterRight, centerY));
                    break;
                case '4':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(leftMargin, centerY)
                    , new Point(letterRight, centerY));
                    AddPolyLine(new Point(letterRight, topMargin), new Point(letterRight, letterBottom));
                    break;
                case '5':
                case 'S':
                    AddPolyLine(new Point(letterRight, topMargin), new Point(leftMargin, topMargin)
                    , new Point(leftMargin, centerY)
                    , new Point(letterRight, centerY)
                    , new Point(letterRight, letterBottom)
                    , new Point(leftMargin, letterBottom));
                    break;
                case '6':
                    AddPolyLine(new Point(letterRight, topMargin), new Point(leftMargin, topMargin)
                    , new Point(leftMargin, letterBottom)
                    , new Point(letterRight, letterBottom)
                    , new Point(letterRight, centerY)
                    , new Point(leftMargin, centerY));
                    break;
                case '7':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(letterRight, topMargin)
                    , new Point(letterRight, letterBottom));
                    break;
                case '8':
                case 'B':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(letterRight, topMargin)
                    , new Point(letterRight, letterBottom)
                    , new Point(leftMargin, letterBottom)
                    , new Point(leftMargin, topMargin));
                    AddPolyLine(new Point(leftMargin, centerY), new Point(letterRight, centerY));
                    break;
                case '9':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(letterRight, topMargin)
                    , new Point(letterRight, letterBottom)
                    , new Point(leftMargin, letterBottom));
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(leftMargin, centerY)
                    , new Point(letterRight, centerY));
                    break;

                case 'A':
                    AddPolyLine(new Point(leftMargin, letterBottom), new Point(leftMargin, centerY)
                    , new Point(centerX, topMargin)
                    , new Point(letterRight, centerY)
                    , new Point(letterRight, letterBottom));
                    AddPolyLine(new Point(leftMargin, centerY), new Point(letterRight, centerY));
                    break;
                case 'C':
                    AddPolyLine(new Point(letterRight, topMargin), new Point(leftMargin, topMargin)
                    , new Point(leftMargin, letterBottom)
                    , new Point(letterRight, letterBottom));
                    break;
                case 'D':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(centerX, topMargin)
                    , new Point(letterRight, centerY)
                    , new Point(centerX, letterBottom)
                    , new Point(leftMargin, letterBottom)
                    , new Point(leftMargin, topMargin));
                    break;
                case 'E':
                    AddPolyLine(new Point(letterRight, topMargin), new Point(leftMargin, topMargin)
                    , new Point(leftMargin, letterBottom)
                    , new Point(letterRight, letterBottom));
                    AddPolyLine(new Point(leftMargin, centerY), new Point(letterRight, centerY));
                    break;
                case 'F':
                    AddPolyLine(new Point(letterRight, topMargin), new Point(leftMargin, topMargin)
                    , new Point(leftMargin, letterBottom));
                    AddPolyLine(new Point(leftMargin, centerY), new Point(letterRight, centerY));
                    break;
                case 'G':
                    AddPolyLine(new Point(letterRight, topMargin), new Point(leftMargin, topMargin)
                    , new Point(leftMargin, letterBottom)
                    , new Point(letterRight, letterBottom)
                    , new Point(letterRight, centerY)
                    , new Point(centerX, centerY));
                    break;
                case 'H':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(leftMargin, letterBottom));
                    AddPolyLine(new Point(letterRight, topMargin), new Point(letterRight, letterBottom));
                    AddPolyLine(new Point(leftMargin, centerY), new Point(letterRight, centerY));
                    break;
                case 'J':
                    AddPolyLine(new Point(letterRight, topMargin), new Point(letterRight, letterBottom)
                    , new Point(centerX, letterBottom)
                    , new Point(leftMargin, centerY));
                    break;
                case 'K':
                    AddPolyLine(new Point(leftMargin, letterBottom), new Point(leftMargin, topMargin));
                    AddPolyLine(new Point(letterRight, topMargin), new Point(leftMargin, centerY)
                    , new Point(letterRight, letterBottom));
                    break;
                case 'L':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(leftMargin, letterBottom)
                    , new Point(letterRight, letterBottom));
                    break;
                case 'M':
                    AddPolyLine(new Point(leftMargin, letterBottom), new Point(leftMargin, topMargin)
                    , new Point(centerX, centerY)
                    , new Point(letterRight, topMargin)
                    , new Point(letterRight, letterBottom));
                    break;
                case 'N':
                    AddPolyLine(new Point(leftMargin, letterBottom), new Point(leftMargin, topMargin)
                    , new Point(letterRight, letterBottom)
                    , new Point(letterRight, topMargin));
                    break;
                case 'P':
                    AddPolyLine(new Point(leftMargin, letterBottom), new Point(leftMargin, topMargin)
                    , new Point(letterRight, topMargin)
                    , new Point(letterRight, centerY)
                    , new Point(leftMargin, centerY));
                    break;
                case 'Q':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(letterRight, topMargin)
                    , new Point(letterRight, letterBottom)
                    , new Point(leftMargin, letterBottom)
                    , new Point(leftMargin, topMargin));
                    AddPolyLine(new Point(centerX, centerY), new Point(letterRight, letterBottom));
                    break;
                case 'R':
                    AddPolyLine(new Point(leftMargin, letterBottom), new Point(leftMargin, topMargin)
                    , new Point(letterRight, topMargin)
                    , new Point(letterRight, centerY)
                    , new Point(leftMargin, centerY)
                    , new Point(letterRight, letterBottom));
                    break;
                case 'T':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(letterRight, topMargin));
                    AddPolyLine(new Point(centerX, topMargin), new Point(centerX, letterBottom));
                    break;
                case 'U':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(leftMargin, letterBottom)
                    , new Point(letterRight, letterBottom)
                    , new Point(letterRight, topMargin));
                    break;
                case 'V':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(centerX, letterBottom), new Point(letterRight, topMargin));
                    break;
                case 'W':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(leftMargin, letterBottom)
                    , new Point(centerX, centerY)
                    , new Point(letterRight, letterBottom)
                    , new Point(letterRight, topMargin));
                    break;
                case 'X':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(letterRight, letterBottom));
                    AddPolyLine(new Point(letterRight, topMargin), new Point(leftMargin, letterBottom));
                    break;
                case 'Y':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(centerX, centerY)
                        , new Point(letterRight, topMargin));
                    AddPolyLine(new Point(centerX, centerY), new Point(centerX, letterBottom));
                    break;
                case 'Z':
                    AddPolyLine(new Point(leftMargin, topMargin), new Point(letterRight, topMargin)
                        , new Point(leftMargin, letterBottom)
                        , new Point(letterRight, letterBottom));
                    break;
                case '\u005E':
                    var num6 = (int)(letterBottom - letterHeight * 0.2);
                    var num7 = (int)(leftMargin + letterWidth * 0.25);
                    var num8 = (int)(letterRight - letterWidth * 0.25);
                    AddPolyLine(new Point(centerX, topMargin), new Point(letterRight, letterBottom)
                    , new Point(num8, num6)
                    , new Point(num7, num6)
                    , new Point(leftMargin, letterBottom)
                    , new Point(centerX, topMargin));
                    break;
                case 'x':
                    AddPolyLine(new Point(leftMargin, centerY), new Point(letterRight, letterBottom));
                    AddPolyLine(new Point(letterRight, centerY), new Point(leftMargin, letterBottom));
                    break;
            }
        }

        public void AddText(string text, Justify justification, int iTopLoc,
            int charWidth, int charHeight, int x, int y)
        {
            int length;
            switch (justification)
            {
                case Justify.Left:
                    length = 100;
                    break;
                case Justify.Center:
                    length = (int)((double)(10000 - text.Length * charWidth) / 2);
                    break;
                case Justify.Right:
                    length = 9900 - text.Length * charWidth;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("justification");
            }

            for (var i = 0; i < text.Length; i++)
            {
                AddLetter(text[i], (int)((double)(length + i * charWidth) / 10000 * x),
                    (int)((double)iTopLoc / 7500 * y), (int)((double)charWidth / 10000 * x),
                    (int)((double)charHeight / 7500 * y));
            }
        }

        public void AddPolygon(Point[] ptArray, Pen penColor)
        {
            _polygons.Add(ptArray);
            _pensPolyColor.Add(penColor);
        }

        public void Clear()
        {
            _points.Clear();
            _pensLineColor.Clear();
            _polygons.Clear();
            _pensPolyColor.Clear();
            _polyLines.Clear();
        }

        public void Draw(Graphics g)
        {
            if (_points.Count % 2 != 0)
            {
                return;
            }

            var num = 0;
            var j = 0;
            while (j < _points.Count)
            {
                g.DrawLine(_pensLineColor[num++], _points[j++], _points[j++]);
            }

            for (var i = 0; i < _polygons.Count; i++)
            {
                g.DrawPolygon(_pensPolyColor[i], _polygons[i]);
            }

            foreach (var points in _polyLines)
            {
                g.DrawLines(Pens.White, points);
            }
        }
    }

    public enum Justify
    {
        Left,
        Center,
        Right
    }
}