using UnityEngine;

public class NormalDebris : Debris
{
    protected override EDebrisType DefineType() => EDebrisType.Normal;

    protected override void HandleDestruction()
    {
        Release();
    }
}
