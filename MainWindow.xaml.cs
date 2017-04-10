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

using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Microsoft.Win32;
using System.IO;

namespace lab1._2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Creator drawer;
        myFigure selectedFigure;
        myFigure shape;
        System.Windows.Media.Color color = System.Windows.Media.Colors.Black;
        FigureList list = FigureList.GetInstance();
        DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(myFigure[]));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLine_Click(object sender, RoutedEventArgs e)
        {
            drawer = new LineCreator();
            for (int i = 0; i < list.count; i++)
            {
                list[i].isSelected = false;
            }
            if (selectedFigure != null) {
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
                selectedFigure = null;
                lbShapes.SelectedIndex = -1;
            }
        }

        private void btnCircle_Click(object sender, RoutedEventArgs e)
        {
            drawer = new CircleCreator();
            for (int i = 0; i < list.count; i++)
            {
                list[i].isSelected = false;
            }
            if (selectedFigure != null)
            {
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
                selectedFigure = null;
                lbShapes.SelectedIndex = -1;
            }
        }

        private void OnNewColorSelected(object sender, SelectionChangedEventArgs e)
        {
            Color newColor = (Color)colorsListBox.SelectedItem;
            color = newColor;
            if (selectedFigure != null)
            {
                if (selectedFigure is IEditable)
                {
                    myFigure selected = list.ToList().Where(t => t.isSelected).ToArray()[0];
                    IEditable editFigure = (IEditable)selected;
                    var index = list.IndexOf(selected);
                    editFigure.Fill(newColor, canvas, index);
                    canvas.Children.RemoveAt(index);
                }
                else {
                    System.Windows.MessageBox.Show("Данная фигура не реализует интерфейс IEditable", "Error", MessageBoxButton.OK);
                }
            }
            else
            {
                color = newColor;
            }
        }

        private void btnEllipse_Click(object sender, RoutedEventArgs e)
        {
            drawer = new EllipseCreator();
            for (int i = 0; i < list.count; i++)
            {
                list[i].isSelected = false;
            }
            if (selectedFigure != null)
            {
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
                selectedFigure = null;
                lbShapes.SelectedIndex = -1;
            }
        }

        private void btnSquare_Click(object sender, RoutedEventArgs e)
        {
            drawer = new SquareCreator();
            for (int i = 0; i < list.count; i++)
            {
                list[i].isSelected = false;
            }
            if (selectedFigure != null)
            {
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
                selectedFigure = null;
                lbShapes.SelectedIndex = -1;
            }
        }

        private void btnRectanle_Click(object sender, RoutedEventArgs e)
        {
            drawer = new RectangleCreator();
            for (int i = 0; i < list.count; i++)
            {
                list[i].isSelected = false;
            }
            if (selectedFigure != null)
            {
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
                selectedFigure = null;
                lbShapes.SelectedIndex = -1;
            }
        }

        private void btnTriangle_Click(object sender, RoutedEventArgs e)
        {
            drawer = new TriangleCreator();
            for (int i = 0; i < list.count; i++)
            {
                list[i].isSelected = false;
            }
            if (selectedFigure != null)
            {
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
                selectedFigure = null;
                lbShapes.SelectedIndex = -1;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(this);
            labelPos.Content = String.Format("X: {0}, Y: {1}", pt.X, pt.Y);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if ((selectedFigure != null) && (e.LeftButton == MouseButtonState.Pressed))
                {
                    if ((selectedFigure is IEditable) && (selectedFigure is ISelectable))
                    {
                        if (e.GetPosition(this).X <= canvas.Width && e.GetPosition(this).Y <= canvas.Height) 
                        {
                            IEditable editFigure = (IEditable)selectedFigure;
                            ISelectable selFigure = (ISelectable)selectedFigure;
                            /*if (editFigure.InRect(e.GetPosition(this))) 
                            {*/
                                int tmp = lbShapes.SelectedIndex;
                                canvas.Children.RemoveAt(canvas.Children.Count - 1);//рамка
                                selectedFigure = editFigure.Redraw(e.GetPosition(this), canvas, tmp);
                                canvas.Children.RemoveAt(tmp);
                                selFigure.Frame(canvas); 
                            //}
                        }
                    }
                    else {
                        System.Windows.MessageBox.Show("Данная фигура не реализует интерфейс IEditable", "Error", MessageBoxButton.OK);
                    }
                }
            }
        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
            {
                if (selectedFigure != null)
                {
                    canvas.Children.RemoveAt(canvas.Children.Count - 1);
                    selectedFigure = null;
                }
                list.DeleteLastFigure(lbShapes);
                if (canvas.Children.Count - 1 >= 0)
                    canvas.Children.RemoveAt(canvas.Children.Count - 1);
            }
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.A)
            {
                list.DeleteAllFigures(lbShapes);
                canvas.Children.Clear();
                selectedFigure = null;
            }
        }

        private void lbShapes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbShapes.SelectedIndex != -1)
            {
                if (selectedFigure != null && (selectedFigure is ISelectable) && (selectedFigure is IEditable)) {
                    canvas.Children.RemoveAt(canvas.Children.Count - 1);    
                }
                selectedFigure = (myFigure)list.GetElemAt((int)lbShapes.SelectedIndex);

                for (int i = 0; i< list.count; i++)
                {
                    list[i].isSelected = (i == lbShapes.SelectedIndex);
                    if (list[i].isSelected && (selectedFigure is ISelectable))
                    {
                        ISelectable selFigure = (ISelectable)selectedFigure;
                        selFigure.Frame(canvas);
                    }
                }
                drawer = null;
            }
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            list.DeleteLastFigure(lbShapes);
            if (canvas.Children.Count - 1 >= 0)
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
            selectedFigure = null;
        }

        private void MenuDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            list.DeleteAllFigures(lbShapes);
            canvas.Children.Clear();
            selectedFigure = null;
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (selectedFigure != null) 
                {
                    if ((selectedFigure is IEditable) && (selectedFigure is ISelectable))
                    {
                        IEditable editFigure = (IEditable)selectedFigure;
                        ISelectable selFigure = (ISelectable)selectedFigure;
                        if (editFigure.InRect(e.GetPosition(this)))
                        {
                            int tmp = lbShapes.SelectedIndex;
                            canvas.Children.RemoveAt(canvas.Children.Count - 1);//рамка
                            selectedFigure = editFigure.Redraw(e.GetPosition(this), canvas, tmp);
                            canvas.Children.RemoveAt(tmp);
                            selFigure.Frame(canvas);
                        }
                        else
                        {
                            canvas.Children.RemoveAt(canvas.Children.Count - 1);
                            selectedFigure = null;
                            lbShapes.SelectedIndex = -1;
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Данная фигура не реализует интерфейс IEditable", "Error", MessageBoxButton.OK);
                    }
                }
                return;
            }
            if (drawer != null)
            {
                drawer.addPoint(e.GetPosition(canvas));
                if (drawer.isEnoughPoints)
                {
                    shape = drawer.Create(color);
                    shape.draw(canvas, canvas.Children.Count);
                    list.AddFigureInList(shape, lbShapes);
                    drawer.isEnoughPoints = true;
                }
            }
            
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selectedFigure != null)
            {
                selectedFigure.isSelected = false;
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JSON File|*.json";
            saveFileDialog1.DefaultExt = "json";
            saveFileDialog1.Title = "Save the Picture";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                myFigure[] arr = list.ToList();
                using (FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, arr);
                }
            }
        }

        private void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JSON files|*.json";

            if (openFileDialog1.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open))
                {
                    myFigure[] arr = (myFigure[])jsonFormatter.ReadObject(fs);

                    canvas.Children.Clear();
                    list.DeleteAllFigures(lbShapes);
                    selectedFigure = null;
                    int i = 0;
                    foreach (myFigure figureToDraw in arr)
                    {
                        figureToDraw.draw(canvas, i);
                        list.AddFigureInList(figureToDraw, lbShapes);
                        i++;
                    }
                }
            }
        }

    }

    [DataContract]
    [KnownType(typeof(myFigure))]
    [KnownType(typeof(myCircle))]
    [KnownType(typeof(myEllipse))]
    [KnownType(typeof(myLine))]
    [KnownType(typeof(myRectangle))]
    [KnownType(typeof(mySquare))]
    [KnownType(typeof(myTriangle))]
    class FigureList
    {
        private static FigureList instance;
        [DataMember]
        protected  List<myFigure> figures = new List<myFigure>();


        public myFigure[] ToList() { return figures.ToArray(); }

        public myFigure this[int i]
        {
            get { return figures[i]; }
            set { figures[i] = value; }
        }

        public int IndexOf(myFigure obj) {
            return figures.IndexOf(obj);
        }

        public void AddFigureInList(myFigure figure, ListBox lbShapes)
        {
            figures.Add(figure);
            lbShapes.Items.Add(figure.getName());
        }

        public int count {
            get {
                return figures.Count;
            }
        }

        public void AddAt(int index, myFigure figure, ListBox lbShapes)
        {
            figures.Insert(index, figure);
            lbShapes.Items.Insert(index, figure.getName());
        }

        public void DeleteLastFigure(ListBox lbShapes)
        {
            if (lbShapes.Items.Count - 1 >= 0)
                lbShapes.Items.RemoveAt(lbShapes.Items.Count - 1);
            if (figures.Count - 1 >= 0)
                figures.RemoveAt(figures.Count - 1);
        }

        public void DeleteAllFigures(ListBox lbShapes)
        {
            figures.Clear();
            lbShapes.Items.Clear();
        }

        public void RemoveAt(int index, ListBox lbShapes)
        {
            figures.RemoveAt(index);
            lbShapes.Items.RemoveAt(index);
        }

        public myFigure GetElemAt(int index)
        {
                myFigure fig = (myFigure)figures.ElementAt(index);
                return fig;
        }

        public static FigureList GetInstance()
        {
            if (instance == null)
                instance = new FigureList();
            return instance;
        }
    }
}
