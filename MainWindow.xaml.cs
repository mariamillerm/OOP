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

namespace Lab1._2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Creator drawer;
        System.Windows.Media.Brush color = System.Windows.Media.Brushes.Black;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLine_Click(object sender, RoutedEventArgs e)
        {
            drawer = new LineCreator();
        }

        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (drawer != null)
            {
                drawer.addPoint(e.GetPosition(canvas));
                if (drawer.isEnoughPoints)
                {
                    myFigure shape = drawer.Create(color);
                    shape.draw(canvas);
                    drawer.isEnoughPoints = true;
                }
            }
        }

        private void btnCircle_Click(object sender, RoutedEventArgs e)
        {
            drawer = new CircleCreator();
        }

        private void OnNewColorSelected(object sender, SelectionChangedEventArgs e)
        {
            Color newColor = (Color)colorsListBox.SelectedItem;
            color = new SolidColorBrush(newColor);
        }
    }

    abstract class Creator
    {
        abstract public myFigure Create(System.Windows.Media.Brush color);
        public abstract void addPoint(Point point);
        public abstract bool isEnoughPoints { get; set; }
    }

    class CircleCreator : Creator
    {
        public int GetDistanse(Point p1, Point p2)
        {
            double x2d = Convert.ToDouble(p2.X.ToString());
            double y2d = Convert.ToDouble(p2.Y.ToString());
            double y1d = Convert.ToDouble(p1.Y.ToString());
            double x1d = Convert.ToDouble(p1.X.ToString());

            double tmp = Math.Sqrt(Math.Pow(x2d - x1d, 2) + Math.Pow(y2d - y1d, 2));

            return (int)tmp;
        }

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
        
        public override myFigure Create(System.Windows.Media.Brush color)
        {
            int rad = GetDistanse(points.ElementAt(0), points.ElementAt(1));
            return new myCircle(Convert.ToInt32(points.ElementAt(0).X), Convert.ToInt32(points.ElementAt(0).Y), rad, color);
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
                if (value) {
                    points.Clear();
                }
            }
        }
        public override void addPoint(Point point)
        {
            points.Add(point);
        }

        public override myFigure Create(System.Windows.Media.Brush color)
        {
            return new myLine(Convert.ToInt32(points.ElementAt(0).X), Convert.ToInt32(points.ElementAt(0).Y), Convert.ToInt32(points.ElementAt(1).X), Convert.ToInt32(points.ElementAt(1).Y), color);  
        }
    }
}
