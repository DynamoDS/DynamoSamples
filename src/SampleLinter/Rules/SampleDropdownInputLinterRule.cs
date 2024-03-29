﻿using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using Dynamo.Graph.Workspaces;
using Dynamo.Linting.Interfaces;
using Dynamo.Linting.Rules;

namespace SampleLinter.Rules
{
    /// <summary>
    /// Similar to the slider rule, but for dropdowns. If you aren't using it as an input, it is worth considering an alternative.
    /// Having ungrouped nodes is a bad practice. This section allows the linter to flag this behavior based on linter settings in the extra folder in the extension directory.
    /// This example node uses .net .resx files and generated satellite assemblies to perform runtime lookup of localized content
    /// depending on the culture of the system Dynamo is running on.
    /// Read more: https://docs.microsoft.com/en-us/dotnet/framework/resources/creating-resource-files-for-desktop-apps
    /// You can use the -l "es-ES" flag when starting DynamoSandbox.exe to replace the English strings with Spanish ones.
    /// For more info on the CLI interface read more: https://github.com/DynamoDS/Dynamo/wiki/Dynamo-Command-Line-Interface
    /// </summary>
    internal class SampleDropdownInputLinterRule : NodeLinterRule
    {
        public override string Id => "E4EC24B8-8F87-42B3-874F-A3DB3A69098A";
        public override SeverityCodesEnum SeverityCode => SeverityCodesEnum.Warning;

        public override string Description => Properties.Resources.DropdownDescription;

        public override string CallToAction => Properties.Resources.DropdownDescription;

        public override List<string> EvaluationTriggerEvents =>
            new List<string>()
            {
                nameof(NodeModel.IsSetAsInput),
                nameof(NodeModel.State),
            };

        
        protected override RuleEvaluationStatusEnum EvaluateFunction(NodeModel nodeModel, string changedEvent)
        {
            if (nodeModel != null)
            {
                if (nodeModel.NodeType.Equals("ExtensionNode"))
                {
                    if (!nodeModel.IsSetAsInput)
                    {
                        return RuleEvaluationStatusEnum.Failed;
                    }
                }
            }

            return RuleEvaluationStatusEnum.Passed;
        }


        protected override List<Tuple<RuleEvaluationStatusEnum, string>> InitFunction(WorkspaceModel workspaceModel)
        {
            //create a list to hold results information.
            //The Tuple should be of a RuleEvaluationStatus and the name of the node
            List<Tuple<RuleEvaluationStatusEnum, string>> results = new List<Tuple<RuleEvaluationStatusEnum, string>>();

            //Iterate over all the nodes in the workspace
            foreach (NodeModel node in workspaceModel.Nodes)
            {
                //Call the Evaluate function 
                var evaluationStatus = EvaluateFunction(node, "initialize");

                //Check what happened for this node.  If it passed the we continue
                if (evaluationStatus == RuleEvaluationStatusEnum.Passed)
                    continue;

                //Create the tuple to return and add it to the result list
                var valueTuple = Tuple.Create(evaluationStatus, node.GUID.ToString());
                results.Add(valueTuple);
            }

            return results;
        }
    }


}
