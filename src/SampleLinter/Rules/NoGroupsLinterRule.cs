using System;
using System.Collections.Generic;
using System.Linq;
using Dynamo.Graph.Nodes;
using Dynamo.Graph.Workspaces;
using Dynamo.Linting.Interfaces;
using Dynamo.Linting.Rules;
using Dynamo.ViewModels;

namespace SampleLinter.Rules
{
    public class NoGroupsLinterRule : GraphLinterRule
    {
        public override string Id => "EF17A093-4E94-4AE9-8876-18631396A23A"; //this id is unique to each linter rule
        public override SeverityCodesEnum SeverityCode => SeverityCodesEnum.Warning;
        public override string Description => "There are nodes that are not in groups.";
        public override string CallToAction => $"You have {_nodesNotInGroups.Count} nodes that are not in groups. Grouping your nodes is a best-practice when authoring Dynamo graphs.";

        //To store our ungrouped nodes for counts and checks
        private readonly List<NodeModel> _nodesNotInGroups = new List<NodeModel>();

        //Allow the end-user to have a few nodes outside of groups. We don't want to be too overbearing.
        private readonly int _ungroupedAllowed = 5;

        public NoGroupsLinterRule(LinterSettings linterSettings)
        {
            _ungroupedAllowed = linterSettings.AllowedUngroupedNodes;
        }

        public override List<string> EvaluationTriggerEvents =>
            new List<string>()
            {
                "Modified"
            };

        protected override Tuple<RuleEvaluationStatusEnum, HashSet<string>> EvaluateFunction(WorkspaceModel workspaceModel, string changedEvent, NodeModel nodeModel = null)
        {
            //clear our list as we are reevaluating the graph
            _nodesNotInGroups.Clear();
            
            if (workspaceModel != null)
            {
                //check each node for grouping
                foreach (var node in workspaceModel.Nodes)
                {
                    CheckNodeForGroup(node, workspaceModel);
                }
            }

            var result = _nodesNotInGroups.Count > _ungroupedAllowed ? RuleEvaluationStatusEnum.Failed : RuleEvaluationStatusEnum.Passed;

            return new Tuple<RuleEvaluationStatusEnum, HashSet<string>>(result, new HashSet<string>());
        }



        protected override List<Tuple<RuleEvaluationStatusEnum, HashSet<string>>> InitFunction(WorkspaceModel workspaceModel)
        {
            _nodesNotInGroups.Clear();

            //create a list to hold results information.
            //The Tuple should be of a RuleEvaluationStatus and the name of the node
            List<Tuple<RuleEvaluationStatusEnum, string>> results = new List<Tuple<RuleEvaluationStatusEnum, string>>();

            //Iterate over all the nodes in the workspace
            foreach (NodeModel node in workspaceModel.Nodes)
            {
                CheckNodeForGroup(node, workspaceModel);
            }

            return new List<Tuple<RuleEvaluationStatusEnum, HashSet<string>>> { EvaluateFunction(workspaceModel, "initialize") };
        }

        private void CheckNodeForGroup(NodeModel nodeModel, WorkspaceModel workspaceModel)
        {
            //collect current groups from file
            var groups = workspaceModel.Annotations.ToList();

            //if there are no groups, return all of the current nodes
            if (!groups.Any())
            {
                _nodesNotInGroups.AddRange(workspaceModel.Nodes);

                return;
            };

            //check if the node is in any of the groups. If not add it to our list.
            if (!groups.Any(g => g.Nodes.Any(n => n.GUID.Equals(nodeModel.GUID))))
            {
                _nodesNotInGroups.Add(nodeModel);
            }
            //the node is in a group, remove it from the list if applicable
            else
            {
                _nodesNotInGroups.Remove(nodeModel);
            }
        }
    }


}
