using PL.TaskForList;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for GantWindow.xaml
    /// </summary>
    public partial class GantWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public IEnumerable<BO.Task> TaskOfList
        {
            get { return (IEnumerable<BO.Task>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskOfList", typeof(IEnumerable<BO.Task>), typeof(GantWindow), new PropertyMetadata(null));

        //private BO.Engineer? Engineer;
        // TaskListWindow? _taskListWindow;

        public GantWindow()
        {
            DataContext = this;
            TaskOfList = s_bl?.Task.GetDetailsToGantt()!;
            InitializeComponent();
        }
        // Calculate Length of Border
        //public double CalculateBorderLength(BO.Task task)
        //{
        //    // Calculate the start date of the task
        //    DateTime? startDate = task.StartDate;

        //    // Determine the required effort time of the task
        //    TimeSpan? requiredEffortTime = task.RequiredEffortTime;

        //    // Consider the start dates and required effort times of any dependent tasks
        //    foreach (BO.Task dependentTask in task.Dependencies!)
        //    {
        //        // Update the start date if the dependent task starts later than the current start date
        //        if (dependentTask.StartDate > startDate)
        //        {
        //            startDate = dependentTask.StartDate;
        //        }

        //        // Update the required effort time if the dependent task's effort time is longer
        //        if (dependentTask.RequiredEffortTime > requiredEffortTime)
        //        {
        //            requiredEffortTime = dependentTask.RequiredEffortTime;
        //        }
        //    }

        //    // Calculate the total length of the border based on start date and required effort time
        //    // You can use any appropriate formula here based on your requirements
        //    double totalLength = /* Your calculation logic here */;

        //    return totalLength;
        //}

        //// Determine Color of Border using TaskColorConverterKey
        //public SolidColorBrush DetermineBorderColor(Task task)
        //{
        //    // Use the TaskColorConverterKey to determine the color of the border
        //    SolidColorBrush borderColor = TaskColorConverterKey.Convert(task, typeof(SolidColorBrush), null, CultureInfo.CurrentCulture) as SolidColorBrush;

        //    return borderColor;
        //}

    }
}
