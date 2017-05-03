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
using System.IO;
using System.Windows.Shapes;
using InnerInterface;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls.Primitives;
using System.Reflection;

namespace lab1._2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Creator drawer;
        private List<Creator> Creators { get; set; }
        myFigure selectedFigure;
        static public List<Type> LoadedTypes { get; set; }
        private RenderTargetBitmap PrimaryBitmap { get; set; }
        private RenderTargetBitmap SecondaryBitmap { get; set; }
        private int SelectedShapeId { get; set; }
        myFigure shape;
        System.Windows.Media.Color color = System.Windows.Media.Colors.Black;
        FigureList list = FigureList.GetInstance();
        //DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(myFigure[]), LoadedTypes);

        public MainWindow()
        {
            InitializeComponent();
            LoadedTypes = new List<Type>();
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
                    selectedFigure = null;
                }
            }
            else
            {
                color = newColor;
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
                            int tmp = lbShapes.SelectedIndex;
                            canvas.Children.RemoveAt(canvas.Children.Count - 1);//рамка
                            selectedFigure = editFigure.Redraw(e.GetPosition(this), canvas, tmp);
                            canvas.Children.RemoveAt(tmp);
                            selFigure.Frame(canvas); 
                        }
                    }
                    else {
                        System.Windows.MessageBox.Show("Данная фигура не реализует интерфейс IEditable", "Error", MessageBoxButton.OK);
                        selectedFigure = null;
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
                if (!(selectedFigure is ISelectable) || !(selectedFigure is IEditable))
                {
                    selectedFigure = null;
                    return;
                }
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
                        selectedFigure = null;
                    }
                }
                return;
            }
            if (drawer != null)
            {
                drawer.addPoint(e.GetPosition(canvas));
                if (drawer.isEnoughPoints)
                {
                    //shape = drawer.Create(color);
                    shape = Creators[SelectedShapeId].Create(color);
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
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(myFigure[]), LoadedTypes);

            if (saveFileDialog1.FileName != "")
            {
                myFigure[] arr = list.ToList();
                using (FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create))
                {
                    jsonFormatter.WriteObject(fs, arr);
                }
            }
        }

        private string ParseJSONFile(string filename) 
        {
            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            StreamReader reader = new StreamReader(file);
            string str = reader.ReadToEnd();
            string mainString = "[";
            string secString = "";
            reader.Close();
            file.Close();

            int i = 0;
            str = str.Remove(0, 1);
            while (str.Length > 0) {
                int k = str.IndexOf("type");
                secString = secString + str.Substring(i, k + 7);//добавляем к строке все до типа
                str = str.Remove(0, k + 7); // удаляем из старой все до типа
                k = str.IndexOf(":");
                String tempStr = str.Substring(0, k);//тип
                bool isFigureInList = false;
                var arr = LoadedTypes.ToList();
                foreach (var type in arr) {
                    if (type.ToString().Contains(tempStr)) {
                        isFigureInList = true;
                    }
                }
                if ((k = str.IndexOf("},{")) == -1)
                {
                    k = str.IndexOf("}]");
                }
                if (isFigureInList)
                {
                    //secString = secString + tempStr;//добавляем тип
                    secString = secString + str.Substring(0, k + 2);//добавляем к строке все остальное
                    str = str.Remove(0, k + 2);
                }
                else {
                    secString = "";
                    str = str.Remove(0, k + 2);
                }
                mainString = mainString + secString;
                secString = "";
            }
            if (mainString.EndsWith(",")) {
                mainString = mainString.Remove(mainString.Length - 1, 1);
                mainString = mainString + "]";
            }
            return mainString;
            //file = new FileStream(filename, FileMode.Truncate, FileAccess.Write);
            //StreamWriter writer = new StreamWriter(file);
            //writer.Write(mainString);
            //writer.Close();
            //file.Close();
        }

        private void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JSON files|*.json";

            if (openFileDialog1.ShowDialog() == true)
            {
                string str = ParseJSONFile(openFileDialog1.FileName);
                using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open))
                {
                    myFigure[] arr;
                    try
                    {
                        DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(myFigure[]), LoadedTypes);
                        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(str));
                        stream.Position = 0;
                        arr = (myFigure[])jsonFormatter.ReadObject(stream);
                    }
                    catch (Exception) {
                        System.Windows.MessageBox.Show("Файл неправильный!", "Error", MessageBoxButton.OK);
                        return;
                    }
                    
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

        private string GetAssemblyName(string name)
        {
            return name.Substring(0, name.Length - 4);
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            Creators = new List<Creator>() { };

            try
            {
                var DllFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
                foreach (string currentFile in DllFiles)
                {
                    string filename = System.IO.Path.GetFileName(currentFile);
                    string path1 = String.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory, filename);
                    string asmName = GetAssemblyName(System.IO.Path.GetFileName(path1));
                    Assembly asm = Assembly.LoadFrom(path1);
                    Type[] types = asm.GetTypes();
                    Type figure = null;
                    try
                    {
                        foreach (Type t in types)
                        {
                            if (t.BaseType.Equals(typeof(myFigure)))
                            {
                                figure = t;
                            }
                        }
                        LoadedTypes.Add(figure);
                    }
                    catch {
                    }

                    Type type = Assembly.LoadFile(currentFile).GetExportedTypes()[0];
                    try
                    {
                        Creators.Add(Activator.CreateInstance(type) as Creator);

                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            System.Windows.Shapes.Path path = new System.Windows.Shapes.Path()
                            {
                                Stretch = Stretch.Uniform,
                                Stroke = Brushes.Black,
                                StrokeThickness = 2,
                                Data = Geometry.Parse(type.GetField("PathData").GetValue(null) as string)
                            };
                            ToggleButton toggleButton = new ToggleButton()
                            {
                                Width = 25,
                                Height = 25,
                                Padding = new Thickness(3),
                                Margin = new Thickness(0, 0, 0, 0),
                                Content = path
                            };
                            toggleButton.Click += Button_Click;
                            ToolBar.Children.Add(toggleButton);
                        }));
                    }
                    catch {
                    }
                }

                myFigure.KnownTypes = LoadedTypes;
                FigureList.KnownTypes = LoadedTypes;
            }
            catch
            {
                System.Windows.MessageBox.Show("Какая-то ошибка с прочтением директории", "Error", MessageBoxButton.OK);
            }

            PrimaryBitmap = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96, 96, PixelFormats.Default);

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            fileSystemWatcher.Created += (x, y) =>
            {

                string filename = System.IO.Path.GetFileName(y.FullPath);
                string path2 = String.Format("{0}/{1}", AppDomain.CurrentDomain.BaseDirectory, filename);
                string asmName = GetAssemblyName(System.IO.Path.GetFileName(path2));
                Assembly asm = Assembly.LoadFrom(path2);
                Type[] types = asm.GetTypes();
                Type figure = null;

                foreach (Type t in types)
                {
                    if (t.BaseType.Equals(typeof(myFigure)))
                    {
                        figure = t;
                    }
                }
                LoadedTypes.Add(figure);
                myFigure.KnownTypes = LoadedTypes;
                FigureList.KnownTypes = LoadedTypes;

                Type type = Assembly.LoadFile(y.FullPath).GetExportedTypes()[0];
                Creators.Add(Activator.CreateInstance(type) as Creator);
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    System.Windows.Shapes.Path path = new System.Windows.Shapes.Path()
                    {
                        Stretch = Stretch.Uniform,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2,
                        Data = Geometry.Parse(type.GetField("PathData").GetValue(null) as string)
                    };
                    ToggleButton toggleButton = new ToggleButton()
                    {
                        Width = 25,
                        Height = 25,
                        Padding = new Thickness(3),
                        Margin = new Thickness(0, 0, 0, 0),
                        Content = path
                    };
                    toggleButton.Click += Button_Click;
                    ToolBar.Children.Add(toggleButton);
                }));
            };
            fileSystemWatcher.EnableRaisingEvents = true;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton selectedToggleButton = sender as ToggleButton;
            SelectedShapeId = ToolBar.Children.IndexOf(selectedToggleButton);
            foreach (UIElement uiElement in ToolBar.Children)
            {
                (uiElement as ToggleButton).IsChecked = uiElement == selectedToggleButton ? true : false;
            }
            drawer = Creators[SelectedShapeId];

            //убирает выделение со списка
            for (int i = 0; i < list.count; i++)
            {
                list[i].isSelected = false;
            }
            //убирает рамку
            if (selectedFigure != null)
            {
                canvas.Children.RemoveAt(canvas.Children.Count - 1);
                selectedFigure = null;
                lbShapes.SelectedIndex = -1;
            }
        }

    }

    [DataContract]
    [KnownType("GetKnownTypes")]
    class FigureList
    {
        public static List<Type> KnownTypes { get; set; }
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

        private static Type[] GetKnownTypes()
        {
            return FigureList.KnownTypes.ToArray();
        }
    }
}
