using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    public abstract class ParentNode : BTNode
    {
        public UnityAction<BTNode> onChildAdded;
        public UnityAction<BTNode> onChildRemoved;

        // 자식에서 Add Child 구현 필요
        public virtual void AddChild(BTNode child)
        {
            onChildAdded?.Invoke(child);
        }

        // 자식에서 Remove Child 구현 필요
        public virtual void RemoveChild(BTNode child)
        {
            onChildRemoved?.Invoke(child);
        }
    }

    public abstract class SingleChildNode : ParentNode
    {
        [SerializeField] protected BTNode child;

        public override void AddChild(BTNode child)
        {
            this.child = child;

            base.AddChild(child);
        }

        public override void RemoveChild(BTNode child)
        {
            this.child = null;

            base.RemoveChild(child);
        }

        public virtual BTNode GetChild()
        {
            return child;
        }
    }

    public abstract class MultipleChildNode : ParentNode
    {
        [SerializeField] protected List<BTNode> children = new List<BTNode>();

        public override void AddChild(BTNode child)
        {
            children.Add(child);

            base.AddChild(child);
        }

        public override void RemoveChild(BTNode child)
        {
            children.Remove(child);

            base.RemoveChild(child);
        }

        public virtual BTNode GetChild(int index = 0)
        {
            return children.Count > index ? children[index] : null;
        }

        public virtual List<BTNode> GetChildren()
        {
            return children;
        }
    }
}