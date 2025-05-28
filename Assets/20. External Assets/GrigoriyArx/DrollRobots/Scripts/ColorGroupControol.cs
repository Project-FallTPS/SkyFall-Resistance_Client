using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [ExecuteInEditMode]
public class ColorGroupControol : MonoBehaviour
{
    public Material[] RobMats;
    public Color MainColor;
    public Color PaintedPartsColor;
    public Color Decal1Color;
    public Color Decal2Color;
    public Color EmissionColor;


    void Update()
    {
        for (int i = 0; i < RobMats.Length; i++)
        {
           RobMats[i].SetColor("_Color", MainColor);
           RobMats[i].SetColor("_PaintedPartsColor", PaintedPartsColor);
           RobMats[i].SetColor("_Decals1Color", Decal1Color);
           RobMats[i].SetColor("_Decals2Color", Decal2Color);
           RobMats[i].SetColor("_EmissionColor", EmissionColor);
        }

        Debug.Log("Color changed");
    }
}
