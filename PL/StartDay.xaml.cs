using DalApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for StartDay.xaml
    /// </summary>
    public partial class StartDay : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        private DateTime? _startDate;

        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = default!;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public StartDay()
        {
            InitializeComponent();
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = ((DatePicker)sender).SelectedDate;
            
            if (selectedDate.HasValue)
            {
             
                s_bl.SetProjectStartDate(selectedDate.Value);
                //IEnumerable<DO.Task> allTasks = _dal.Task.ReadAll();

                //foreach (var task in allTasks) 
                //{
                //    s_bl.Task.UpdateStartDate()
                //}
                MessageBox.Show("Start date set successfully!", "Success", MessageBoxButton.OK);
                this.Close();
            }
        }
    }
}
