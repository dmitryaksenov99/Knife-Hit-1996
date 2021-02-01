using UnityEngine;

public class GridRow : MonoBehaviour
{
    [SerializeField] public KnifeSkin knifeSkin;
    public void SetCurrentKnifeSkin()
    {
        PlayerPrefs.SetString("currentKnifeSkin", knifeSkin.Name);
    }
}
