using System;
using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace lab1._2.Shapes
{
    [DataContract(Name = "Line")]
    public class myLine : myFigure, ISelectable, IEditable
    {
        [DataMember]
        private int width, height;
        [DataMember]
        private bool lr;

        public myLine(Point p1, Point p2, System.Windows.Media.Color color) 
        {
            if (p1.X <= p2.X && p2.Y >= p1.Y) {
                this.p1 = p1;
                this.width = (int)p2.X - (int)p1.X;
                this.height = (int)p2.Y - (int)p1.Y;
                this.lr = true;
            } else if (p2.X <= p1.X && p1.Y >= p2.Y) {
                this.p1 = p2;
                this.width = (int)p1.X - (int)p2.X;
                this.height = (int)p1.Y - (int)p2.Y;
                this.lr = true;
            } else if (p2.X >= p1.X && p1.Y >= p2.Y) {
                this.p1 = p2;
                this.width = (int)p2.X - (int)p1.X;
                this.height = (int)p1.Y - (int)p2.Y;
                this.lr = false;
            } else {
                this.p1 = p1;
                this.width = (int)p1.X - (int)p2.X;
                this.height = (int)p2.Y - (int)p1.Y;
                this.lr = false;
            } 
            this.color = color;
        }

        public override void draw(System.Windows.Controls.Canvas canvas, int index)
        {
            var line = new Line();
            System.Windows.Media.Brush brush = new System.Windows.Media.SolidColorBrush(color);
            line.Stroke = brush;
            line.X1 = this.p1.X;
            line.Y1 = this.p1.Y;
            if (lr)
            {
                line.X2 = this.p1.X + this.width;
                line.Y2 = this.p1.Y + this.height;
            } else {
                line.X2 = this.p1.X - this.width;
                line.Y2 = this.p1.Y + this.height;
            }
            line.StrokeThickness = 3;
            canvas.Children.Insert(index, line);
        }

        public override string getName()
        {
            return "Линия";
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
            if (lr)
            {
                if ((pt.X >= (double)(this.p1.X - 5)) && (pt.Y >= (double)(this.p1.Y - 5)))
                {
                    if ((pt.X <= (double)(this.p1.X + this.width + 5)) && (pt.Y <= (double)(this.p1.Y + this.height + 5)))
                    {
                        result = true;
                    }
                }
            }
            else {
                if ((pt.X >= (double)(this.p1.X - this.width - 5)) && (pt.Y >= (double)(this.p1.Y - 5)))
                {
                    if ((pt.X <= (double)(this.p1.X + 5)) && (pt.Y <= (double)(this.p1.Y + this.height + 5)))
                    {
                        result = true;
                    }
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
                if (this.lr)
                {
                    Canvas.SetLeft(rect, this.p1.X - 5);
                    Canvas.SetTop(rect, this.p1.Y - 5);
                }
                else 
                {
                    Canvas.SetLeft(rect, this.p1.X - this.width - 5);
                    Canvas.SetTop(rect, this.p1.Y - 5);
                }
            }
        }
    }
}
