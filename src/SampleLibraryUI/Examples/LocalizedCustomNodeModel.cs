using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleLibraryUI.Examples
{
    /// <summary>
    /// This example node uses .net .resx files and generated satellite assemblies to perform runtime lookup of localized content
    /// depending on the culture of the system Dynamo is running on.
    /// Read more: https://docs.microsoft.com/en-us/dotnet/framework/resources/creating-resource-files-for-desktop-apps
    /// You can use the -l "es-ES" flag when starting DynamoSandbox.exe to replace the English strings with Spanish ones.
    /// For more info on the CLI interface read more: https://github.com/DynamoDS/Dynamo/wiki/Dynamo-Command-Line-Interface
    /// </summary>
    [NodeName("LocalizedNode")]
    [NodeDescription("CustomNodeModelDescription", typeof(Properties.Resources))]
    [OutPortNames("string")]
    [OutPortTypes("string")]
    [IsDesignScriptCompatible]
    public class LocalizedCustomNodeModel : NodeModel
    {
        public LocalizedCustomNodeModel()
        {
            RegisterAllPorts();
        }

        [JsonConstructor]
        public LocalizedCustomNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts) { }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            //return a localized string, that is defined in the resx file/resource assembly.
            return new List<AssociativeNode> { AstFactory.BuildAssignment(this.AstIdentifierForPreview, AstFactory.BuildPrimitiveNodeFromObject(Properties.Resources.LocalStringResult)) };
        }
    }
}
