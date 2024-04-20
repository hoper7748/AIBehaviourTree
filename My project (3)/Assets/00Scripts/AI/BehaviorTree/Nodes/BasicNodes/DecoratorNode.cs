using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    public abstract class DecoratorNode : SingleChildNode
    {
        public override BTNode Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}