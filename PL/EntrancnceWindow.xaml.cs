using BO;
using PL.TaskForList;
using PL.User;
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
using System.Windows.Threading;

namespace PL.User
{
    /// <summary>
    /// Interaction logic for EntrancnceWindow.xaml
    /// </summary>
    public partial class EntrancnceWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public static readonly DependencyProperty CurrentTimeProperty =
           DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(EntrancnceWindow), new PropertyMetadata(DateTime.Now));

      


        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        private DispatcherTimer _timer;
        public EntrancnceWindow()
        {
            InitializeComponent();
            _user = new BO.User();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick!;
            _timer.Start();
            DataContext = this;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
                CurrentTime = CurrentTime.AddSeconds(1);
        }

        private BO.User _user;

        public BO.User User
        {
            get { return _user; }
            set
            {
                if (value is BO.User)
                    _user = value;
                else
                    throw new ArgumentException("Invalid user type");
            }
        }

        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(IEnumerable<BO.User>), typeof(EntrancnceWindow));

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
                User = s_bl.User.ReadByNameAndPassword(nameUser, codeUser);

                if (User.Password != codeUser || User.UserName != nameUser)
                {
                    MessageBox.Show("Please try again", " User name or password is incorrect", MessageBoxButton.OK);
                }


                if (User.IsAdmin)
                {
                    new MainWindow(User).Show();
                    this.Close();

                }
                if (nameUser != "" || codeUser != "")
                    if (!User.IsAdmin)
                    {
                        new EngineerViewWindow(User).Show();
                        this.Close();
                    }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Please try again", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {

            new UserWindow().Show();
            this.Close();
        }

        private void BtnAdvanceYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByYear(1);
            UpdateCurrentTime();
        }

        private void BtnAdvanceHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByHour(1);
            UpdateCurrentTime();
        }

        private void BtnAdvanceDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AdvanceTimeByDay(1);
            UpdateCurrentTime();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            s_bl.InitializeTime(DateTime.Now);
            UpdateCurrentTime();
        }
        private void UpdateCurrentTime()
        {
            CurrentTime = s_bl.Clock;
        }
    }
}

