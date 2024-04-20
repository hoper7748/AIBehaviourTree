using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BTNodeTypeAttribute : Attribute
    {
        public readonly Type key;
        public readonly string nodeName;

        public BTNodeTypeAttribute(Type key, string name = null)
        {
            this.key = key;
            nodeName = name;
            if (nodeName == null)
            {
                nodeName = Regex.Replace(key.Name.Replace("Node", ""), "(\\B[A-Z])", " $1");
            }
        }
    }

    public abstract class BTNode : ScriptableObject
    {
        public enum State
        {
            Running,
            Success,
            Failure
        }

        [HideInInspector] public State state = State.Running;
        [HideInInspector] public bool started = false;
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;
        [HideInInspector] public Blackboard blackboard;
        [HideInInspector] public AiAgent agent;
        [TextArea] public string description;
        protected Action prevUpdateEvent;
        protected Action<State> lateUpdateEvent;

        public State Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            prevUpdateEvent?.Invoke();
            state = OnUpdate();
            lateUpdateEvent?.Invoke(state);

            if (state == State.Failure || state == State.Success)
            {
                OnStop();
                started = false;
            }

            return state;
        }

        public virtual BTNode Clone()
        {
            return Instantiate(this);
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
    }
}