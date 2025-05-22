using System.Collections.Generic;
using UnityEngine;

public sealed class SelectorNode : INode
{
    List<INode> _childs;

    public SelectorNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.ENodeState Evaluate()
    {
        if (_childs == null)
        {
            return INode.ENodeState.Failed;
        }

        foreach (var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.ENodeState.Running:
                    return INode.ENodeState.Running;
                case INode.ENodeState.Success:
                    return INode.ENodeState.Success;
            }
        }
        return INode.ENodeState.Failed;
    }
}
