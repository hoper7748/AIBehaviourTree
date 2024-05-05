using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    public abstract class WeightNode : ActionNode, IWeightNode
    {
        public UnityAction<float, float> onWeightChanged;

        private float weight;
        public float Weight
        {
            get { return weight; }
            set
            {
                if (weight != value)
                {
                    float original = weight;
                    weight = value;
                    onWeightChanged?.Invoke(original, weight);
                }
            }
        }

        private float coolTime;
        public float CoolTime
        {
            get { return coolTime; }
            set
            {
                if (coolTime != value)
                {
                    coolTime = value;
                }
            }
        }

        private float startCoolTime;

        public bool IsCoolDown() =>  Time.time - startCoolTime >= CoolTime;

        protected sealed override State OnUpdate()
        {
            if(IsCoolDown())
            {
                // Time.time은 오버플로 걱정은 없다.
                // 다만, 정확성 문제가 있어서 ms기준으로 4.5시간 만에 정확성을 잃고, s기준으로 194일만에 정확성을 잃는다.
                // 추후 정확성이 필요해지면 로직을 수정해야 한다.
                var newState = ExcuteUpdate();
                if (newState == State.Success)
                {
                    startCoolTime = Time.time;
                }
                return newState;
            }

            return State.Failure;
        }

        protected abstract State ExcuteUpdate();

        public override BTNode Clone()
        {
            WeightNode node = Instantiate(this);
            node.Weight = Weight;
            node.CoolTime = CoolTime;
            startCoolTime = Time.time;
            return node;
        }
    }
}