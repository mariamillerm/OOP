using System;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace lab1._2.Shapes
{
    public class myCircle : myFigure
    {
        public int radius;

        public myCircle(int x, int y, int radius, System.Windows.Media.Brush color) 
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.color = color;
        }

        public override void draw(System.Windows.Controls.Canvas canvas) 
        {
            var circle = new Ellipse();
            circle.Fill = this.color;
            circle.Width = this.radius * 2;
            circle.Height = this.radius * 2;
            canvas.Children.Add(circle);
            Canvas.SetLeft(circle, this.x - this.radius);
            Canvas.SetTop(circle, this.y - this.radius);
        }
    }
}