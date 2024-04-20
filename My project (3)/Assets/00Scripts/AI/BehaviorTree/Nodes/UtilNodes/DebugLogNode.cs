using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(DebugLogNode), "디버그 로그")]
    public class DebugLogNode : ActionNode
    {
        public string message;

        protected override void OnStart()
        {
            FDebug.Log($"OnStart_{message}");
        }

        protected override void OnStop()
        {
            FDebug.Log($"OnStop_{message}");
        }

        protected override State OnUpdate()
        {
            FDebug.Log($"OnUpdate_{message}");

            return State.Success;
        }
    }
}