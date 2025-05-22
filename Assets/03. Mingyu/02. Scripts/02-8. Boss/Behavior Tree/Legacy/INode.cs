using UnityEngine;

public interface INode
{
    public enum ENodeState
    { Running, Success, Failed }

    public ENodeState Evaluate();
}
