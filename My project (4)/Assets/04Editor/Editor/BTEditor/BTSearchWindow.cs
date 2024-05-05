using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using System.Reflection;
using ArumSystem.BehaviorTreeEditor.Nodes;

    namespace ArumSystem.BehaviorTreeEditor
{

    public class BTSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private const string NoramlAttackNodeName = "NormalAttackNode";
        private const string ChargedAttackNodeName = "ChargedAttackNode";
        private const string DashNodeName = "DashNode";

        private BehaviorTreeView treeView;
        private BehaviorTreeEditor treeEditor;
        private Texture2D indentationIcon;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
        {
           new SearchTreeGroupEntry(new GUIContent("노드 생성"))
        };

            // 액션 노드
            {
                searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("액션 노드"), 1));
                var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
                EntriesGenerator(ref searchTreeEntries, types);
            }

            // 컴포짓 노드
            {
                searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("컴포지트 노드"), 1));
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();

                EntriesGenerator(ref searchTreeEntries, types);
            }

            // 데코레이터 노드
            {
                searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("데코레이터 노드"), 1));
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();

                EntriesGenerator(ref searchTreeEntries, types);
            }

            // 디스플레이 노드
            {
                searchTreeEntries.Add(new SearchTreeGroupEntry(new GUIContent("개발자 노드"), 1));
                var types = TypeCache.GetTypesDerivedFrom<DevelopmentNode>();

                EntriesGenerator(ref searchTreeEntries, types);
            }

            return searchTreeEntries;
        }

        private void EntriesGenerator(ref List<SearchTreeEntry> entries, TypeCache.TypeCollection types)
        {
            foreach (var type in types)
            {
                if (type.IsAbstract) { continue; }

                var attribute = type.GetCustomAttribute<BTNodeTypeAttribute>();
                if (attribute == null) { continue; }

                var newEntry = new SearchTreeEntry(new GUIContent($"{attribute.nodeName}", indentationIcon))
                {
                    level = 2,
                    userData = attribute.key
                };

                entries.Add(newEntry);
            }
        }

        private void EntryGenerator(ref List<SearchTreeEntry> entries, Type type)
        {
            if (type.IsAbstract) { return; }

            var attribute = type.GetCustomAttribute<BTNodeTypeAttribute>();
            if (attribute == null) { return; }

            var newEntry = new SearchTreeEntry(new GUIContent($"{attribute.nodeName}", indentationIcon))
            {
                level = 2,
                userData = attribute.key
            };

            entries.Add(newEntry);
        }

        public void Initialize(BehaviorTreeView treeView)
        {
            this.treeView = treeView;

            indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = treeView.GetLocalMousePosition(context.screenMousePosition, true);

            return treeView.CreateNode((Type)searchTreeEntry.userData, localMousePosition);
        }
        /*

       public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
       {
           Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);

           switch (searchTreeEntry.userData)
           {
               case CSCommandType.NormalAttack:
                   CSNormalAttackNode normalAttackNode = graphView.CreateNode(NoramlAttackNodeName, CSCommandType.NormalAttack, localMousePosition) as CSNormalAttackNode;
                   if (normalAttackNode == null) { FDebug.LogError("[Add Node Error] Failed to Add CSNormalAttackNode."); }

                   graphView.AddElement(normalAttackNode);

                   return true;
               case CSCommandType.ChargedAttack:
                   CSChargedAttackNode chargedAttackNode = graphView.CreateNode(ChargedAttackNodeName, CSCommandType.ChargedAttack, localMousePosition) as CSChargedAttackNode;
                   if (chargedAttackNode == null) { FDebug.LogError("[Add Node Error] Failed to Add CSChargedAttackNode."); }

                   graphView.AddElement(chargedAttackNode);

                   return true;
               case CSCommandType.Dash:
                   CSDashNode dashNode = graphView.CreateNode(DashNodeName, CSCommandType.Dash, localMousePosition) as CSDashNode;
                   if (dashNode == null) { FDebug.LogError("[Add Node Error] Failed to Add CSDashNode."); }

                   graphView.AddElement(dashNode);

                   return true;
               case Group:
                   graphView.CreateGroup("CommandGroup", localMousePosition);

                   return true;
               default:
                   return false;
           }
       }*/
    }
}