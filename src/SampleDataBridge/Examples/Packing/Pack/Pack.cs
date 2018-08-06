using Dynamo.Graph.Nodes;
using Newtonsoft.Json;
using ProtoCore.AST.AssociativeAST;
using SampleDataBridge.Examples.Packing.Pack.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleDataBridge.Examples.Packing.Pack
{
    /// <summary>
    /// Creates a node that takes in a TypeDefinition and input values defined by the given TypeDefinition and outputs it as dictionary.
    /// </summary>
    [NodeName("Pack")]
    [NodeCategory("SampleDataBridge.Examples.Packing")]
    [NodeDescription("Creates a dictionary given a TypeDefinition and its corresponding input values")]
    [OutPortNames("Out")]
    [OutPortTypes("Dictionary")]
    [OutPortDescriptions("Dictionary")]
    [IsDesignScriptCompatible]
    public class Pack : PackingNode
    {
        private List<object> cachedValues;
        private ValidationManager validationManager;

        public Pack()
        {
            validationManager = new ValidationManager(this);
        }

        /// <summary>
        /// Private constructor used for serialization.
        /// </summary>
        /// <param name="inPorts">A collection of <see cref="PortModel"/> objects.</param>
        /// <param name="outPorts">A collection of <see cref="PortModel"/> objects.</param>
        [JsonConstructor]
        protected Pack(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts)
            : base(inPorts, outPorts)
        {
            validationManager = new ValidationManager(this);
        }

        protected override void RefreshTypeDefinitionPorts()
        {
            cachedValues = null;
            InPorts.Skip(1).ToList().ForEach(port => InPorts.Remove(port));

            if (TypeDefinition != null)
            {
                foreach (var property in TypeDefinition.Properties)
                {
                    InPorts.Add(new PortModel(PortType.Input, this, new PortData(property.Key, property.Value.ToString())));
                }
            }
        }

        protected override void ValidateInputs(List<object> values)
        {
            var wasInWarningState = validationManager.Warnings.Any();
            if (values.Count > 1)
            {
                var valuesByIndex = new Dictionary<int, object>();
                for (int i = 1; i < values.Count; ++i)
                {
                    if (cachedValues == null || cachedValues.Count <= i || values[i] != cachedValues[i])
                    {
                        valuesByIndex[i] = values[i];
                    }
                }

                validationManager.HandleValidation(valuesByIndex);
            }

            cachedValues = values;

            if (wasInWarningState != validationManager.Warnings.Any())
            {
                OnNodeModified(true);
            }
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            var baseOutput = base.BuildOutputAst(inputAstNodes).ToList();

            if (inputAstNodes == null || !IsValidInputState(inputAstNodes) || TypeDefinition == null)
            {
                baseOutput.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()));
                return baseOutput;
            }

            inputAstNodes = inputAstNodes.Skip(1).ToList();
            inputAstNodes.Add(AstFactory.BuildStringNode(TypeDefinition.Name));
            var keys = TypeDefinition.Properties.Select(input => AstFactory.BuildStringNode(input.Key) as AssociativeNode).ToList();
            keys.Add(AstFactory.BuildStringNode("typeid"));

            var isCollectionInputs = TypeDefinition.Properties.Select(input => AstFactory.BuildBooleanNode(input.Value.IsCollection) as AssociativeNode).ToList();

            var functionCall = AstFactory.BuildFunctionCall(
                new Func<List<string>, List<bool>, object, object>(SampleDataBridge.Functions.Packing.PackFunctions.PackOutputAsDictionary),
                new List<AssociativeNode> { AstFactory.BuildExprList(keys), AstFactory.BuildExprList(isCollectionInputs), AstFactory.BuildExprList(inputAstNodes) });

            baseOutput.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall));

            return baseOutput;
        }
    }
}
