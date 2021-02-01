using System;
using UnityEngine;
[Serializable]
public class LevelManager : MonoBehaviour
{
    [SerializeField] public LevelModel currentLevelModel;

    static LevelModel basicLevel = new LevelModel
    {
        LevelNumber = 0,
        IsBossLevel = false,
        //Время смены направления вращения цилиндра
        TimeChangingSpeedRotationCylinderMin = 1.0f,//Минимальное
        TimeChangingSpeedRotationCylinderMax = 3.0f,//Максимальное

        SmoothTimeCylinderRotationChange = 0.7f,
        CylinderRotationSpeedMin = -500,
        CylinderRotationSpeedMax = 500,
        IsCylinderRotating = true,
        ///
        OnLevelKnifeCount = 5,
        SpawnKnifePause = 0,
        CylinderJumpSpeed = 0,
    };
    public void SetZeroLevel()
    {
        ResetLevelValue();
        currentLevelModel = basicLevel;
    }
    void CheckAndSetMaxLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("levelNumber");
        int maxLevel = PlayerPrefs.GetInt("maxLevel", 0);

        if (currentLevel > maxLevel)
        {
            PlayerPrefs.SetInt("maxLevel", currentLevel);
        }
    }
    public void ResetLevelValue()
    {
        PlayerPrefs.SetInt("levelNumber", 0);
    }
    public int GetMaxLevel()
    {
        int maxLevel = PlayerPrefs.GetInt("maxLevel", 0);
        return maxLevel;
    }
    public void NextLevel()
    {
        CheckAndSetMaxLevel();
        LevelModel nextLevel = currentLevelModel;

        nextLevel.LevelNumber++;
        nextLevel.IsCylinderRotating = true;
        nextLevel.CylinderSound = SoundType.Disabled;
        nextLevel.IsBossLevel = false;

        PlayerPrefs.SetInt("levelNumber", nextLevel.LevelNumber);

        if (nextLevel.LevelNumber % 3 == 0)
        {
            nextLevel.IsBossLevel = true;
            nextLevel.CylinderSound = SoundType.SlowBreath;
            nextLevel.CylinderJumpSpeed = 7.5f;
        }
        if (nextLevel.LevelNumber % 6 == 0)
        {
            nextLevel.IsBossLevel = true;
            nextLevel.CylinderSound = SoundType.FastBreath;
            nextLevel.CylinderJumpSpeed = 12f;
        }
        if (nextLevel.LevelNumber % 9 == 0)
        {
            nextLevel.IsBossLevel = true;
            nextLevel.CylinderSound = SoundType.Scream;
            nextLevel.CylinderJumpSpeed = 40f;
        }

        if (nextLevel.CylinderRotationSpeedMin > -1500)
            nextLevel.CylinderRotationSpeedMin *= 1.01f;

        if (nextLevel.CylinderRotationSpeedMax < 1500)
            nextLevel.CylinderRotationSpeedMax *= 1.01f;

        if (nextLevel.TimeChangingSpeedRotationCylinderMin > 0.5f)
            nextLevel.TimeChangingSpeedRotationCylinderMin -= 0.01f;

        if (nextLevel.TimeChangingSpeedRotationCylinderMax > 1f)
            nextLevel.TimeChangingSpeedRotationCylinderMax -= 0.01f;

        currentLevelModel = nextLevel;
    }
}
[Serializable]
public struct LevelModel
{
    public int LevelNumber;
    public bool IsBossLevel;
    /// <summary>
    /// Настройки цилиндра
    /// </summary>

    public float TimeChangingSpeedRotationCylinderMin;
    public float TimeChangingSpeedRotationCylinderMax;
    public float SmoothTimeCylinderRotationChange;

    public float CylinderRotationSpeedMin;
    public float CylinderRotationSpeedMax;
    public bool IsCylinderRotating;
    /// <summary>
    /// Настройки спавна ножей
    /// </summary>
    [Range(5, 10)]
    public int OnLevelKnifeCount;
    public float SpawnKnifePause;
    /// <summary>
    /// 
    /// </summary>
    public SoundType CylinderSound;
    public float CylinderJumpSpeed;
}
public enum SoundType
{
    Disabled,
    SlowBreath,
    FastBreath,
    Scream
}