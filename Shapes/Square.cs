using System;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace lab1._2.Shapes
{
    class mySquare : myFigure
    {
        public int width;

        public mySquare(int x, int y, int width, System.Windows.Media.Brush color)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.color = color;
        }

        public override void draw(Canvas canvas)
        {
            var square = new Rectangle();
            square.Fill = this.color;
            square.Width = this.width;
            square.Height = this.width;
            canvas.Children.Add(square);
            Canvas.SetLeft(square, this.x);
            Canvas.SetTop(square, this.y);
        }
    }
}
