using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(SequencerNode), "½ÃÄö¼­")]
    public class SequencerNode : CompositeNode
    {
        private int currentChild;

        protected override void OnStart()
        {
            currentChild = 0;
        }

        protected override void OnStop()
        {

        }

        protected override State OnCompositeUpdate(out BTNode excutedNode)
        {
            excutedNode = GetExcutingNode(currentChild);

            State childState = excutedNode.Update();
            if (childState != State.Success)
            {
                return childState;
            }

            return ++currentChild == children.Count ? State.Success : State.Running;
        }
    }
}