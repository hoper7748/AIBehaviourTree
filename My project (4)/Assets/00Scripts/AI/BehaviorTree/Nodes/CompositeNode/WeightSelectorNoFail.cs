using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(WeightSelectorNoFail), "실패 없는 가중치 셀렉터")]
    public class WeightSelectorNoFail : WeightSelectorNode
    {
        protected override State OnCompositeUpdate(out BTNode excutedNode)
        {
            State curState;
            do
            {
                curState = base.OnCompositeUpdate(out excutedNode);
            } while (curState == State.Failure);

            return curState;
        }
    }
}

