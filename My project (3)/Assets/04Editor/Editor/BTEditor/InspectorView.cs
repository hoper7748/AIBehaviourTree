using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using ArumSystem.BehaviorTreeEditor;


public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }

    private Editor editor;

    public InspectorView()
    {

    }

    internal void UpdateSelection(NodeView nodeView)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            if (editor.target)
            {
                editor.OnInspectorGUI();
            }
        });
        Add(container);
    }
}