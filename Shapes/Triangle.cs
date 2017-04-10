using System;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace lab1._2.Shapes
{
    [DataContract(Name = "Triangle")]
    class myTriangle : myFigure, ISelectable, IEditable
    {
        [DataMember]
        public Point p2, p3;
        [DataMember]
        private Point pp1, pp2, pp3;
        [DataMember]
        private int width, height;

        public myTriangle(Point p1, Point p2, Point p3, Color color)
        {
            this.width = Math.Max((int)p1.X, Math.Max((int)p2.X, (int)p3.X)) - Math.Min((int)p1.X, Math.Min((int)p2.X, (int)p3.X));
            this.height = Math.Max((int)p1.Y, Math.Max((int)p2.Y, (int)p3.Y)) - Math.Min((int)p1.Y, Math.Min((int)p2.Y, (int)p3.Y));
            this.pp1.X = Math.Min((int)p1.X, Math.Min((int)p2.X, (int)p3.X));
            this.pp1.Y = Math.Min((int)p1.Y, Math.Min((int)p2.Y, (int)p3.Y));
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.color = color;
        }

        public override void draw(Canvas canvas, int index)
        {
            var triangle = new Polygon();
            System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(color);
            triangle.Fill = brush;
            PointCollection myPointCollection = new PointCollection();
            myPointCollection.Add(this.p1);
            myPointCollection.Add(this.p2);
            myPointCollection.Add(this.p3);
            triangle.Points = myPointCollection;
            canvas.Children.Insert(index, triangle);
        }

        public override string getName()
        {
            return "Треугольник";
        }

        public override bool isSelected { get; set; }

        public myFigure Redraw(Point p1, Canvas canvas, int index)
        {
            this.p1 = p1;
            draw(canvas, index + 1);
            return this;
        }

        public myFigure Fill(System.Windows.Media.Color color, Canvas canvas, int index)
        {
            this.color = color;
            draw(canvas, index + 1);
            return this;
        }

        public bool InRect(Point pt)
        {
            bool result = false;
            if ((pt.X >= (double)(this.pp1.X - 5)) && (pt.Y >= (double)(this.pp1.Y - 5)))
            {
                if ((pt.X <= (double)(this.pp1.X + this.height + 5)) && (pt.Y <= (double)(this.pp1.Y + this.height + 5)))
                {
                    result = true;
                }
            }
            return result;
        }

        public void Frame(Canvas canvas)
        {
            if (this.isSelected)
            {
                var rect = new Rectangle();
                rect.Stroke = System.Windows.Media.Brushes.Black;
                System.Windows.Media.DoubleCollection coll = new System.Windows.Media.DoubleCollection();
                coll.Add(3);
                coll.Add(4);
                rect.StrokeDashArray = coll;
                rect.Width = this.width + 10;
                rect.Height = this.height + 10;
                canvas.Children.Add(rect);
                Canvas.SetLeft(rect, this.pp1.X - 5);
                Canvas.SetTop(rect, this.pp1.Y - 5);
            }
        }
    }
}
