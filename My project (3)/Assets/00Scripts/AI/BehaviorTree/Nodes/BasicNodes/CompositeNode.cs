using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    public abstract class CompositeNode : MultipleChildNode
    {
        protected BTNode runningNode;

        protected sealed override State OnUpdate()
        {
            BTNode excutedNode;
            State state = OnCompositeUpdate(out excutedNode);

            if (excutedNode != null && excutedNode.state == State.Running)
            {
                runningNode = excutedNode;
            }

            return state;
        }

        protected abstract State OnCompositeUpdate(out BTNode excutedNode);

        protected virtual BTNode GetExcutingNode(int currentChild)
        {
            if (runningNode != null && runningNode.state == State.Running)
            {
                return runningNode;
            }
            else
            {
                return children[currentChild];
            }
        }

        public override BTNode Clone()
        {
            CompositeNode node = Instantiate(this);
            node.children = children.ConvertAll(c => c.Clone());
            return node;
        }
    }
}