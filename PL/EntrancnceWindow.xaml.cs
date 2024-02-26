using BO;
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
using System.Windows.Threading;

namespace PL
{
    /// <summary>
    /// Interaction logic for EntrancnceWindow.xaml
    /// </summary>
    public partial class EntrancnceWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        //public static readonly DependencyProperty CurrentTimeProperty =
        //   DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(DateTime.Now));

        //public DateTime CurrentTime
        //{
        //    get { return (DateTime)GetValue(CurrentTimeProperty); }
        //    set { SetValue(CurrentTimeProperty, value); }
        //}

        //private DispatcherTimer _timer;
        public EntrancnceWindow()
        {
            InitializeComponent();
            //_timer = new DispatcherTimer();
            //_timer.Interval = TimeSpan.FromSeconds(1);
            //_timer.Tick += Timer_Tick!;
            //_timer.Start();
        }

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    // Update CurrentTime property with current time
        //    CurrentTime = DateTime.Now;
        //}
        public BO.User User
        {
            get { return (BO.User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register(nameof(BO.User), typeof(IEnumerable<BO.User>), typeof(EntrancnceWindow));

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                Grid grid = (Grid)button.Parent;

                TextBox textBoxUser = (TextBox)grid.Children[2];
                PasswordBox passwordBoxUser = (PasswordBox)grid.Children[3];

                string nameUser = textBoxUser.Text;
                string codeUser = passwordBoxUser.Password;

                User = s_bl.User.Read(codeUser)!;
                if (User.Id != codeUser) 
                {
                    MessageBox.Show("Wrong Code", "Pleasr try again", MessageBoxButton.OK);
                }
                else
                {
                    new MainWindow(User).Show();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Operation Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        //private void BtnAdvanceYear_Click(object sender, RoutedEventArgs e)
        //{
        //    s_bl.AdvanceTimeByYear(1);
        //    UpdateCurrentTime();
        //}

        //private void BtnAdvanceHour_Click(object sender, RoutedEventArgs e)
        //{
        //    s_bl.AdvanceTimeByHour(1);
        //    UpdateCurrentTime();
        //}

        //private void BtnAdvanceDay_Click(object sender, RoutedEventArgs e)
        //{
        //    s_bl.AdvanceTimeByDay(1);
        //    UpdateCurrentTime();
        //}

        //private void BtnReset_Click(object sender, RoutedEventArgs e)
        //{
        //    s_bl.InitializeTime(DateTime.Now);
        //    UpdateCurrentTime();
        //}
        //// Update the CurrentTime property
        //private void UpdateCurrentTime()
        //{
        //    CurrentTime = s_bl.Clock;
        //}
    }
}
