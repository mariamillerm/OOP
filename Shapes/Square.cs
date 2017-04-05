using System;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace lab1._2.Shapes
{
    [DataContract(Name = "Square")]
    class mySquare : myFigure, ISelectable, IEditable
    {
        [DataMember]
        public Point p2;
        [DataMember]
        private int side;

        public mySquare(Point p1, Point p2, System.Windows.Media.Color color)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.color = color;
            int x1 = Convert.ToInt32(this.p1.X);
            int y1 = Convert.ToInt32(this.p1.Y);
            int x2 = Convert.ToInt32(this.p2.X);
            int y2 = Convert.ToInt32(this.p2.Y);
            if (y2 >= y1 && x2 >= x1)
            {
                this.side = x2 - x1;
            }
            else if (y1 >= y2 && x1 >= x2)
            {
                this.p1 = p2;
                this.side = x1 - x2;
            }
            else if (x2 >= x1 && y1 >= y2)
            {
                this.p1.X = p1.X;
                this.p1.Y = p2.Y;
                this.side = x2 - x1;
            }
            else
            {
                this.p1.X = p2.X;
                this.p1.Y = p1.Y;
                this.side = x1 - x2;
            }
        }

        public override void draw(Canvas canvas, int index)
        {
            var square = new Rectangle();
            System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(color);
            square.Fill = brush;
            square.Width = this.side;
            square.Height = this.side;
            //canvas.Children.Add(square);
            canvas.Children.Insert(index, square);
            Canvas.SetLeft(square, this.p1.X);
            Canvas.SetTop(square, this.p1.Y);
        }

        public override string getName()
        {
            return "Квадрат";
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
            if ((pt.X >= (double)(this.p1.X - 10)) && (pt.Y >= (double)(this.p1.Y - 10)))
            {
                if ((pt.X <= (double)(this.p1.X + this.side + 5)) && (pt.Y <= (double)(this.p1.Y + this.side + 5)))
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
                rect.Width = this.side + 10;
                rect.Height = this.side + 10;
                canvas.Children.Add(rect);
                Canvas.SetLeft(rect, this.p1.X - 5);
                Canvas.SetTop(rect, this.p1.Y - 5);
            }
        }
    }
}
