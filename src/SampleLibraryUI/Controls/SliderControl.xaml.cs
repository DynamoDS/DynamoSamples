using System.Windows.Controls;

namespace SampleLibraryUI.Controls
{
    /// <summary>
    /// Interaction logic for SliderControl.xaml
    /// </summary>
    public partial class SliderControl : UserControl
    {
        public SliderControl()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e){ }
    }
}
