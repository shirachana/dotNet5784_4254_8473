using PL.Engineer;
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
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }

        public static readonly DependencyProperty TaskProperty =
            DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));
        private TaskListWindow _taskListWindow;
        public TaskWindow(TaskListWindow taskListWindow, int taskId = 0)
        {
            _taskListWindow = taskListWindow;
            if (taskId == 0)
            {
                // Create a new object with default values
                Task = new BO.Task();
            }
            else
            {
                try
                {
                    // Retrieve the existing engineer object from the BL based on the engineerId
                    Task =s_bl.Task.Read(taskId)!;
                    
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., display an error message
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Close the window or handle the error as appropriate
                    Close();
                }

            }

            if (Task.Engineer == null)
                Task.Engineer = new();
            InitializeComponent();
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
                    _taskListWindow.TaskList = updatedTaskList;
                }
                else
                {
                    s_bl.Task.Update(Task);
                    IEnumerable<BO.TaskInList> updatedTaskList = s_bl?.Task.ReadAll()!;
                    MessageBox.Show("Task updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _taskListWindow.TaskList = updatedTaskList;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Operation Fail", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    _taskListWindow.TaskList = updatedTaskList;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Operation Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonAddDep_Click(object sender, RoutedEventArgs e) 
        {

        }
        private void ListBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
