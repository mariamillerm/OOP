using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace lab1._2.Shapes
{
    interface IEditable
    {
        myFigure Fill(Color color, Canvas canvas, int index);
        myFigure Redraw(Point p1, Canvas canvas, int index);
        bool InRect(Point pt);
    }
}
