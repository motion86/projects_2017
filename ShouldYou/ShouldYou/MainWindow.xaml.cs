using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace ShouldYou
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random _rdn { get; set; }
        private Brush _yes => Brushes.DarkGreen;
        private Brush _no => Brushes.DarkRed;
        private Brush _dk => Brushes.LightGray;
        private Brush _thinking => Brushes.Black;
        public MainWindow()
        {
            InitializeComponent();
            _rdn = new Random();
            MyBox.Background = _dk;
            MyBox.MouseUp += async(o, e) =>
            {
                MyBox.Background = _thinking;
                MyBox.Background = await Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(500);
                    return Flip();
                });
            };
            MyBox.MouseDown += (o, e) =>
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            };
            exitCharm.MouseDown += (o, e) => this.Close();
        }

        private Brush Flip()
        {
            var x = _rdn.NextDouble();
            if (x < 0.33333)
                return _no;
            if (x < 0.66666)
                return _dk;
            return _yes;
        }
    }
}
