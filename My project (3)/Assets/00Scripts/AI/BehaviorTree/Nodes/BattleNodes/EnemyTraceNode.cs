using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(EnemyTraceNode), "Àû Ãß°Ý")]
    public class EnemyTraceNode : ActionNode
    {
        public float attackDistance;
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (!blackboard.CheckTarget())
            {
                return State.Failure;
            }

            float speed = agent.arum.status.GetStatus(agent.arum.moveSpeedKey);

            Vector3 dir = blackboard.target.transform.position - agent.transform.position;

            if (attackDistance >= dir.magnitude)
            {
                return State.Success;
            }

            agent.transform.position += dir.normalized * Time.deltaTime * speed;

            return State.Running;
        }
    }
}