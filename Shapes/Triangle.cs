using System;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace lab1._2.Shapes
{
    class myTriangle : myFigure
    {
        public int x2, y2, x3, y3;

        public myTriangle(int x1, int y1, int x2, int y2, int x3, int y3, Brush color)
        {
            this.x = x1;
            this.y = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.x3 = x3;
            this.y3 = y3;
            this.color = color;
        }

        public override void draw(Canvas canvas)
        {
            var triangle = new Polygon();
            triangle.Fill = this.color;
            System.Windows.Point Point1 = new System.Windows.Point(this.x, this.y);
            System.Windows.Point Point2 = new System.Windows.Point(this.x2, this.y2);
            System.Windows.Point Point3 = new System.Windows.Point(this.x3, this.y3);
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(Point1);
            myPointCollection.Add(Point2);
            myPointCollection.Add(Point3);
            triangle.Points = myPointCollection;
            canvas.Children.Add(triangle);

        }
    }
}
