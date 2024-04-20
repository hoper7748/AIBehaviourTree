using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using ArumSystem.BehaviorTreeEditor.Nodes;

namespace ArumSystem.BehaviorTreeEditor
{
    public class NodeView : Node
    {
        public Action<NodeView> OnNodeSelected;
        public BTNode node;
        public Port input;
        public Port output;

        public NodeView(BTNode node, Vector2? position) : base("Assets/04Editor/Editor/BTEditor/NodeView.uxml")
        {
            this.node = node;
            title = node.name;
            viewDataKey = node.guid;
            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPort();
            CreateOutputPort();
            SetupClasses();

            Label description = this.Q<Label>("description");
            description.bindingPath = "description";
            description.Bind(new SerializedObject(node));

            if (position.HasValue)
            {
                SetPosition(new Rect(position.Value, Vector2.zero));
            }
        }

        private void SetupClasses()
        {
            if (node is ActionNode)
            {
                AddToClassList("action");
            }
            else if (node is CompositeNode)
            {
                AddToClassList("composite");
            }
            else if (node is DecoratorNode)
            {
                AddToClassList("decorator");
            }
            else if (node is RootNode)
            {
                AddToClassList("root");
            }
            else if (node is DevelopmentNode)
            {
                AddToClassList("develop");
            }
        }

        private void CreateOutputPort()
        {
            if (node is ActionNode)
            {
            }
            else if (node is CompositeNode)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else if (node is DecoratorNode)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (node is RootNode)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (node is DevelopmentNode)
            {

            }

            if (output != null)
            {
                output.portName = "";
                //output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(output);
            }
        }

        private void CreateInputPort()
        {
            if (node is ActionNode)
            {
                input = InstantiateCustomPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is CompositeNode)
            {
                input = InstantiateCustomPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is DecoratorNode)
            {
                input = InstantiateCustomPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is RootNode)
            {

            }
            else if (node is DevelopmentNode)
            {
                input = InstantiateCustomPort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }


            if (input != null)
            {
                input.portName = "";
                //input.style.flexDirection = FlexDirection.Column;
                inputContainer.Add(input);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Undo.RecordObject(node, "Behavior Tree (Set Position)");

            node.position.x = newPos.x;
            node.position.y = newPos.y;

            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();

            if (OnNodeSelected != null)
            {
                OnNodeSelected.Invoke(this);
            }
        }

        public void RemoveInputPort(BehaviorTreeView btView)
        {
            if (input == null) { return; }

            List<Edge> removingEdges = new List<Edge>();

            foreach (var child in input?.connections)
            {
                removingEdges.Add(child);
            }

            while (removingEdges.Count > 0)
            {
                btView.DeleteGraphElement(removingEdges[0]);
                removingEdges.RemoveAt(0);
            }
        }

        public void RemoveOutputPort(BehaviorTreeView btView)
        {
            if (output == null) { return; }

            List<Edge> removingEdges = new List<Edge>();

            foreach (var child in output?.connections)
            {
                removingEdges.Add(child);
            }

            while (removingEdges.Count > 0)
            {
                btView.DeleteGraphElement(removingEdges[0]);
                removingEdges.RemoveAt(0);
            }
        }

        public void SortChildren()
        {
            CompositeNode composite = node as CompositeNode;

            if (composite)
            {
                composite.GetChildren().Sort(SortByHorizontalPosition);
            }
        }

        private int SortByHorizontalPosition(BTNode left, BTNode right)
        {
            return left.position.x < right.position.x ? -1 : 1;
        }

        public void UpdateState()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("success");
            RemoveFromClassList("failure");

            if (Application.isPlaying)
            {
                switch (node.state)
                {
                    case BTNode.State.Running:
                        if (node.started)
                        {
                            AddToClassList("running");
                        }
                        break;
                    case BTNode.State.Success:
                        AddToClassList("success");
                        break;
                    case BTNode.State.Failure:
                        AddToClassList("failure");
                        break;
                }
            }
        }

        public virtual Port InstantiateCustomPort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
        {
            return CustomPort.Create<Edge>(orientation, direction, capacity, type);
        }
    }
}