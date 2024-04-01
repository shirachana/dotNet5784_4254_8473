using PL.TaskForList;
using PL.User;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for EngineerViewWindow.xaml
    /// </summary>
    public partial class EngineerViewWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private readonly BO.User user;

        public EngineerViewWindow(BO.User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            new EntrancnceWindow().Show();
            this.Close();
        }

        private void BtnUppUse_Click(object sender, RoutedEventArgs e)
        {
            new UserWindow().Show();
        }
        private TaskListWindow? _taskListWindow;
        private void BtnCurrent_Click(object sender, RoutedEventArgs e)
        {
            BO.Engineer eng = s_bl.Engineer.Read(int.Parse(user.Id!))!;
            if (eng.Task != null)
            {
                new TaskWindow(user.Id).Show();
            }
            else
            {
                MessageBox.Show("There is no current task to this engineer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(user.Id!);
            BO.Engineer eng = s_bl.Engineer.Read(id)!;
            new TaskListWindow(eng.Id).Show();
        }
    }
}
