using PL.Engineer;
using PL.TaskForList;
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

namespace PL.TaskForList
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl?.Task.ReadAll()!;
        }
        public TaskListWindow(BO.Engineer? engineer=null)
        {
            InitializeComponent();
            TaskList = s_bl?.Task.TasksForEngineer(engineer!)!;
        }
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));

        public BO.Status TaskStatus { get; set; } = BO.Status.None;
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (TaskStatus == BO.Status.None) ?
                s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => item.Status == TaskStatus)!;

        }
        private void UpdateBtn_Click(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? TaskList = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (TaskList != null)
            {
                // Create a new window with the selected item in update mode
                TaskWindow window = new TaskWindow(this, TaskList.Id);
                window.ShowDialog();
            }
        }

        private void ButtonAddUppT_Click(object sender, RoutedEventArgs e)
        {
            new TaskWindow(this).ShowDialog();
        }
    }
}
