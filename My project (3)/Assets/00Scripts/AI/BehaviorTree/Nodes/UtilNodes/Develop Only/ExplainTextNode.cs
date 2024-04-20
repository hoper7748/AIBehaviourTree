using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(ExplainTextNode), "설명 텍스트")]
    public class ExplainTextNode : DevelopmentNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            return State.Success;
        }
    }
}