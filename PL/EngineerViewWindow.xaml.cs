using BO;
using DO;
using PL.TaskForList;
using PL.User;
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
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for EngineerViewWindow.xaml
    /// </summary>
    public partial class EngineerViewWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private readonly BO.User user;
        private UserWindow userWindow;
        public EngineerViewWindow(BO.User user)
        {
            InitializeComponent();
            this.user = user;
            this.userWindow = new UserWindow();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            new EntrancnceWindow().Show();
            this.Close(); 
        }

        private void BtnUppUse_Click(object sender, RoutedEventArgs e)
        {
            if (userWindow == null || !userWindow.IsVisible)
            {
                userWindow = new UserWindow();
            }

            userWindow.User = user;
            userWindow.Show();
        }
        private TaskListWindow? _taskListWindow;
        private void BtnCurrent_Click(object sender, RoutedEventArgs e)
        {
            // int id = int.Parse(user.Id!);
            new TaskWindow(_taskListWindow!, user.Id!).Show();
            this.Close();
            
        }
        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
             int id = int.Parse(user.Id!);
            BO.Engineer eng = s_bl.Engineer.Read(id)!;
            new TaskListWindow(eng).Show();
            this.Close();
        }
    }
}
