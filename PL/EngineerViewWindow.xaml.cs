using BO;
using DO;
using PL.User;
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
    /// Interaction logic for EngineerViewWindow.xaml
    /// </summary>
    public partial class EngineerViewWindow : Window
    {
        private readonly BO.User user;
        private UserWindow userWindow;

        public EngineerViewWindow(BO.User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            new EntrancnceWindow().Show();
            this.Close(); 
        }

        private void BtnUppUse_Click(object sender, RoutedEventArgs e)
        {
            // Check if the userWindow instance is null or closed
            if (userWindow == null || !userWindow.IsVisible)
            {
                userWindow = new UserWindow();
            }

            userWindow.User = user;
            userWindow.Show();
        }
    }
}
