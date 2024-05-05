using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    public abstract class AttackNode : ActionNode
    {
        public float attackDelay;
        protected float currentTime;

        protected override void OnStart()
        {
            currentTime = 0;
        }

        protected override State OnUpdate()
        {
            //if (!blackboard.CheckTarget()) { return State.Failure; }

            //currentTime += Time.deltaTime;

            //if (currentTime >= attackDelay)
            //{
            //    return State.Success;
            //}

            return State.Running;
        }
    }
}