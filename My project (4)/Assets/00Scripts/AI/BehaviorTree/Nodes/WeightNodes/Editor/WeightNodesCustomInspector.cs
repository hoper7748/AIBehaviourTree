using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ArumSystem.BehaviorTreeEditor.Nodes;

[CustomEditor(typeof(TestNodeA))] public class TestNodeACustomInspector : WeightNodeCustomInspector { }

[CustomEditor(typeof(TestNodeB))] public class TestNodeBCustomInspector : WeightNodeCustomInspector { }

[CustomEditor(typeof(RetreatNode))] public class RetreatNodeCustomInspector : WeightNodeCustomInspector { }

[CustomEditor(typeof(SkillPerformerNode))] public class SkillPerformerNodeCustomInspector : WeightNodeCustomInspector { }

[CustomEditor(typeof(WeightDecoratorNode))] public class WeightDecoratorNodeCustomInspector : WeightNodeCustomInspector { }