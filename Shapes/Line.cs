using System;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace lab1._2.Shapes
{
    public class myLine : myFigure
    {
        public int x2, y2;

        public myLine(int x1, int y1, int x2, int y2, System.Windows.Media.Brush color) 
        {
            this.x = x1;
            this.y = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.color = color;
        }

        public override void draw(System.Windows.Controls.Canvas canvas)
        {
            var line = new Line();
            line.Stroke = this.color;
            line.X1 = this.x;
            line.Y1 = this.y;
            line.X2 = this.x2;
            line.Y2 = this.y2;
            line.StrokeThickness = 3;
            canvas.Children.Add(line);
        }
    }
}
