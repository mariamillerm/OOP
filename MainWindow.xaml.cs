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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Creator drawer;
        System.Windows.Media.Brush color = System.Windows.Media.Brushes.Black;
        FigureList list = FigureList.GetInstance();

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
                    list.AddFigureInList(shape);
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

        private void btnEllipse_Click(object sender, RoutedEventArgs e)
        {
            drawer = new EllipseCreator();
        }

        private void btnSquare_Click(object sender, RoutedEventArgs e)
        {
            drawer = new SquareCreator();
        }

        private void btnRectanle_Click(object sender, RoutedEventArgs e)
        {
            drawer = new RectangleCreator();
        }

        private void btnTriangle_Click(object sender, RoutedEventArgs e)
        {
            drawer = new TriangleCreator();
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(this);
            labelPos.Content = String.Format("X: {0}, Y: {1}", pt.X, pt.Y);
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
            {
                list.DeleteLastFigure();
                if (canvas.Children.Count - 1 >= 0)
                    canvas.Children.RemoveAt(canvas.Children.Count - 1);
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.A)
            {
                list.DeleteAllFigures();
                canvas.Children.Clear();
            }
        }
    }

    class FigureList
    {
        private static FigureList instance;
        protected  List<myFigure> figures = new List<myFigure>();

        public void AddFigureInList(myFigure figure)
        {
            figures.Add(figure);
        }

        public void DeleteLastFigure()
        {
            if (figures.Count - 1 >= 0)
                figures.RemoveAt(figures.Count - 1);
        }

        public void DeleteAllFigures()
        {
            figures.Clear();
        }

        public static FigureList GetInstance()
        {
            if (instance == null)
                instance = new FigureList();
            return instance;
        }
    }
}
