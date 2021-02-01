using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinsGrid : MonoBehaviour
{
    public GameObject prefab;

    SkinsManager skinsManager;
    void Start()
    {
        skinsManager = FindObjectOfType<SkinsManager>().GetComponent<SkinsManager>();
        CreatePrefabs();
    }

    void CreatePrefabs()
    {
        List<KnifeSkin> userKnifeSkins = skinsManager.GetUserKnifeSkins();
        GameObject newObj;

        foreach (var item in userKnifeSkins)
        {
            newObj = Instantiate(prefab, transform);
            newObj.GetComponent<Image>().sprite = item.Icon;
            newObj.GetComponent<GridRow>().knifeSkin = item;
        }
    }
}
