using System;
using System.Collections.Generic;
using CoreNodeModels.Input;
using Dynamo.Graph.Nodes;
using Dynamo.Graph.Workspaces;
using Dynamo.Linting.Interfaces;
using Dynamo.Linting.Rules;

namespace SampleLinter.Rules
{
    internal class SampleSliderInputLinterRule : NodeLinterRule
    {
        public override string Id => "54EFD003-FF02-41B6-9F2E-52216BA23844";
        public override SeverityCodesEnum SeverityCode => SeverityCodesEnum.Warning;

        public override string Description => "You have number sliders placed that are not inputs.";

        public override string CallToAction => "If the placed number sliders are needing to be inputs, remember to rename them and mark them as input. If you do not need these as inputs, consider an alternative node.";

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
                if (nodeModel is DoubleSlider || (nodeModel is IntegerSlider64Bit))
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
