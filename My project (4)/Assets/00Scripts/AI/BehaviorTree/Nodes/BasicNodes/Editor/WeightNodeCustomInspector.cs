using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ArumSystem.BehaviorTreeEditor.Nodes;

[CustomEditor(typeof(WeightNode))]
public class WeightNodeCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var node = (IWeightNode)target;

        if(node == null) { return; }

        node.Weight = ((int)(EditorGUILayout.Slider("Weight", node.Weight, 0, 10) * 1000)) * 0.001f;
        node.CoolTime = ((int)(EditorGUILayout.Slider("Cool Time", node.CoolTime, 0, 10) * 1000)) * 0.001f;
    }
}
