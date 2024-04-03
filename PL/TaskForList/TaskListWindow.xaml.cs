using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BO;

namespace PL.TaskForList
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        private BO.Engineer? eng;

        private readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public BO.Status TaskStatus { get; set; } = BO.Status.None;

        public bool IsUser
        {
            get { return (bool)GetValue(isUserProperty); }
            set { SetValue(isUserProperty, value); }
        }

        public static readonly DependencyProperty isUserProperty =
            DependencyProperty.Register("IsUser", typeof(bool), typeof(TaskListWindow));


        public ObservableCollection<BO.TaskInList> TaskList
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(ObservableCollection<BO.TaskInList>), typeof(TaskListWindow));

        public TaskListWindow(bool isUser, int id = 0)
        {
            if (id != 0)
                eng = s_bl.Engineer.Read(id);
            InitializeComponent();
            if (isUser)
                TaskList = new(s_bl?.Task.ReadAll()!);
            else
            {
                BO.Engineer eng = s_bl.Engineer.Read(id);
                TaskList =new( s_bl.Task.TasksForEngineer(eng));
            }
            IsUser = isUser;
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskStatus == BO.Status.None)
                TaskList = new(s_bl?.Task.ReadAll()!);
            else
                TaskList = new(s_bl?.Task.ReadAll(item => item.Status == TaskStatus)!);
        }

        private void UpdateBtn_Click(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? TaskList = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (TaskList != null)
            {
                // Create a new window with the selected item in update mode
                TaskWindow window = new TaskWindow(CollectionChange, eng, TaskList.Id);
                window.Show();
            }
        }

        private void CollectionChange(int id, bool? newObj = null)
        {
            try
            {
                if (newObj is null)//remove engineer
                {
                    BO.TaskInList oldTask = TaskList.First(x => x.Id == id);
                    TaskList.Remove(oldTask);
                }
                if (newObj == false)
                {
                    BO.Task task = s_bl.Task.Read(id)!;
                    BO.TaskInList oldEnj = TaskList.First(x => x.Id == id);
                    int index = TaskList.IndexOf(oldEnj);
                    TaskList[index] = new TaskInList()
                    {
                        Id = id,
                        Alias = task.Alias,
                        Description = task.Description,
                        Status = task.Status
                    };
                }
                else
                {
                    BO.Task task = s_bl.Task.Read(id)!;
                    TaskList.Add(new TaskInList()
                    {
                        Id = id,
                        Alias = task.Alias,
                        Description = task.Description,
                        Status = task.Status
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Please try again", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ButtonAddUppT_Click(object sender, RoutedEventArgs e)
        {
            new TaskWindow(CollectionChange).Show();
        }

    }
}
