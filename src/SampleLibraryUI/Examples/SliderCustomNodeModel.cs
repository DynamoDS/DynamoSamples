using System;
using System.Collections.Generic;
using System.Windows;
using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using ProtoCore.AST.AssociativeAST;
using SampleLibraryUI.Controls;
using SampleLibraryUI.Properties;
using SampleLibraryZeroTouch;
using Newtonsoft.Json;

namespace SampleLibraryUI.Examples
{
     /*
      * This exmple shows how to create a UI node for Dynamo
      * which loads custom data-bound UI into the node's view
      * at run time. 
     
      * Nodes with custom UI follow a different loading path
      * than zero touch nodes. The assembly which contains
      * this node needs to be located in the 'nodes' folder in
      * Dynamo in order to be loaded at startup.
     
      * Dynamo uses the MVVM model of programming, 
      * in which the UI is data-bound to the view model, which
      * exposes data from the underlying model. Custom UI nodes 
      * are a hybrid because NodeModel objects already have an
      * associated NodeViewModel which you should never need to
      * edit. So here we will create a data binding between 
      * properties on our class and our custom UI.
     */

    // The NodeName attribute is what will display on 
    // top of the node in Dynamo
    [NodeName("Slider Custom Node Model")]
    // The NodeCategory attribute determines how your
    // node will be organized in the library. You can
    // specify your own category or by default the class
    // structure will be used.  You can no longer 
    // add packages or custom nodes to one of the 
    // built-in OOTB library categories.
    [NodeCategory("SampleLibraryUI.Examples")]
    // The description will display in the tooltip
    // and in the help window for the node.
    [NodeDescription("A sample UI node which displays custom UI.")]
    // Specifying InPort and OutPort types simply
    // adds these types to the help window for the
    // node when hovering the name in the library.
    //[InPortTypes("double")]
    [OutPortTypes("double", "double")]
    // Add the IsDesignScriptCompatible attribute to ensure
    // that it gets loaded in Dynamo.
    [IsDesignScriptCompatible]
    public class SliderCustomNodeModel : NodeModel
    {
        #region private members

        private double sliderValue;

        #endregion

        #region properties

        /// <summary>
        /// A value that will be bound to our
        /// custom UI's slider.
        /// </summary>
        public double SliderValue
        {
            get { return sliderValue; }
            set
            {
                sliderValue = value;
                RaisePropertyChanged("SliderValue");

                OnNodeModified();
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// The constructor for a NodeModel is used to create
        /// the input and output ports and specify the argument
        /// lacing. It gets invoked when the node is added to 
        /// the graph from the library or through copy/paste.
        /// </summary>
        public SliderCustomNodeModel()
        {
            // When you create a UI node, you need to do the
            // work of setting up the ports yourself. To do this,
            // you can populate the InPorts and the OutPorts
            // collections with PortData objects describing your ports.

            // Nodes can have an arbitrary number of inputs and outputs.
            // If you want more ports, just create more PortData objects.
            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("upper value", "returns a 0-10 double value")));
            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("lower value", "returns a 0-100 double value")));

            // This call is required to ensure that your ports are
            // properly created.
            RegisterAllPorts();

            // The arugment lacing is the way in which Dynamo handles
            // inputs of lists. If you don't want your node to
            // support argument lacing, you can set this to LacingStrategy.Disabled.
            ArgumentLacing = LacingStrategy.Disabled;

            // Set initial slider value.
            sliderValue = 4;
        }

        // Starting with Dynamo v2.0 you must add Json constructors for all nodeModel
        // dervived nodes to support the move from an Xml to Json file format.  Failing to
        // do so will result in incorrect ports being generated upon serialization/deserialization.
        // This constructor is called when opening a Json graph.
        [JsonConstructor]
        SliderCustomNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts) { }

        #endregion

        #region public methods

        /// <summary>
        /// BuildOutputAst is where the outputs of this node are calculated.
        /// This method is used to do the work that a compiler usually does 
        /// by parsing the inputs List inputAstNodes into an abstract syntax tree.
        /// </summary>
        /// <param name="inputAstNodes"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            // When you create your own UI node you are responsible
            // for generating the abstract syntax tree (AST) nodes which
            // specify what methods are called, or how your data is passed
            // when execution occurs.

            // WARNING!!!
            // Do not throw an exception during AST creation. If you
            // need to convey a failure of this node, then use
            // AstFactory.BuildNullNode to pass out null.

            // We create a DoubleNode to wrap the value 'sliderValue' that
            // we've stored in a private member.

            var doubleNode = AstFactory.BuildDoubleNode(sliderValue);

            // A FunctionCallNode can be used to represent the calling of a 
            // function in the AST. The method specified here must live in 
            // a separate assembly and have been loaded by Dynamo at the time 
            // that this AST is built. If the method can't be found, you'll get 
            // a "De-referencing a non-pointer warning."

            var funcNode = AstFactory.BuildFunctionCall(
                new Func<double,double>(SampleUtilities.MultiplyInputByNumber), 
                new List<AssociativeNode>(){doubleNode});

            // Using the AstFactory class, we can build AstNode objects
            // that assign doubles, assign function calls, build expression lists, etc.
            return new[]
            {
                // In these assignments, GetAstIdentifierForOutputIndex finds 
                // the unique identifier which represents an output on this node
                // and 'assigns' that variable the expression that you create.

                // For the first node, we'll just pass through the 
                // input provided to this node.
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0), AstFactory.BuildDoubleNode(sliderValue)),

                // For the second node, we'll build a double node that 
                // passes along our value for multipled value.
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(1), funcNode)
            };
        }

        #endregion
    }

    /// <summary>
    ///     View customizer for CustomNodeModel Node Model.
    /// </summary>
    public class SliderCustomNodeModelNodeViewCustomization : INodeViewCustomization<SliderCustomNodeModel>
    {
        /// <summary>
        /// At run-time, this method is called during the node 
        /// creation. Here you can create custom UI elements and
        /// add them to the node view, but we recommend designing
        /// your UI declaratively using xaml, and binding it to
        /// properties on this node as the DataContext.
        /// </summary>
        /// <param name="model">The NodeModel representing the node's core logic.</param>
        /// <param name="nodeView">The NodeView representing the node in the graph.</param>
        public void CustomizeView(SliderCustomNodeModel model, NodeView nodeView)
        {
            // The view variable is a reference to the node's view.
            // In the middle of the node is a grid called the InputGrid.
            // We reccommend putting your custom UI in this grid, as it has
            // been designed for this purpose.

            // Create an instance of our custom UI class (defined in xaml),
            // and put it into the input grid.
            var sliderControl = new SliderControl();
            nodeView.inputGrid.Children.Add(sliderControl);

            // Set the data context for our control to be the node model.
            // Properties in this class which are data bound will raise 
            // property change notifications which will update the UI.
            sliderControl.DataContext = model;
        }

        /// <summary>
        /// Here you can do any cleanup you require if you've assigned callbacks for particular 
        /// UI events on your node.
        /// </summary>
        public void Dispose() { }
    }

}
