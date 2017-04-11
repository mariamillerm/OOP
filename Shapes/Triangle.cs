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
        private Point pp1;
        [DataMember]
        private int width, height, three_width, three_height, two_width;
        [DataMember]
        private bool lr, threeHasBiggerX;
        private int thirdPoint = 6;

        public myTriangle(Point p1, Point p2, Point p3, Color color)
        {
            this.width = Math.Max((int)p1.X, Math.Max((int)p2.X, (int)p3.X)) - Math.Min((int)p1.X, Math.Min((int)p2.X, (int)p3.X));
            this.height = Math.Max((int)p1.Y, Math.Max((int)p2.Y, (int)p3.Y)) - Math.Min((int)p1.Y, Math.Min((int)p2.Y, (int)p3.Y));
            this.pp1.X = Math.Min((int)p1.X, Math.Min((int)p2.X, (int)p3.X));
            this.pp1.Y = Math.Min((int)p1.Y, Math.Min((int)p2.Y, (int)p3.Y));
            if (isLeftRight(p1, p2, p3))
            {
                if (this.thirdPoint == 1)
                    this.p3 = p1;
                else if (this.thirdPoint == 2)
                    this.p3 = p2;
                else
                    this.p3 = p3;

                if (this.p1.X <= this.p3.X)
                {
                    this.three_width = (int)this.p3.X - (int)this.p1.X;
                    this.threeHasBiggerX = true;
                }
                else
                {
                    this.three_width = (int)this.p1.X - (int)this.p3.X;
                    this.threeHasBiggerX = false;
                }
                this.three_height = (int)this.p3.Y - (int)this.p1.Y;
            }
            else 
            {
                if (this.thirdPoint == 1)
                    this.p3 = p1;
                else if (this.thirdPoint == 2)
                    this.p3 = p2;
                else
                    this.p3 = p3;

                if (this.p1.X <= this.p3.X)
                {
                    this.three_width = (int)this.p3.X - (int)this.p1.X;
                    this.threeHasBiggerX = true;
                }
                else
                {
                    this.three_width = (int)this.p1.X - (int)this.p3.X;
                    this.threeHasBiggerX = false;
                }
                this.three_height = (int)this.p3.Y - (int)this.p1.Y;
            }
            this.color = color;
        }

        private bool isLeftRight(Point p1, Point p2, Point p3) 
        {
            bool result = false;
            Point tempPoint_1, tempPoint_2;
            int tmp = Math.Min((int)p1.Y, Math.Min((int)p2.Y, (int)p3.Y));
            if (tmp == p1.Y)
            {
                tempPoint_1 = p1;
                this.p1 = p1;
                this.thirdPoint -= 1;
            }
            else if (tmp == p2.Y)
            {
                tempPoint_1 = p2;
                this.p1 = p2;
                this.thirdPoint -= 2;
            }
            else
            {
                tempPoint_1 = p3;
                this.p1 = p3;
                this.thirdPoint -= 3;
            }
            tmp = Math.Max((int)p1.Y, Math.Max((int)p2.Y, (int)p3.Y));
            if (tmp == p1.Y)
            {
                tempPoint_2 = p1;
                this.p2 = p1;
                this.thirdPoint -= 1;
            }
            else if (tmp == p2.Y)
            {
                tempPoint_2 = p2;
                this.p2 = p2;
                this.thirdPoint -= 2;
            }
            else
            {
                tempPoint_2 = p3;
                this.p2 = p3;
                this.thirdPoint -= 3;
            }
            if (tempPoint_1.X < tempPoint_2.X)
            {
                this.lr = true;
                result = true;
                this.two_width = (int)this.p2.X - (int)this.p1.X;
            }
            else
            {
                this.two_width = (int)this.p1.X - (int)this.p2.X;
            }
            return result;
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
            this.p2.Y = this.p1.Y + this.height;
            this.p3.Y = this.p1.Y + this.three_height;
            if (this.threeHasBiggerX)
            {
                this.p3.X = this.p1.X + this.three_width;
            }
            else
            {
                this.p3.X = this.p1.X - this.three_width;
            }
            if (this.lr)
            {
                this.p2.X = this.p1.X + this.two_width;
            }
            else 
            {
                this.p2.X = this.p1.X - this.two_width;
            }
            this.pp1.X = Math.Min((int)p1.X, Math.Min((int)p2.X, (int)p3.X));
            this.pp1.Y = Math.Min((int)p1.Y, Math.Min((int)p2.Y, (int)p3.Y));

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
