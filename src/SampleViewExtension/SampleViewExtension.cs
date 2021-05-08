using System;
using System.Windows;
using System.Windows.Controls;
using Dynamo.Wpf.Extensions;

namespace SampleViewExtension
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
    public class SampleViewExtension : ViewExtensionBase
    {
        private MenuItem sampleMenuItem;

        public override void Dispose()
        {
        }

        public override void Startup(ViewStartupParams p)
        {
        }

        public override void Loaded(ViewLoadedParams p)
        {
            // Save a reference to your loaded parameters.
            // You'll need these later when you want to use
            // the supplied workspaces

            var viewModel = new SampleWindowViewModel(p);
            var window = new SampleWindow
            {
                // Set the data context for the main grid in the window.
                MainGrid = { DataContext = viewModel },

                // Set the owner of the window to the Dynamo window.
                Owner = p.DynamoWindow
            };

            sampleMenuItem = new MenuItem { Header = "Show View Extension Sample Window", IsCheckable = true };
            sampleMenuItem.Checked += (sender, args) => p.AddToExtensionsSideBar(this, window);
            sampleMenuItem.Unchecked += (sender, args) => p.CloseExtensioninInSideBar(this);
            p.AddExtensionMenuItem(sampleMenuItem);
        }

        public override void Shutdown()
        {
        }

        public override void Closed()
        {
            if (sampleMenuItem != null)
            {
                sampleMenuItem.IsChecked = false;
            }
        }

        public override string UniqueId
        {
            get
            {
                return "61BB15A8-9B6B-4AB0-8D75-F7C34E0B9112";
            }  
        } 

        public override string Name
        {
            get
            {
                return "Sample View Extension";
            }
        } 

    }
}
