using System;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace lab1._2.Shapes
{
    [DataContract(Name = "Circle")]
    public class myCircle : myFigure, ISelectable, IEditable
    {
        [DataMember]
        public Point p2;
        [DataMember]
        private int rad;

        public myCircle(Point p1, Point p2, System.Windows.Media.Color color) 
        {
            this.p1 = p1;
            this.p2 = p2;
            this.color = color;
            this.rad = GetDistanse(this.p1, this.p2);
        }

        public int GetDistanse(Point p1, Point p2)
        {
            double x2d = Convert.ToDouble(p2.X.ToString());
            double y2d = Convert.ToDouble(p2.Y.ToString());
            double y1d = Convert.ToDouble(p1.Y.ToString());
            double x1d = Convert.ToDouble(p1.X.ToString());

            double tmp = Math.Sqrt(Math.Pow(x2d - x1d, 2) + Math.Pow(y2d - y1d, 2));

            return (int)tmp;
        }

        public override void draw(System.Windows.Controls.Canvas canvas, int index) 
        {
            var circle = new Ellipse();
            System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(color);
            circle.Fill = brush;
            circle.Width = this.rad * 2;
            circle.Height = this.rad * 2;
            //canvas.Children.Add(circle);
            canvas.Children.Insert(index, circle);
            Canvas.SetLeft(circle, this.p1.X - this.rad);
            Canvas.SetTop(circle, this.p1.Y - this.rad);
        }

        public override bool isSelected { get; set; }

        public override string getName()
        {
            return "Круг";
        }

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

        public bool InRect(Point pt) {
            bool result =  false;
            if ( (pt.X >= (double)(this.p1.X - this.rad - 5)) && (pt.Y >= (double)(this.p1.Y -this.rad - 5)) ) {
                if ( (pt.X <= (double)(this.p1.X + this.rad + 5)) && (pt.Y <= (double)(this.p1.Y + this.rad + 5)) )
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
                rect.Width = this.rad * 2 + 10;
                rect.Height = this.rad * 2 + 10;
                canvas.Children.Add(rect);
                Canvas.SetLeft(rect, this.p1.X - this.rad - 5);
                Canvas.SetTop(rect, this.p1.Y- this.rad - 5);
            }
        }
    }
}