using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArumSystem.BehaviorTreeEditor;

public class BehaviorTreeRunner : MonoBehaviour
{
    public BehaviorTree tree;
    //[SerializeField] private AiAgent agent;

    private void Start()
    {
        //if(agent == null) { agent = GetComponent<AiAgent>(); }

        tree = tree.Clone();
        //tree.Bind(agent);
    }

    protected virtual void Update()
    {
        tree.Update();
    }
}
