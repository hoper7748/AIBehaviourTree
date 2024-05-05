using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using ArumSystem.SkillSystem;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(SkillPerformerNode), "스킬 실행자")]
    public class SkillPerformerNode : WeightNode
    {
        //public SkillType type;
        public int index;

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State ExcuteUpdate()
        {
            //var skill = agent.skillManager.GetSkillByIndex(type, index);

            //if (skill == null) { return BTNode.State.Failure; }

            //var State = skill.RunSkill();

            return State.Success;
        }
    }
}