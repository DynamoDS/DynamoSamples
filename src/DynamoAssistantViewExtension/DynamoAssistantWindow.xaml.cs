﻿using System.Windows;

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

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SendMessage(UserInput.Text);
            ViewModel.UserInput = string.Empty;
        }

        private void DescribeGraphButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DescribeGraph();
        }

        private void OptimizeGraphButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OptimizeGraph();
        }
    }
}
