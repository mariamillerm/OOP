using System;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace lab1._2.Shapes
{
    class myRectangle : mySquare
    {
        int height;

        public myRectangle(int x, int y, int height, int width, Brush color) : base (x, y, width, color) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
        }

        public override void draw(Canvas canvas)
        {
            var rect = new Rectangle();
            rect.Fill = this.color;
            rect.Width = this.width;
            rect.Height = this.height;
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, this.x);
            Canvas.SetTop(rect, this.y);
        }
    }
}
