using UnityEngine;

[CreateAssetMenu(fileName = "New AppleData", menuName = "Apple Data", order = 51)]
public class CylinderObjectData : ScriptableObject
{
    [SerializeField] public Sprite Icon;
    [SerializeField] public string Name;
    [SerializeField] public float SpawnProbability;
}
