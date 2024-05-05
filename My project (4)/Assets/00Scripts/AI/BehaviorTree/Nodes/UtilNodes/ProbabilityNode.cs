using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(ProbabilityNode), "È®·ü")]
    public class ProbabilityNode : DecoratorNode
    {
        [Range(0, 1)] public float probability;

        protected override void OnStart()
        {
            state = State.Failure;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            float passNumber = Random.Range(0f, 1f);

            if (passNumber <= probability || state == State.Running)
            {
                var state = child.Update();
                return state;
            }

            return State.Failure;
        }
    }
}