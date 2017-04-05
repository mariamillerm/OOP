using lab1._2.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab1._2
{
    abstract class Creator
    {
        abstract public myFigure Create(System.Windows.Media.Color color);
        public abstract void addPoint(Point point);
        public abstract bool isEnoughPoints { get; set; }
    }

    class CircleCreator : Creator
    {
        public List<Point> points = new List<Point>();
        public override void addPoint(Point point)
        {
            points.Add(point);
        }
        public override bool isEnoughPoints
        {
            get
            {
                if (points.Count == 2)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    points.Clear();
                }
            }
        }

        public override myFigure Create(System.Windows.Media.Color color)
        {
            return new myCircle(points.ElementAt(0), points.ElementAt(1), color);
        }
    }

    class EllipseCreator : CircleCreator
    {
        public override myFigure Create(Color color)
        {
            return new myEllipse(points.ElementAt(0), points.ElementAt(1), color);
        }
    }

    class LineCreator : Creator
    {
        protected List<Point> points = new List<Point>();
        public override bool isEnoughPoints
        {
            get
            {
                if (points.Count == 2)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    points.Clear();
                }
            }
        }
        public override void addPoint(Point point)
        {
            points.Add(point);
        }

        public override myFigure Create(System.Windows.Media.Color color)
        {
            return new myLine(points.ElementAt(0), points.ElementAt(1), color);
        }
    }

    class SquareCreator : Creator
    {
        public List<Point> points = new List<Point>();
        public override void addPoint(Point point)
        {
            points.Add(point);
        }
        public override bool isEnoughPoints
        {
            get
            {
                if (points.Count == 2)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    points.Clear();
                }
            }
        }

        public override myFigure Create(Color color)
        {
            return new mySquare(points.ElementAt(0), points.ElementAt(1), color);
        }
    }

    class RectangleCreator : SquareCreator
    {
        public override myFigure Create(Color color)
        {
            return new myRectangle(points.ElementAt(0), points.ElementAt(1), color);
        }
    }

    class TriangleCreator : Creator
    {
        public List<Point> points = new List<Point>();
        public override void addPoint(Point point)
        {
            points.Add(point);
        }
        public override bool isEnoughPoints
        {
            get
            {
                if (points.Count == 3)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                {
                    points.Clear();
                }
            }
        }

        public override myFigure Create(Color color)
        {
            return new myTriangle(points.ElementAt(0), points.ElementAt(1), points.ElementAt(2), color);
        }
    }
}
