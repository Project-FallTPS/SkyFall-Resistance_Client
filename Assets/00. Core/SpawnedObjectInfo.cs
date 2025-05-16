using UnityEngine;
using System;


[Serializable]
public struct SpawnedObjectInfo<EObjectType> where EObjectType : Enum
{
    public EObjectType ObjectType;
    public int Probability;
    public SpawnedObjectInfo(EObjectType objectType, int probability)
    {
        ObjectType = objectType;
        Probability = probability;
    }
}
