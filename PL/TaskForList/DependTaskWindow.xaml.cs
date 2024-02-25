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
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.TaskInList> DepTaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(DepTaskListProperty); }
            set { SetValue(DepTaskListProperty, value); }
        }
        // Assuming your data is in an ObservableCollection<MyItem>
        public static readonly DependencyProperty DepTaskListProperty =
            DependencyProperty.Register(nameof(DepTaskList), typeof(IEnumerable<BO.TaskInList>), typeof(DependTaskWindow));




        public List<TaskInList> selectedTasks
        {
            get { return (List<TaskInList>)GetValue(selectedListProp); }
            set { SetValue(selectedListProp, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty selectedListProp =
            DependencyProperty.Register(nameof(selectedTasks), typeof(List<TaskInList>), typeof(DependTaskWindow));



        public DependTaskWindow(IEnumerable<TaskInList> dependencies)
        {
            selectedTasks = dependencies.ToList() ?? new();
            InitializeComponent();
            DepTaskList = s_bl?.Task.ReadAll()!;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox? checkBox = sender as CheckBox;
            if (checkBox.IsChecked == true)
            {
                selectedTasks.Add(checkBox.Tag as TaskInList);
            }
            else
            {
                selectedTasks.Remove(checkBox.Tag as TaskInList);
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Operation Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Close();
        }

    }
}

