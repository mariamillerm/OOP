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
    class myTriangle : myFigure//, ISelectable, IEditable
    {
        [DataMember]
        public Point p2, p3;

        public myTriangle(Point p1, Point p2, Point p3, Color color)
        {
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

        //public void drawRect(Canvas canvas)
        //{
        //    if (this.isSelected)
        //    {
        //        var rect = new Rectangle();
        //        rect.Stroke = System.Windows.Media.Brushes.Gray;
        //        rect.Width = 12;
        //        rect.Height = 12;
        //        canvas.Children.Add(rect);
        //        Canvas.SetLeft(rect, this.p1.X);
        //        Canvas.SetTop(rect, this.p1.Y);
        //    }
        //}
    }
}
