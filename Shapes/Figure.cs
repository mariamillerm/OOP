using System;
using System.Drawing;

namespace lab1._2.Shapes
{
    public abstract class myFigure
    {
        public int x, y;
        public System.Windows.Media.Brush color;

        public abstract void draw(System.Windows.Controls.Canvas canvas);
    }
}