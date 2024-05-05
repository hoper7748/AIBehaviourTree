using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    public class TestNodeA : WeightNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State ExcuteUpdate()
        {
            //FDebug.Log("Test A");

            return State.Success;
        }
    }
}