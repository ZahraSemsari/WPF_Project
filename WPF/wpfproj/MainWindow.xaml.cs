using System.Windows;
using TaskManager.ViewModels;

namespace TaskManager
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = (MainViewModel)DataContext;
        }

        private async void AddListButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.AddList();
        }

        private async void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.AddTask(sender, e);
        }

        private async void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.DeleteTask();
        }

        private async void RenameTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedTask != null)
            {
                await ViewModel.RenameTask(ViewModel.SelectedTask.Name);
            }
        }

        private async void DeleteListButton_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.DeleteList();
        }
    }
}
