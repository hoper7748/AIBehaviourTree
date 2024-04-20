using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(SelectorNode), "ºø∑∫≈Õ")]

    public class SelectorNode : CompositeNode
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
            if (childState == State.Success || childState == State.Running)
            {
                return childState;
            }

            return ++currentChild == children.Count ? State.Failure : State.Running;
        }
    }
}