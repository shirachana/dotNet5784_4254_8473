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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));
        public EngineerWindow(int engineerId = 0)
        {
            InitializeComponent();
            if (engineerId == 0)
            {
                // Create a new object with default values
                Engineer = new BO.Engineer();
            }
            else
            {
                try
                {
                    // Retrieve the existing engineer object from the BL based on the engineerId
                    Engineer = s_bl.Engineer.Read(engineerId)!;
                }
                catch (Exception ex)
                {
                    // Handle exceptions, e.g., display an error message
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Close the window or handle the error as appropriate
                    Close();
                }

            }

        }
        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Engineer.Id == 0)
                {
                    s_bl.Engineer.Create(Engineer);
                    MessageBox.Show("Engineer added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    s_bl.Engineer.Update(Engineer);
                    MessageBox.Show("Engineer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
