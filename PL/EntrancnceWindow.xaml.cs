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

namespace PL
{
    /// <summary>
    /// Interaction logic for EntrancnceWindow.xaml
    /// </summary>
    public partial class EntrancnceWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public EntrancnceWindow()
        {
            InitializeComponent();
        }
        public BO.User user
        {
            get { return (BO.User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(BO.User), typeof(EntrancnceWindow), new PropertyMetadata(null));

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                user = s_bl.User.Read(UserName.Text)!;
                if (user.Id != Code.Text) 
                {
                    MessageBox.Show("Wrong Code", "Pleasr try again", MessageBoxButton.OK);
                }
                else
                {
                    new MainWindow(user).Show();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Operation Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
