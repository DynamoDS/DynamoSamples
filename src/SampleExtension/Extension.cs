using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleExtension
{
    /// <summary>
    /// The Extension framework for Dynamo allowed you to extend
    /// Dynamo by loading your own classes that can interact with Dynamo's API.
    /// An Extension has two components, an assembly containing your class and
    /// an xml manifest file telling Dynamo where to find your assembly. Extension
    /// manifests are loaded from the Dynamo/Extensions folder or from package/extra
    /// folders.
    /// 
    /// This sample demonstrates a simple IExtension which tracks nodes added to the workspace.
    /// </summary>
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
        /// <summary>
        /// Ready is called when the DynamoModel is finished being built, or when the extension is installed
        /// sometime after the DynamoModel is alreay built. ReadyParams provide access to references like the
        /// CurrentWorkspace.
        /// </summary>
        /// <param name="sp"></param>
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
