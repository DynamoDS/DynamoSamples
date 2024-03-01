using System.Windows;

namespace DynamoAssistant
{
    /// <summary>
    /// Interaction logic for SampleWindow.xaml
    /// </summary>
    public partial class DynamoAssistantWindow : Window
    {
        internal DynamoAssistantWindowViewModel ViewModel => MainGrid.DataContext as DynamoAssistantWindowViewModel;
        public DynamoAssistantWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SendMessage(UserInput.Text);
            ViewModel.UserInput = string.Empty;
        }
    }
}
