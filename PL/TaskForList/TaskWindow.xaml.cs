using BO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PL.TaskForList
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public bool OpenDialogHost
        {
            get { return (bool)GetValue(OpenDialogHostProperty); }
            set { SetValue(OpenDialogHostProperty, value); }
        }

        public static readonly DependencyProperty OpenDialogHostProperty =
            DependencyProperty.Register(nameof(OpenDialogHost), typeof(bool), typeof(TaskWindow));

        public IEnumerable<BO.TaskInList> TasksList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(DepTaskListProperty); }
            set { SetValue(DepTaskListProperty, value); }
        }
        public static readonly DependencyProperty DepTaskListProperty =
            DependencyProperty.Register(nameof(TasksList), typeof(IEnumerable<BO.TaskInList>), typeof(TaskWindow));

        public bool isUserValue
        {
            get { return (bool)GetValue(isUserVProperty); }
            set { SetValue(isUserVProperty, value); }
        }
        public static readonly DependencyProperty isUserVProperty =
            DependencyProperty.Register("isUserValue", typeof(bool), typeof(TaskWindow));
        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }
        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow));

        private TaskListWindow? _taskListWindow;

        public TaskWindow(TaskListWindow taskListWindow, BO.Engineer? engineerT = null, int taskId = 0)
        {
            if (engineerT == null)
                isUserValue = true;
            else
            {
                isUserValue = false;
            }
            OpenDialogHost = false;
            _taskListWindow = taskListWindow;
            if (taskId == 0)
            {
                Task = new BO.Task();
                Task.Dependencies = new();
            }
            else
            {
                try
                {
                    Task = s_bl.Task.Read(taskId) ?? new();
                    if (Task.Dependencies is null)
                        Task.Dependencies = new();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }

            }
            if (Task.Engineer == null)
                Task.Engineer = new();
            DataContext = this;
            InitializeComponent();
        }

        public TaskWindow(TaskListWindow taskListWindow, string? engId)
        {
            try
            {
                InitializeComponent();
                _taskListWindow = taskListWindow;
                OpenDialogHost = false;

                // Fetch the engineer's task based on the specified ID
                if (int.TryParse(engId, out int id))
                {
                    BO.TaskInEngineer engineerTask = s_bl.Engineer.GetEngineerTask(id)!;

                    if (engineerTask != null)
                    {
                        Task = s_bl.Task.Read(engineerTask.Id) ?? new();
                        if (Task.Dependencies is null)
                            Task.Dependencies = new();

                    }
                    else
                    {
                        MessageBox.Show("No task found for the engineer.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid engineer ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            if (Task == null || Task.Engineer == null)
            {
                Task = Task ?? new();
                Task.Engineer = new();
            }
        }




        private void btnAddUpdateTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((sender as Button)!.Content.ToString() == "Add")
                {
                    s_bl.Task.Create(Task);

                    IEnumerable<BO.TaskInList> updatedTaskList = s_bl?.Task.ReadAll()!;
                    MessageBox.Show("Task added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _taskListWindow!.TaskList = updatedTaskList;
                }
                else
                {
                    if (isUserValue == true)
                    {
                        s_bl.Task.Update(Task);
                        IEnumerable<BO.TaskInList> updatedTaskList = s_bl?.Task.ReadAll()!;
                        MessageBox.Show("Task updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        _taskListWindow!.TaskList = updatedTaskList;
                    }
                    else if (isUserValue == false)
                    {
                        s_bl.Task.UpdateStartDate(Task.Id, DateTime.Now);
                        s_bl.Engineer.UpdateEngineerToTask(Task.Engineer!.Id!, Task.Id);
                        //IEnumerable<BO.TaskInList> updatedTaskList = s_bl?.Task.ReadAll()!;
                        MessageBox.Show("Engineer in Task and the Date of Task updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        //  _taskListWindow!.TaskList = updatedTaskList;
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Operation Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Do you want to delete the Task?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes && (sender as Button)!.Content.ToString() == "Delete")
                {
                    s_bl.Task.Delete(Task.Id);
                    IEnumerable<BO.TaskInList> updatedTaskList = s_bl?.Task.ReadAll()!;
                    MessageBox.Show("Task delete successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _taskListWindow!.TaskList = updatedTaskList;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Operation Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddDepBtn_Click(object sender, RoutedEventArgs e)
        {
            TasksList = s_bl.Task.ReadAll();
            OpenDialogHost = true;
        }
        private void CompleteDateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (isUserValue == false)
                // {
                s_bl.Task.UpdateCompleteDate(Task.Id, DateTime.Now);
                IEnumerable<BO.TaskInList> updatedTaskList = s_bl?.Task.ReadAll()!;
                MessageBox.Show("The Complete Date of Task updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                //_taskListWindow!.TaskList = updatedTaskList;
                // }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Operation Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditDependency(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            try
            {
                Label label = ((Label)sender);

                if (label.Content is null)
                    return;

                if (label.Background == Brushes.Green)
                {
                    Task.Dependencies!.RemoveAll(x => x.Id == ((TaskInList)label.Content).Id);
                    label.Background = Brushes.White;
                }
                else
                {
                    Task.Dependencies!.Add((TaskInList)label.Content);
                    label.Background = Brushes.Green;
                }

                BO.Task tmp = Task;
                Task = new();
                Task = tmp;
            }
            catch { }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            if (e.Delta > 0)
            {
                // Scroll up
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 20);
            }
            else
            {
                // Scroll down
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 20);
            }

            // Mark the event as handled to prevent the default scrolling behavior
            e.Handled = true;
        }
    }
}
