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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));
        public BO.EngineerExperience experience { get; set; } = BO.EngineerExperience.None;

        private void cbExperienceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (experience == BO.EngineerExperience.None) ?
                 s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == experience)!;

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            new EngineerWindow(this).ShowDialog();
        }

        private void UpdateBtn_Click(object sender, MouseButtonEventArgs e)
        {

            BO.Engineer? EngineerList = (sender as ListView)?.SelectedItem as BO.Engineer;
            if (EngineerList != null)
            {
                // Create a new window with the selected item in update mode
                EngineerWindow window = new EngineerWindow(this,EngineerList.Id);
                window.ShowDialog();
            }
        }
    }
}
