using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleExtension
{
    public class Extension : IExtension
    {
        public List<NodeModel> nodes = new List<NodeModel>();
        public bool readyCalled = false;

        public string Name
        {
            get
            {
                return "testExtension";
            }
        }

        public string UniqueId
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

        public void Dispose()
        {
           
        }

        public void Ready(ReadyParams sp)
        {
            sp.CurrentWorkspaceModel.NodeAdded += CurrentWorkspaceModel_NodeAdded;
            this.readyCalled = true;
        }

        private void CurrentWorkspaceModel_NodeAdded(Dynamo.Graph.Nodes.NodeModel obj)
        {
            this.nodes.Add(obj);
        }

        public void Shutdown()
        {
           
        }

        public void Startup(StartupParams sp)
        {
        
        }
    }
}
