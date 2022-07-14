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

namespace CustomerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool _isMove = false;
        private Point _mouseDownPos;
        private Point _mouseControlDownPos;
        public MainWindow()
        {
            InitializeComponent();
            DemoModel = new object();
        }

        public static readonly DependencyProperty DemoModelProperty = DependencyProperty.Register(
       "DemoModel", typeof(object), typeof(MainWindow), new PropertyMetadata(default(object)));

        public object DemoModel
        {
            get => GetValue(DemoModelProperty);
            set => SetValue(DemoModelProperty, value);
        }

        private void TreeViewItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeView = sender as TreeViewItem;
            var box = new TextBlock { Text = treeView.Header.ToString() };
            box.MouseEnter += Box_MouseEnter;
            CanvasDic.Children.Add(box);
        }

        private void Box_MouseEnter(object sender, MouseEventArgs e)
        {
            DemoModel = sender;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMove)
            {
                var c = e.Source as FrameworkElement;
                if (c != null && !(c is Canvas))
                {

                    var pos = e.GetPosition(this);
                    var dp = pos - _mouseDownPos;
                    Canvas.SetLeft(c, _mouseControlDownPos.X + dp.X);
                    Canvas.SetTop(c, _mouseControlDownPos.Y + dp.Y);
                }
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var c = e.Source as FrameworkElement;
            var runC = e.Source as System.Windows.Documents.Run;
            if (c != null && !(c is Canvas))
            {
                MoveControl(c, e);
            }
            else if(runC != null && !(runC.Parent is Canvas))
            {
                var pa = runC.Parent as FrameworkElement;
                if(pa != null)
                MoveControl(pa, e);
            }
        }

        private void MoveControl(FrameworkElement c, MouseButtonEventArgs e)
        {
            _isMove = true;
            _mouseDownPos = e.GetPosition(this);
            _mouseControlDownPos = new Point(double.IsNaN(Canvas.GetLeft(c)) ? 0 : Canvas.GetLeft(c), double.IsNaN(Canvas.GetTop(c)) ? 0 : Canvas.GetTop(c));
            c.CaptureMouse();
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var c = e.Source as FrameworkElement;
            if (c != null && !(c is Canvas))
            {
                _isMove = false;
                c.ReleaseMouseCapture();
            }
        }
        //private void MainWindow_load(object sender, RoutedEventArgs e)
        //{
        //    var item = typeof(MainWindow).Assembly.GetTypes().Where(t => typeof(Control).IsAssignableFrom(t))
        //      .Where(t => !t.IsAbstract && t.IsClass).Select(t => (Control)Activator.CreateInstance(t));
        //    foreach (var toolBase in item)
        //    {
        //        StackPanelToolList.Children.Add(toolBase);
        //    }
        //}


    }
}
