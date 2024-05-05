using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArumSystem.BehaviorTreeEditor.Nodes
{
    [BTNodeType(typeof(WeightSelectorNode), "가중치 셀렉터")]
    public class WeightSelectorNode : CompositeNode
    {
        [SerializeField] private float totalWeights;

        public WeightSelectorNode()
        {
            onChildAdded += OnChildAdded;
            onChildRemoved += OnChildRemoved;
        }

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnCompositeUpdate(out BTNode excutedNode)
        {
            if (children.Count == 0) { excutedNode = null; return State.Failure; }

            excutedNode = GetExcutingNode();

            State childState = excutedNode.Update();

            return childState;
        }

        protected virtual BTNode GetExcutingNode()
        {
            if (runningNode != null && runningNode.state == State.Running)
            {
                return runningNode;
            }
            else
            {
                float rand = Random.Range(0, totalWeights);
                float sum = 0;
                foreach (BTNode child in children)
                {
                    if (child is IWeightNode weightChild)
                    {
                        float weight = weightChild.Weight;
                        sum += weight;

                        if (rand < sum)
                        {
                            return child;
                        }
                    }
                }

                //FDebug.LogError("Total Weight is Invalid", GetType());

                return null;
            }
        }

        private void OnChildAdded(BTNode node)
        {
            if(node is WeightNode weightNode)
            {
                totalWeights += weightNode.Weight;
                weightNode.onWeightChanged += OnWeightChanged;

                return;
            }
            
            if(node is WeightDecoratorNode weightDecorator)
            {
                totalWeights += weightDecorator.Weight;
                weightDecorator.onWeightChanged += OnWeightChanged;

                return;
            }

            //FDebug.LogWarning("Used Invalid Node. This Node's children should be Weight Node. Please Check Children of this Node", GetType());
        }
        private void OnChildRemoved(BTNode node)
        {
            if(node is WeightNode weightNode)
            {
                totalWeights -= weightNode.Weight;
                weightNode.onWeightChanged -= OnWeightChanged;

                if (children.Count == 0) { totalWeights = 0; }
                return;
            }

            if (node is WeightDecoratorNode weightDecorator)
            {
                totalWeights -= weightDecorator.Weight;
                weightDecorator.onWeightChanged -= OnWeightChanged;

                if (children.Count == 0) { totalWeights = 0; }
                return;
            }

            //FDebug.LogWarning("Used Invalid Node. This Node's children should be Weight Node. Please Check Children of this Node", GetType());
        }

        private void OnWeightChanged(float oldWeight, float newWeight)
        {
            totalWeights -= oldWeight;
            totalWeights += newWeight;
        }

        public override BTNode Clone()
        {
            var node = base.Clone() as WeightSelectorNode;

            node.totalWeights = 0;
            foreach (var child in node.children)
            {
                node.OnChildAdded(child);
            }

            return node;
        }
    }
}