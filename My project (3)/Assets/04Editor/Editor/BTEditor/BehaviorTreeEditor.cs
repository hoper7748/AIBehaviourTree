using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.Callbacks;

namespace ArumSystem.BehaviorTreeEditor
{
    public class BehaviorTreeEditor : EditorWindow
    {
        private BehaviorTreeView treeView;
        private InspectorView inspectorView;
        private IMGUIContainer blackboardView;

        SerializedObject treeObject;
        SerializedProperty blackboardProperty;

        [MenuItem("Custom Windows/BehaviorTreeEditor/Editor ...")]
        public static void OpenWindow()
        {
            BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
            wnd.titleContent = new GUIContent("BehaviorTreeEditor");
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is BehaviorTree)
            {
                OpenWindow();
                return true;
            }

            return false;
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/04Editor/Editor/BTEditor/BehaviorTreeEditor.uxml");
            visualTree.CloneTree(root);

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/04Editor/Editor/BTEditor/BehaviorTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<BehaviorTreeView>();
            treeView.SetEditor(this);
            inspectorView = root.Q<InspectorView>();
            blackboardView = root.Q<IMGUIContainer>();
            blackboardView.onGUIHandler = () =>
            {
                if (blackboardProperty == null) { return; }
                if (treeObject.targetObject == null) { return; }

                treeObject?.Update();
                EditorGUILayout.PropertyField(blackboardProperty);
                treeObject?.ApplyModifiedProperties();
            };

            treeView.OnNodeSelected = OnNodeSelectionChanged;

            OnSelectionChange();
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            switch (change)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }

        private void OnSelectionChange()
        {
            BehaviorTree tree = Selection.activeObject as BehaviorTree;
            if (!tree)
            {
                if (Selection.activeGameObject)
                {
                    var runner = Selection.activeGameObject.GetComponent<BehaviorTreeRunner>();
                    if (runner)
                    {
                        tree = runner.tree;
                    }
                }
            }

            if (!tree) { return; }

            if (Application.isPlaying ? true : AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                treeView?.PopulateView(tree);
            }

            treeObject = new SerializedObject(tree);
            blackboardProperty = treeObject.FindProperty("blackboard");
        }

        private void OnGUI()
        {
            if (Event.current?.type == EventType.KeyDown && Event.current?.keyCode == KeyCode.Delete)
            {
                DeleteSelectedNode();
                Event.current.Use(); // 이벤트 처리 완료
            }
        }

        private void DeleteSelectedNode()
        {
            // 그래프 뷰에서 선택된 요소들을 가져옴
            var selection = treeView.selection;
            var selections = new List<GraphElement>();

            // 선택된 각 요소에 대해 작업 수행
            foreach (var selectedElement in selection)
            {
                // 선택된 요소가 NodeView인 경우에만 삭제
                if (selectedElement is GraphElement elem)
                {
                    selections.Add(elem);
                }
            }

            while (selections.Count > 0)
            {
                var nodeView = selections[0] as NodeView;

                if (nodeView != null)
                {
                    Undo.RecordObject(nodeView.node, "Behavior Tree (DeleteNode)");
                    nodeView.RemoveInputPort(treeView);
                    nodeView.RemoveOutputPort(treeView);
                }

                // 선택된 노드를 삭제
                treeView.DeleteGraphElement(selections[0]);

                selections.RemoveAt(0);
            }

            treeView.PopulateView();
        }

        private void OnNodeSelectionChanged(NodeView node)
        {
            inspectorView.UpdateSelection(node);
        }

        private void OnInspectorUpdate()
        {
            treeView?.UpdateNodeState();
        }
    }
}