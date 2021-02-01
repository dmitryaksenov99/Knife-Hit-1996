using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New KnifeSkin", menuName = "Knife Skin", order = 52)]
public class KnifeSkin : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    public Texture2D Texture2D;
}