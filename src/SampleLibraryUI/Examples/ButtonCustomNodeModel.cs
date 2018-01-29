using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using ProtoCore.AST.AssociativeAST;
using SampleLibraryUI.Controls;
using SampleLibraryZeroTouch;
using Newtonsoft.Json;

namespace SampleLibraryUI.Examples
{
    /*
     * This exmple shows how to create a UI node for Dynamo
     * which loads custom data-bound UI into the node's view
     * at run time. 

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
    [NodeName("Button Custom Node Model")]
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
    [InPortTypes("string")]
    [OutPortTypes("string")]
    // Add the IsDesignScriptCompatible attribute to ensure
    // that it gets loaded in Dynamo.
    [IsDesignScriptCompatible]
    public class ButtonCustomNodeModel : NodeModel
    {
        #region private members

        private string buttonText;
        private string windowText;
        private const string defaultButtonText = "Default Button Text";
        private const string defaultWindowText = "Default Window Text";

        #endregion

        #region properties

        /// <summary>
        /// Text that will appear on the button
        /// on our node.
        /// </summary>
        public string ButtonText
        {
            get { return buttonText; }
            set
            {
                buttonText = value;

                // Raise a property changed notification
                // to alert the UI that an element needs
                // an update.
                RaisePropertyChanged("ButtonText");
            }
        }

        /// <summary>
        /// Text that will appear in the message
        /// box when button is pressed.
        /// </summary>
        public string WindowText
        {
            get { return windowText; }
            set
            {
                windowText = value;
            }
        }

        /// <summary>
        /// DelegateCommand objects allow you to bind
        /// UI interaction to methods on your data context.
        /// </summary>
        [JsonIgnore]
        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand ButtonCommand { get; set; }

        #endregion

        #region constructor

        /// <summary>
        /// The constructor for a NodeModel is used to create
        /// the input and output ports and specify the argument
        /// lacing. It gets invoked when the node is added to 
        /// the graph from the library or through copy/paste.
        /// </summary>
        public ButtonCustomNodeModel()
        {
            // When you create a UI node, you need to do the
            // work of setting up the ports yourself. To do this,
            // you can populate the InPorts and the OutPorts
            // collections with PortData objects describing your ports.
            InPorts.Add(new PortModel(PortType.Input, this, new PortData("inputString", "a string value displayed on our button")));

            // Nodes can have an arbitrary number of inputs and outputs.
            // If you want more ports, just create more PortData objects.
            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("button value", "returns the string value displayed on our button")));
            OutPorts.Add(new PortModel(PortType.Output, this, new PortData("window value", "returns the string value displayed in our window when button is pressed")));

            // This call is required to ensure that your ports are
            // properly created.
            RegisterAllPorts();

            // Listen for input port disconnection to trigger button UI update
            this.PortDisconnected += ButtonCustomNodeModel_PortDisconnected;

            // The arugment lacing is the way in which Dynamo handles
            // inputs of lists. If you don't want your node to
            // support argument lacing, you can set this to LacingStrategy.Disabled.
            ArgumentLacing = LacingStrategy.Disabled;

            // We create a DelegateCommand object which will be 
            // bound to our button in our custom UI. Clicking the button 
            // will call the ShowMessage method.
            ButtonCommand = new DelegateCommand(ShowMessage, CanShowMessage);

            // Setting our property here will trigger a 
            // property change notification and the UI 
            // will be updated to reflect the new value.
            ButtonText = defaultButtonText;
            WindowText = defaultWindowText;
        }

        // Starting with Dynamo v2.0 you must add Json constructors for all nodeModel
        // dervived nodes to support the move from an Xml to Json file format.  Failing to
        // do so will result in incorrect ports being generated upon serialization/deserialization.
        // This constructor is called when opening a Json graph.
        [JsonConstructor]
        ButtonCustomNodeModel(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
            this.PortDisconnected += ButtonCustomNodeModel_PortDisconnected;
            ButtonCommand = new DelegateCommand(ShowMessage, CanShowMessage);
        }

        // Restore default button/window text and trigger UI update
        private void ButtonCustomNodeModel_PortDisconnected(PortModel obj)
        {
            ButtonText = defaultButtonText;
            WindowText = defaultWindowText;
            RaisePropertyChanged("ButtonText");
        }

        #endregion

        // Use the VMDataBridge to safely retrieve our input values
        #region databridge callback
        /// <summary>
        /// Register the data bridge callback.
        /// </summary>
        protected override void OnBuilt()
        {
            base.OnBuilt();
            VMDataBridge.DataBridge.Instance.RegisterCallback(GUID.ToString(), DataBridgeCallback);
        }

        /// <summary>
        /// Unregister the data bridge callback.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            VMDataBridge.DataBridge.Instance.UnregisterCallback(GUID.ToString());
        }

        /// <summary>
        /// Callback method for DataBridge mechanism.
        /// This callback only gets called when 
        ///     - The AST is executed
        ///     - After the BuildOutputAST function is executed 
        ///     - The AST is fully built
        /// </summary>
        /// <param name="data">The data passed through the data bridge.</param>
        private void DataBridgeCallback(object data)
        {
            ArrayList inputs = data as ArrayList;
            string inputText = "";
            foreach (var input in inputs)
            {
                inputText += input.ToString() + " ";
            }

            WindowText = ("Data bridge callback of node " + GUID.ToString().Substring(0, 5) + ": " + inputText);
            ButtonText = inputText;
        }
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

            // If inputs are not connected return null
            if (!InPorts[0].Connectors.Any())
            {
                return new[] 
                {
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()),
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(1), AstFactory.BuildNullNode()),
                };
            }

            // A FunctionCallNode can be used to represent the calling of a 
            // function in the AST. The method specified here must live in 
            // a separate assembly (in our case SampleUtilities) and have 
            // been loaded by Dynamo at the time that this AST is built. 
            // If the method can't be found, you'll get a "De-referencing a 
            // non -pointer warning."

            AssociativeNode buttonFuncNode =
                AstFactory.BuildFunctionCall(
                    new Func<string, string>(SampleUtilities.DescribeButtonMessage),
                    new List<AssociativeNode> { inputAstNodes[0] });

            AssociativeNode windowFuncNode =
                AstFactory.BuildFunctionCall(
                    new Func<string, string, string>(SampleUtilities.DescribeWindowMessage),
                    new List<AssociativeNode> { AstFactory.BuildStringNode(GUID.ToString()), inputAstNodes[0] });

            // Using the AstFactory class, we can build AstNode objects
            // that assign doubles, assign function calls, build expression lists, etc.
            return new[]
            {
                // In these assignments, GetAstIdentifierForOutputIndex finds 
                // the unique identifier which represents an output on this node
                // and 'assigns' that variable the expression that you create.

                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), buttonFuncNode),
                    AstFactory.BuildAssignment(
                        AstFactory.BuildIdentifier(AstIdentifierBase + "_dummy"),
                        VMDataBridge.DataBridge.GenerateBridgeDataAst(GUID.ToString(), AstFactory.BuildExprList(inputAstNodes))),

                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(1), windowFuncNode),
                    AstFactory.BuildAssignment(
                        AstFactory.BuildIdentifier(AstIdentifierBase + "_dummy"),
                        VMDataBridge.DataBridge.GenerateBridgeDataAst(GUID.ToString(), AstFactory.BuildExprList(inputAstNodes)))
            };
        }

        #endregion

        #region command methods
        private bool CanShowMessage(object obj)
        {
            return true;
        }

        private void ShowMessage(object obj)
        {
            MessageBox.Show(WindowText);
        }

        #endregion
    }

    /// <summary>
    ///     View customizer for CustomNodeModel Node Model.
    /// </summary>
    public class ButtonCustomNodeModelNodeViewCustomization : INodeViewCustomization<ButtonCustomNodeModel>
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
        public void CustomizeView(ButtonCustomNodeModel model, NodeView nodeView)
        {
            // The view variable is a reference to the node's view.
            // In the middle of the node is a grid called the InputGrid.
            // We reccommend putting your custom UI in this grid, as it has
            // been designed for this purpose.

            // Create an instance of our custom UI class (defined in xaml),
            // and put it into the input grid.
            var buttonControl = new ButtonControl();
            nodeView.inputGrid.Children.Add(buttonControl);

            // Set the data context for our control to be the node model.
            // Properties in this class which are data bound will raise 
            // property change notifications which will update the UI.
            buttonControl.DataContext = model;
        }

        /// <summary>
        /// Here you can do any cleanup you require if you've assigned callbacks for particular 
        /// UI events on your node.
        /// </summary>
        public void Dispose() { }
    }

}
