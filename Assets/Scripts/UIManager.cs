using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] public Image imagePrefab;
    [SerializeField] public Image[] uiKnifeImages;
    [SerializeField] public GameObject uiKnivesGM;
    [SerializeField] public GameObject backToMainMenuButton;
    [SerializeField] public Sprite knifeIcon;
    [SerializeField] public Text textApplesCount;
    [SerializeField] public Text textLevelNumber;
    [SerializeField] public Text textMaxLevel;
    [SerializeField] public List<UiWindow> uiWins;

    LevelManager levelManager;
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        SetMaxLevelValueToText();
    }
    void SetMaxLevelValueToText()
    {
        textMaxLevel.text = "Max level is " + levelManager.GetMaxLevel();
    }
    public void ShowWindow(UiWindowName name)
    {
        foreach (UiWindow uiWin in uiWins)
        {
            try
            {
                if (uiWin.Name == name)
                {
                    uiWin.Window.SetActive(true);
                }
                else
                {
                    uiWin.Window.SetActive(false);
                }

                if (name == UiWindowName.MainWindow)
                {
                    SetMaxLevelValueToText();
                    backToMainMenuButton.gameObject.SetActive(false);
                }
                else
                {
                    backToMainMenuButton.gameObject.SetActive(true);
                }            
            }
            catch
            {

            }
        }
    }
    public void OpenMainWindow()
    {
        ShowWindow(UiWindowName.MainWindow);
    }
    public void OpenSkinsWindow()
    {
        ShowWindow(UiWindowName.SkinsWindow);
    }
    public void MakeUiKnives(int knifesCount)
    {
        uiKnifeImages = new Image[knifesCount];
        for (int i = 0; i < uiKnifeImages.Length; i++)
        {
            uiKnifeImages[i] = Instantiate(imagePrefab);
            uiKnifeImages[i].sprite = knifeIcon;
            uiKnifeImages[i].transform.parent = uiKnivesGM.transform;
        }
        uiKnivesGM.transform.position = new Vector3(0, uiKnifeImages.Length * imagePrefab.rectTransform.rect.height, 0);
    }
    public void ClearUiKnives()
    {
        foreach (var uiKnifeImage in uiKnifeImages)
        {
            Destroy(uiKnifeImage);
        }
    }
    public void UpdateUiApplesCount()
    {
        textApplesCount.text = "Apples: " + PlayerPrefs.GetInt("applesCount", 0);
    }
    public void UpdateUiLevelValue()
    {
        textLevelNumber.text = "Level " + PlayerPrefs.GetInt("levelNumber", 0);
    }
}
[Serializable]
public struct UiWindow
{
    public UiWindowName Name;
    public GameObject Window;
}

public enum UiWindowName
{
    MainWindow,
    LevelWindow,
    LooseWindow,
    SkinsWindow
}