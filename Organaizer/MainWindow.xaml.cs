using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Organaizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            ListViewTask.Foreground = new SolidColorBrush(Colors.Black);

            var uri = new Uri(@"Dictionary1.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.Black);

            MainGrid.Background = new SolidColorBrush(Colors.Black);

            ButtonApply.Background = new SolidColorBrush(Colors.DimGray);
            ButtonApply.Foreground = new SolidColorBrush(Colors.White);

            ButtonDarkTheme.Background = new SolidColorBrush(Colors.DimGray);
            ButtonDarkTheme.Foreground = new SolidColorBrush(Colors.White);

            ButtonLightTheme.Background = new SolidColorBrush(Colors.DimGray);
            ButtonLightTheme.Foreground = new SolidColorBrush(Colors.White);

            ButtonAddTask.Background = new SolidColorBrush(Colors.DimGray);
            ButtonAddTask.Foreground = new SolidColorBrush(Colors.White);


            ListViewTask.Background = new SolidColorBrush(Colors.Black);
            ListViewTask.Foreground = new SolidColorBrush(Colors.White);
        }
           
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Background = new SolidColorBrush(Colors.White);

            MainGrid.Background = new SolidColorBrush(Colors.White);

            ButtonApply.Background = new SolidColorBrush(Colors.White);
            ButtonApply.Foreground = new SolidColorBrush(Colors.Black);

            ButtonDarkTheme.Background = new SolidColorBrush(Colors.White);
            ButtonDarkTheme.Foreground = new SolidColorBrush(Colors.Black);

            ButtonLightTheme.Background = new SolidColorBrush(Colors.White);
            ButtonLightTheme.Foreground = new SolidColorBrush(Colors.Black);

            ButtonAddTask.Background = new SolidColorBrush(Colors.White);
            ButtonAddTask.Foreground = new SolidColorBrush(Colors.Black);


            ListViewTask.Background = new SolidColorBrush(Colors.White);
            ListViewTask.Foreground = new SolidColorBrush(Colors.Black);
        }
        
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}