using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Linq;
using UnityEngine;
using ArumSystem.BehaviorTreeEditor.Nodes;
using ArumSystem.BehaviorTreeEditor;

public class BehaviorTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviorTreeView, UxmlTraits> { }
    public Action<NodeView> OnNodeSelected;
    private BehaviorTree tree;
    private BehaviorTreeEditor treeEditor;
    private BTSearchWindow searchWindow;

    public BehaviorTreeView()
    {
        GridBackground grid = new GridBackground();
        Insert(0, grid);

        this.AddManipulator(GetNewZoom());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        SetSearchWindow();

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/04Editor/Editor/BTEditor/BehaviorTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void SetSearchWindow()
    {
        if (searchWindow == null)
        {
            searchWindow = ScriptableObject.CreateInstance<BTSearchWindow>();

            searchWindow.Initialize(this);
        }
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
    }

    public void SetEditor(BehaviorTreeEditor editor)
    {
        treeEditor = editor;
    }

    private ContentZoomer GetNewZoom()
    {
        var zoomer = new ContentZoomer();
        zoomer.maxScale = 3;
        zoomer.minScale = 0.15f;
        zoomer.scaleStep = 0.1f;

        return zoomer;
    }

    private void OnUndoRedo()
    {
        if(tree == null) { return; }

        PopulateView(tree);
        AssetDatabase.SaveAssets();
    }

    private NodeView FindNodeView(BTNode node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    internal void PopulateView()
    {
        PopulateView(tree);
    }

    internal void PopulateView(BehaviorTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;

        DeleteElements(graphElements);

        graphViewChanged += OnGraphViewChanged;

        if (tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        // creates node view
        tree.nodes.ForEach(n => CreateNodeView(n, null));

        // create edges
        tree.nodes.ForEach(n =>
        {
            var children = tree.GetChildren(n);
            children?.ForEach(c =>
            {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);

                if (parentView == null || childView == null) { return; }

                Edge edge = parentView.output.ConnectTo(childView.input);
                AddElement(edge);
            });
        });

    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node).ToList();
    }

    public GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                DeleteGraphElement(elem);
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.AddChild(parentView.node, childView.node);
            });
        }

        if (graphViewChange.movedElements != null)
        {
            nodes.ForEach((n) =>
            {
                NodeView view = n as NodeView;
                view.SortChildren();
            });
        }

        return graphViewChange;
    }

    public void DeleteGraphElement(GraphElement elem)
    {
        NodeView nodeView = elem as NodeView;
        if (nodeView != null)
        {
            tree.DeleteNode(nodeView.node);
        }

        Edge edge = elem as Edge;
        if (edge != null)
        {
            NodeView parentView = edge.output.node as NodeView;
            NodeView childView = edge.input.node as NodeView;
            tree.RemoveChild(parentView.node, childView.node);
        }
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        var mousePosition = GetLocalMousePosition(evt.mousePosition);

        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                if (type.IsAbstract) { continue; }

                evt.menu.AppendAction($"[{type.BaseType.Name.Replace("Node", "")}] {type.Name.Replace("Node", "")}", (a) => CreateNode(type, mousePosition));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                if (type.IsAbstract) { continue; }
                evt.menu.AppendAction($"[{type.BaseType.Name.Replace("Node", "")}] {type.Name.Replace("Node", "")}", (a) => CreateNode(type, mousePosition));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                if (type.IsAbstract) { continue; }
                evt.menu.AppendAction($"[{type.BaseType.Name.Replace("Node", "")}] {type.Name.Replace("Node", "")}", (a) => CreateNode(type, mousePosition));
            }
        }

        {
            var types = TypeCache.GetTypesDerivedFrom<DevelopmentNode>();

            foreach (var type in types)
            {
                if (type.IsAbstract) { continue; }
                evt.menu.AppendAction($"[{type.BaseType.Name.Replace("Node", "")}] {type.Name.Replace("Node", "")}", (a) => CreateNode(type, mousePosition));
            }
        }
    }

    public bool CreateNode(Type type, Vector2? position)
    {
        BTNode node = tree.CreateNode(type);

        if (node == null) { return false; }

        CreateNodeView(node, position);

        return true;
    }

    private void CreateNodeView(BTNode node, Vector2? position)
    {
        NodeView nodeView = new NodeView(node, position);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    public void UpdateNodeState()
    {
        nodes.ForEach(n =>
        {
            NodeView view = n as NodeView;

            view.UpdateState();
        });
    }

    public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
    {
        if (treeEditor == null) { return Vector2.zero; }

        Vector2 worldMousePosition = mousePosition;

        if (isSearchWindow)
        {
            worldMousePosition -= treeEditor.position.position;
        }

        Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

        return localMousePosition;
    }
}
