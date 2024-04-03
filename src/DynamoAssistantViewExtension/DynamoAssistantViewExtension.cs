using System.Windows.Controls;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;

namespace DynamoAssistant
{
    /// <summary>
    /// The View Extension framework for Dynamo allows you to extend
    /// the Dynamo UI by registering custom MenuItems. A ViewExtension has 
    /// two components, an assembly containing a class that implements 
    /// IViewExtension or extends ViewExtensionBase, and a ViewExtensionDefinition
    /// XML file used to instruct Dynamo where to find the class containing the
    /// View Extension implementation. The ViewExtensionDefinition XML file must
    /// be located in your [dynamo]\viewExtensions folder.
    /// 
    /// This sample demonstrates a View Extension implementation which inherits from
    /// ViewExtensionBase. It adds a user interface to Dynamo's extension sidebar
    /// when its MenuItem is clicked. 
    /// The Window created tracks the number of nodes in the current workspace, 
    /// by handling the workspace's NodeAdded and NodeRemoved events.
    /// </summary>
    public class DynamoAssistantViewExtension : ViewExtensionBase
    {
        private MenuItem assistantMenuItem;

        public override void Dispose()
        {
            // Do nothing for now
        }

        public override void Startup(ViewStartupParams p)
        {
        }

        public override void Loaded(ViewLoadedParams p)
        {
            // Save a reference to your loaded parameters.
            // You'll need these later when you want to use
            // the supplied workspaces

            var viewModel = new DynamoAssistantWindowViewModel(p);
            var window = new DynamoAssistantWindow
            {
                DataContext = viewModel,
                // Set the data context for the main grid in the window.
                MainGrid = { DataContext = viewModel },

                // Set the owner of the window to the Dynamo window.
                Owner = p.DynamoWindow
            };
            viewModel.dynamoViewModel = p.DynamoWindow.DataContext as DynamoViewModel;
            assistantMenuItem = new MenuItem { Header = "Dynamo Gen-AI assistant", IsCheckable = true };
            assistantMenuItem.Checked += (sender, args) => p.AddToExtensionsSideBar(this, window);
            assistantMenuItem.Unchecked += (sender, args) => p.CloseExtensioninInSideBar(this);
            p.AddExtensionMenuItem(assistantMenuItem);
        }

        public override void Shutdown()
        {
        }

        public override void Closed()
        {
            if (assistantMenuItem != null)
            {
                assistantMenuItem.IsChecked = false;
            }
        }

        public override string UniqueId
        {
            get
            {
                return "DA05ED05-6842-4CF0-9ADC-575888A01FEC";
            }
        }

    public override string Name
        {
            get
            {
                return "Gen-AI assistant";
            }
        }

    }
}
