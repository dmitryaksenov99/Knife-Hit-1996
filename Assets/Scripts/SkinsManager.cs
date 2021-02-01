using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    [SerializeField] KnifeSkin[] allKnifeSkins;
    [SerializeField] Texture2D cylinderBossSkin;
    [SerializeField] Texture2D cylinderDefaultSkin;
    [SerializeField] Material skyboxMaterial;
    [SerializeField] Texture2D skyboxDefaultTexture;
    [SerializeField] Texture2D skyboxBossTexture;

    LevelManager levelManager;
    CylinderController cylinderController;
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        cylinderController = FindObjectOfType<CylinderController>();
    }
    public KnifeSkin GetCurrentKnifeSkin()
    {
        string ks = PlayerPrefs.GetString("currentKnifeSkin", allKnifeSkins[0].Name);
        return allKnifeSkins.First(x => x.Name == ks);
    }
    public List<KnifeSkin> GetUserKnifeSkins()
    {
        List<KnifeSkin> userSkins = new List<KnifeSkin>();

        foreach (var knifeSkin in allKnifeSkins)
        {
            bool isOpened = bool.TryParse(PlayerPrefs.GetString(knifeSkin.Name, false.ToString()), out _);

            if (isOpened)
            {
                userSkins.Add(knifeSkin);
            }
        }

        return userSkins;
    }
    public void SetCurrentCylinderSkin()
    {
        if (levelManager.currentLevelModel.IsBossLevel)
        {
            cylinderController.GetComponent<Renderer>().material.SetTexture("_MainTex", cylinderBossSkin);
        }
        else
        {
            cylinderController.GetComponent<Renderer>().material.SetTexture("_MainTex", cylinderDefaultSkin);
        }
    }
    public void SetSkyboxMaterial()
    {
        if (levelManager.currentLevelModel.IsBossLevel)
        {
            skyboxMaterial.SetTexture("_FrontTex", skyboxBossTexture);
        }
        else
        {
            skyboxMaterial.SetTexture("_FrontTex", skyboxDefaultTexture);
        }
    }
}

