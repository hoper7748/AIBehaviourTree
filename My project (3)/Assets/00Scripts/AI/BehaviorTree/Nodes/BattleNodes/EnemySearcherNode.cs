using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(EnemySearcherNode), "Àû Å½»ö")]
    public class EnemySearcherNode : ActionNode
    {
        public float traceDistance = 5f;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            float maxDistance = traceDistance;
            var enemies = Physics2D.OverlapCircleAll(agent.transform.position, maxDistance, 1 << 6);

            if (enemies.Length <= 0) { return State.Failure; }

            float distance = maxDistance * maxDistance;
            Arum targetArum = null;

            foreach (var enemy in enemies)
            {
                var arum = enemy.gameObject.GetComponent<Arum>();

                if (arum.GroupID == agent.arum.GroupID) { continue; }

                float newDistance = (arum.transform.position - agent.transform.position).sqrMagnitude;
                if (distance >= newDistance)
                {
                    distance = newDistance;
                    targetArum = arum;
                }
            }

            blackboard.target = targetArum;

            return blackboard.target == null ? State.Failure : State.Success;
        }
    }
}