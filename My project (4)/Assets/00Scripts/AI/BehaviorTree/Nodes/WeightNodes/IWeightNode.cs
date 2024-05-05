using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    public interface IWeightNode
    {
        public float Weight
        {
            get; set;
        }

        public float CoolTime
        {
            get; set;
        }

        public bool IsCoolDown();

        protected void BlockNode()
        {
            Weight = 0;
        }
    }
}
