using DevComponents.AdvTree;

public static class Extensions
{
    public static void MoveUp(this Node node)
    {
        Node parent = node.Parent;
        AdvTree view = node.TreeControl;
        if (parent != null)
        {
            int index = parent.Nodes.IndexOf(node);
            if (index > 0)
            {
                parent.Nodes.RemoveAt(index);
                parent.Nodes.Insert(index - 1, node);
            }
        }
        else if (node.TreeControl.Nodes.Contains(node)) //root node
        {
            int index = view.Nodes.IndexOf(node);
            if (index > 0)
            {
                view.Nodes.RemoveAt(index);
                view.Nodes.Insert(index - 1, node);
            }
        }
    }

    public static void MoveDown(this Node node)
    {
        Node parent = node.Parent;
        AdvTree view = node.TreeControl;
        if (parent != null)
        {
            int index = parent.Nodes.IndexOf(node);
            if (index < parent.Nodes.Count - 1)
            {
                parent.Nodes.RemoveAt(index);
                parent.Nodes.Insert(index + 1, node);
            }
        }
        else if (view != null && view.Nodes.Contains(node)) //root node
        {
            int index = view.Nodes.IndexOf(node);
            if (index < view.Nodes.Count - 1)
            {
                view.Nodes.RemoveAt(index);
                view.Nodes.Insert(index + 1, node);
            }
        }
    }
}