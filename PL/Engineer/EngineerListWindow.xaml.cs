using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public ObservableCollection<BO.Engineer> EngineerList
        {
            get { return (ObservableCollection<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(ObservableCollection<BO.Engineer>), typeof(EngineerListWindow));

        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = new(s_bl?.Engineer.ReadAll()!);
        }

        public BO.EngineerExperience experience { get; set; } = BO.EngineerExperience.None;

        private void cbExperienceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (experience == BO.EngineerExperience.None)
                EngineerList = new(s_bl?.Engineer.ReadAll()!);
            else
                EngineerList = new(s_bl?.Engineer.ReadAll(item => item.Level == experience)!);
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            new EngineerWindow(CollectionChange).ShowDialog();
        }

        private void CollectionChange(int id, bool? newObj = null)
        {
            try
            {
                if (newObj is null)//remove engineer
                {
                    BO.Engineer oldEnj = EngineerList.First(x => x.Id == id);
                    EngineerList.Remove(oldEnj);
                }
                if (newObj == false)
                {
                    BO.Engineer eng = s_bl.Engineer.Read(id)!;
                    BO.Engineer oldEnj = EngineerList.First(x => x.Id == id);
                    int index = EngineerList.IndexOf(oldEnj);
                    EngineerList[index] = eng;
                }
                else
                {
                    BO.Engineer eng = s_bl.Engineer.Read(id)!;
                    EngineerList.Add(eng);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Please try again", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateBtn_Click(object sender, MouseButtonEventArgs e)
        {

            BO.Engineer? EngineerList = (sender as ListView)?.SelectedItem as BO.Engineer;
            if (EngineerList != null)
            {
                EngineerWindow window = new EngineerWindow(CollectionChange, EngineerList.Id);
                window.ShowDialog();
            }
        }
    }
}
