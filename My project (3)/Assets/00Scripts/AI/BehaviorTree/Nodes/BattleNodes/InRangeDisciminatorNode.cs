using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(InRangeDisciminatorNode), "거리 판별자")]
    public class InRangeDisciminatorNode : ActionNode
    {
        public float discriminateDistance;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (blackboard.target == null)
            {
                return State.Failure;
            }

            Vector3 distance = blackboard.target.transform.position - agent.transform.position;

            if (distance.magnitude < discriminateDistance)
            {
                return State.Success;
            }
            else
            {
                return State.Failure;
            }
        }
    }
}