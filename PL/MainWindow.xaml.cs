using BO;
using PL.Engineer;
using PL.TaskForList;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MainWindow(User user)
        {
            InitializeComponent();
        }

        private void btnEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }

        private void btnTask_Click(object sender, RoutedEventArgs e) 
        { 
            
            new TaskListWindow().Show();
        }

        private void btnInitDB_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to initialize the database?", "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                s_bl.InitializeDB();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to reset the database?", "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                s_bl.ResetDB();
        }

    }
}