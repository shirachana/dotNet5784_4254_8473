using System;
using BO;
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
using System.Diagnostics;

namespace PL.TaskForList
{
    public partial class DependTaskWindow : Window
    {
        static int Id;
        public DependTaskWindow(int id)
        {
            InitializeComponent();
            DepTaskList = s_bl?.Task.ReadAll()!;
            Id = id;
            // Assuming your data is in an ObservableCollection<MyItem>
        }
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public IEnumerable<BO.TaskInList> DepTaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(DepTaskListProperty); }
            set { SetValue(DepTaskListProperty, value); }
        }
        // Assuming your data is in an ObservableCollection<MyItem>
        public static readonly DependencyProperty DepTaskListProperty =
            DependencyProperty.Register("DepTaskList", typeof(IEnumerable<BO.TaskInList>), typeof(DependTaskWindow), new PropertyMetadata(null));

        private List<TaskInList> selectedTasks = new List<TaskInList>();

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            TaskInList task = checkBox.DataContext as TaskInList;

            if (checkBox.IsChecked == true)
            {
                selectedTasks.Add(task);
            }
            else
            {
                selectedTasks.Remove(task);
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var task in selectedTasks)
                {
                    s_bl.Task.Read(task.Id);
                    
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show($"Error: {ex.Message}", "Operation Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Close();
        }

    }
}

