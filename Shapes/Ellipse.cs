using System;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace lab1._2.Shapes
{
    public class myEllipse : myCircle
    {
        public int width;

        public myEllipse(int x, int y, int radius, int width, System.Windows.Media.Brush color) : base (x, y, radius, color)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.width = width;
            this.color = color;
        }

        public override void draw(System.Windows.Controls.Canvas canvas)
        {
            var ellipse = new Ellipse();
            ellipse.Fill = this.color;
            ellipse.Width = this.width;
            ellipse.Height = this.radius;
            canvas.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, this.x);
            Canvas.SetTop(ellipse, this.y);
        }
    }
}
