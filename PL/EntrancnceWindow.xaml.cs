using BO;
using PL.TaskForList;
using PL.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
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
using System.Windows.Controls.Primitives;


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
            CodeUser = new SecureString();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTime = CurrentTime.AddSeconds(1);
        }

        private BO.User? _user;

        public BO.User User
        {
            get { return _user!; }
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

        private String _nameUser;
        private SecureString _codeUser = new SecureString();

        public String NameUser
        {
            get { return _nameUser!; }
            set
            {
                if (_nameUser != value)
                {
                    _nameUser = value;
                    OnPropertyChanged(nameof(NameUser));
                }
            }
        }

        public SecureString CodeUser
        {
            get { return _codeUser; }
            set
            {
                if (_codeUser != value)
                {
                    _codeUser = value!;
                    OnPropertyChanged(nameof(CodeUser));
                }
            }
        }
        private string? _password;

        public string Password
        {
            get => _password!;
            set
            {
                _password = value;
                CodeUser = new SecureString();
                foreach (char c in value)
                {
                    CodeUser.AppendChar(c);
                }
                CodeUser.MakeReadOnly();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private string ConvertSecureStringToString(SecureString secureString)
        {
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(secureString);//System.ArgumentNullException: 'Value cannot be null. Arg_ParamName_Name'
            try
            {
                return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
                Password = passwordBox.Password;
        }
        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string code = ConvertSecureStringToString(CodeUser);
                User = s_bl.User.ReadByNameAndPassword(NameUser, code);

                if (User.Password != code || User.UserName != NameUser)
                {
                    MessageBox.Show("Please try again", " User name or password is incorrect", MessageBoxButton.OK);
                }


                if (User.IsAdmin)
                {
                    new MainWindow(User).Show();
                    this.Close();

                }
                if (NameUser != "" || code != "")
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

