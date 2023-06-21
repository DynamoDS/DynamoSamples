using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.Graph.Workspaces;
using Dynamo.Linting;
using Dynamo.Linting.Rules;
using Dynamo.Wpf.Extensions;

namespace SampleLinter
{
    public class SampleLinter : LinterExtensionBase
    {
        public override string UniqueId => "44BAAD49-4750-47BE-AB60-61DD30962FAE";
        public override string Name => "Sample Linter";

        private HomeWorkspaceModel currentWorkspace;

        private SampleSliderPlacedLinterRule sliderPlacedRule;

        private const string NODE_ADDED_PROPERTY = "NodeAdded";

        public override void Ready(ReadyParams rp)
        {
            sliderPlacedRule = new SampleSliderPlacedLinterRule();

            AddLinterRule(sliderPlacedRule);

            var stuff = base.IsActive;
        }


        public override void Shutdown()
        {
            throw new NotImplementedException();
        }


        //private void SubscribeGraphEvents()
        //{
        //    currentWorkspace.NodeAdded += OnNodeAdded;
        //}
        //private void OnNodeAdded(NodeModel node)
        //{
        //    EvaluateNodeRules(node, NODE_ADDED_PROPERTY);
        //}
        //private void EvaluateNodeRules(NodeModel modifiedNode, string changedProperty)
        //{
        //    //TODO:on demand or subscribed
        //    sliderPlacedRule.

        //    if (!IsActive)
        //        return;

        //    var nodeRules = LinterRules.
        //        Where(x => x is NodeLinterRule).
        //        Cast<NodeLinterRule>().
        //        ToList();

        //    if (nodeRules is null)
        //        return;

        //    foreach (var rule in nodeRules)
        //    {
        //        if (changedProperty != NODE_ADDED_PROPERTY && !rule.EvaluationTriggerEvents.Contains(changedProperty))
        //            continue;

        //        rule.
        //    }
        //}
    }
}
