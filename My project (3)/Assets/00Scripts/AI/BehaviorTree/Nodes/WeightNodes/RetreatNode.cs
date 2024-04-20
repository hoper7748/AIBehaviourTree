using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(RetreatNode), "후퇴")]
    public class RetreatNode : WeightNode
    {
        public float retreatTime = 0.5f;
        public float speedRatio = 1;
        private float currentTime;

        protected override void OnStart()
        {
            currentTime = 0;
        }

        protected override void OnStop()
        {

        }

        protected override State ExcuteUpdate()
        {
            if (blackboard.target == null)
            {
                return State.Failure;
            }

            currentTime += Time.deltaTime;

            float speed = agent.arum.status.GetStatus(agent.arum.moveSpeedKey) * speedRatio;

            Vector3 dir = blackboard.target.transform.position - agent.transform.position;

            agent.transform.position -= dir.normalized * Time.deltaTime * speed;

            if (currentTime > retreatTime)
            {
                return State.Success;
            }


            return State.Running;
        }
    }
}