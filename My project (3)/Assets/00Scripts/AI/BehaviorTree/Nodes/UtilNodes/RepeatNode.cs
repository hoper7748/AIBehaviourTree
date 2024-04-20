using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(RepeatNode), "นÝบน")]
    public class RepeatNode : DecoratorNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            child.Update();

            return State.Running;
        }
    }
}