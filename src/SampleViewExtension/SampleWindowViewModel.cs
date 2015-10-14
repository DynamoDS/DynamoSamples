using System;
using System.Linq;
using Dynamo.Core;
using Dynamo.Extensions;

namespace SampleViewExtension
{
    public class SampleWindowViewModel : NotificationObject, IDisposable
    {
        private string selectedNodesText = "Begin selecting ";
        private ReadyParams readyParams;

        public string SelectedNodesText = @"There are {readyParams.CurrentWorkspaceModel.Nodes.Count()} nodes in the workspace.";

        public SampleWindowViewModel(ReadyParams p)
        {
            readyParams = p;
            p.CurrentWorkspaceModel.NodeAdded += CurrentWorkspaceModel_NodesChanged;
            p.CurrentWorkspaceModel.NodeRemoved += CurrentWorkspaceModel_NodesChanged;
        }

        private void CurrentWorkspaceModel_NodesChanged(Dynamo.Models.NodeModel obj)
        {
            RaisePropertyChanged("SelectedNodesText");
        }

        public void Dispose()
        {
            readyParams.CurrentWorkspaceModel.NodeAdded -= CurrentWorkspaceModel_NodesChanged;
            readyParams.CurrentWorkspaceModel.NodeRemoved -= CurrentWorkspaceModel_NodesChanged;
        }
    }
}
