using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HeadSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        List<Window> winlist;

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            winlist = new List<Window>();
            for (int i = 0; i < 15000; i++)
            {
                Window w = new Window();
                winlist.Add(w);
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            winlist.Clear();
        }
    }
}