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

namespace PL.User
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.User User
        {
            get { return (BO.User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("User", typeof(BO.User), typeof(UserWindow), new PropertyMetadata(null));
        public UserWindow()
        {
            InitializeComponent();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Grid grid = (Grid)btn.Parent;
            TextBox userNameTextBox = (TextBox)grid.Children[4];
            TextBox pasTextBox = (TextBox)grid.Children[5];
            TextBox nameTextBox = (TextBox)grid.Children[6];
            TextBox idTextBox = (TextBox)grid.Children[7];
            int idE = int.Parse(idTextBox.Text);
            try
            {

                BO.Engineer eng = s_bl.Engineer.Read(idE)!;
                string id = (idTextBox.Text);
                // Check if the engineer exists in the list of employees
                if (eng.Name != nameTextBox.Text)
                {
                    MessageBox.Show("Engineer with the provided ID and Name does not exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Add the user to the list of users
                BO.User user = new() { UserName = userNameTextBox.Text, Id = id, Password = pasTextBox.Text, IsAdmin = false };
                if (user == null)
                {
                    s_bl.User.Create(user!);
                }
                else
                {
                    s_bl.User.Update(user!);
                }
                // Close the current window
                new EngineerViewWindow(user!).Show();
                this.Close();

                // Navigate to the next window or perform any other action
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Operation Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            new EntrancnceWindow().Show();
            this.Close();
        }
    }
}
