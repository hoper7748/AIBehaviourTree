using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(MeleeAttackNode), "근접 공격")]
    public class MeleeAttackNode : AttackNode
    {
        protected override void OnStart()
        {
            base.OnStart();

            //if (!blackboard.CheckTarget()) { return; }
            //DamageInfo damageInfo = new DamageInfo(agent.arum, blackboard.target, 1, agent.arum.status.GetStatus(agent.arum.knockbackKey));

            //agent.arum.InstantAttack(damageInfo);
        }

        protected override void OnStop()
        {

        }
    }
}