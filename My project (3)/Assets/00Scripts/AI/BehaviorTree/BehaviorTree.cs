using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using ArumSystem.BehaviorTreeEditor.Nodes;

namespace ArumSystem.BehaviorTreeEditor
{
    [CreateAssetMenu()]
    public class BehaviorTree : ScriptableObject
    {
        public BTNode rootNode;
        public BTNode.State treeState = BTNode.State.Running;
        public List<BTNode> nodes = new List<BTNode>();
        public Blackboard blackboard = new Blackboard();

        public BTNode.State Update()
        {
            if (rootNode.state == BTNode.State.Running)
            {
                treeState = rootNode.Update();
            }

            return treeState;
        }

#if UNITY_EDITOR
        public BTNode CreateNode(Type type)
        {
            var node = CreateInstance(type) as BTNode;

            if (node == null) { return null; }

            node.name = Regex.Replace(type.Name.Replace("Node", ""), "(\\B[A-Z])", " $1");

            node.guid = GUID.Generate().ToString();

            Undo.RecordObject(this, "Behavior Tree (CreateNode)");

            nodes.Add(node);

            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }

            Undo.RegisterCreatedObjectUndo(node, "Behavior Tree (CreateNode)");

            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(BTNode node)
        {
            Undo.RecordObject(this, "Behavior Tree (DeleteNode)");

            nodes.Remove(node);
            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);

            AssetDatabase.SaveAssets();
        }

        public void AddChild(BTNode parent, BTNode child)
        {
            ParentNode parentNode = parent as ParentNode;
            if (parentNode)
            {
                Undo.RecordObject(parentNode, "Behavior Tree (AddChild)");
                parentNode.AddChild(child);
                EditorUtility.SetDirty(parentNode);

                return;
            }

            /*DecoratorNode decorator = parent as DecoratorNode;
            if(decorator)
            {
                Undo.RecordObject(decorator, "Behavior Tree (AddChild)");
                decorator.child = child;
                EditorUtility.SetDirty(decorator);

                return;
            }

            RootNode rootNode = parent as RootNode;
            if (rootNode)
            {
                Undo.RecordObject(rootNode, "Behavior Tree (AddChild)");
                rootNode.child = child;
                EditorUtility.SetDirty(rootNode);

                return;
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behavior Tree (AddChild)");
                composite.children?.Add(child);
                EditorUtility.SetDirty(composite);

                return;*/
        }

        public void RemoveChild(BTNode parent, BTNode child)
        {
            ParentNode parentNode = parent as ParentNode;
            if (parentNode)
            {
                Undo.RecordObject(parentNode, "Behavior Tree (RemoveChild)");
                parentNode.RemoveChild(child);
                EditorUtility.SetDirty(parentNode);

                return;
            }

            /* DecoratorNode decorator = parent as DecoratorNode;
             if (decorator)
             {
                 Undo.RecordObject(decorator, "Behavior Tree (RemoveChild)");
                 decorator = null;
                 EditorUtility.SetDirty(decorator);

                 return;
             }

             RootNode rootNode = parent as RootNode;
             if (rootNode)
             {
                 Undo.RecordObject(rootNode, "Behavior Tree (RemoveChild)");
                 rootNode.child = null;
                 EditorUtility.SetDirty(rootNode);

                 return;
             }

             CompositeNode composite = parent as CompositeNode;
             if (composite)
             {
                 Undo.RecordObject(composite, "Behavior Tree (RemoveChild)");
                 composite.children?.Remove(child);
                 EditorUtility.SetDirty(composite);

                 return;
             }*/
        }

        public List<BTNode> GetChildren(BTNode parent)
        {
            var singleChildNode = parent as SingleChildNode;
            if (singleChildNode)
            {
                var child = singleChildNode.GetChild();

                return child == null ? null : new List<BTNode> { child };
            }

            var multipleChildNode = parent as MultipleChildNode;
            if (multipleChildNode)
            {
                var children = multipleChildNode.GetChildren();

                return children;
            }

            /*DecoratorNode decorator = parent as DecoratorNode;
            if (decorator && decorator.child != null)
            {
                children.Add(decorator.child);
            }

            RootNode rootNode = parent as RootNode;
            if (rootNode && rootNode.child != null)
            {
                children.Add(rootNode.child);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite && composite.children != null)
            {
                return composite.children;
            }*/

            return null;
        }
#endif

        public void Traverse(BTNode node, Action<BTNode> visitor)
        {
            if (node)
            {
                visitor.Invoke(node);
                var children = GetChildren(node);
                children?.ForEach((n) => Traverse(n, visitor));
            }
        }

        public BehaviorTree Clone()
        {
            BehaviorTree tree = Instantiate(this);
            tree.rootNode = tree.rootNode.Clone();
            tree.nodes = new List<BTNode>();
            Traverse(tree.rootNode, (n) =>
            {
                tree.nodes.Add(n);
            });
            return tree;
        }

        public void Bind(AiAgent agent)
        {
            Traverse(rootNode, node =>
            {
                node.agent = agent;
                node.blackboard = blackboard;
            });
        }
    }

}
