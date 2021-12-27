using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateColour", menuName = "CreateColour/NewColour", order = 1)]
public class Colour : ScriptableObject
{
    public Color color;
    public string colorName;
    [HideInInspector] public bool alreadyAvaliable = false;
}
