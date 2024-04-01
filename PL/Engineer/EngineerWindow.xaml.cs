using BO;
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

        Action<int, bool?> _collectionChange;
        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        public static readonly DependencyProperty EngineerProperty =
            DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow));

        public EngineerWindow(Action<int, bool?> collectionChange, int engineerId = 0)
        {
            _collectionChange = collectionChange;
            InitializeComponent();

            if (engineerId == 0)
                Engineer = new BO.Engineer();
            else
            {
                try
                {
                    Engineer = s_bl.Engineer.Read(engineerId)!;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }

            }

        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((sender as Button)!.Content.ToString() == "Add")
                {
                    s_bl.Engineer.Create(Engineer);
                    MessageBox.Show("Engineer added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _collectionChange(Engineer.Id, true);
                }
                else
                {
                    s_bl.Engineer.Update(Engineer);
                    MessageBox.Show("Engineer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _collectionChange(Engineer.Id, false);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Operation Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Do you want to delete the Engineer?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes && (sender as Button)!.Content.ToString() == "Delete")
                {
                    s_bl.Engineer.Delete(Engineer.Id);
                    MessageBox.Show("Engineer delete successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    _collectionChange(Engineer.Id, null);
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
