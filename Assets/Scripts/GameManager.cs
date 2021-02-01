using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] CylinderController cylinderController;
    [SerializeField] LevelManager levelManager;
    [SerializeField] SkinsManager skinsManager;

    [SerializeField] Camera mainCamera;
    [SerializeField] public GameObject knife;
    [SerializeField] GameObject[] spawnedKnives;

    [SerializeField] bool spawnTimeDone = true;
    [SerializeField] bool stopKnifeSpawn = false;
    [SerializeField] int delayBeforeNextLevel = 2;
    [SerializeField] int spawnedKnivesCount = 0;

    [SerializeField] Vector2Int customResIfScreenIsHorizontal = new Vector2Int(454, 782);
    [SerializeField] Vector2Int customResIfScreenIsVertical = new Vector2Int(246, 440);

    void Start()
    {
        SetShaderResolution();
        ShowMainWindow();
    }
    void ShowMainWindow()
    {
        uiManager.ShowWindow(UiWindowName.MainWindow);
    }
    void SetShaderResolution()
    {
        if (Screen.width / Screen.height > 0)
        {
            mainCamera.GetComponent<PSXEffects>().customRes = customResIfScreenIsHorizontal;
        }
        else
        {
            mainCamera.GetComponent<PSXEffects>().customRes = customResIfScreenIsVertical;
        }
    }
    void OnApplicationQuit()
    {
        levelManager.ResetLevelValue();
    }

    public void SpawnKnife()
    {
        if (spawnedKnivesCount < levelManager.currentLevelModel.OnLevelKnifeCount)
        {
            if (spawnTimeDone && !stopKnifeSpawn)
            {
                spawnTimeDone = false;
                GameObject newKnife = Instantiate(knife);
                spawnedKnives[spawnedKnivesCount] = newKnife;
                spawnedKnives[spawnedKnivesCount].GetComponent<Rigidbody>().isKinematic = true;
                spawnedKnives[spawnedKnivesCount].GetComponent<KnifeController>().ResetKnife();
                spawnedKnives[spawnedKnivesCount].GetComponent<KnifeController>().FirstSpawn();

                StartCoroutine(SpawnTimer());
            }
        }
    }
    public void PushKnife()
    {
        spawnedKnives[spawnedKnivesCount].GetComponent<KnifeController>().Move();
        spawnedKnivesCount++;
        uiManager.MakeUiKnives(levelManager.currentLevelModel.OnLevelKnifeCount - spawnedKnivesCount);
    }
    public void WinCheck()
    {
        if (levelManager.currentLevelModel.OnLevelKnifeCount == spawnedKnivesCount)
        {
            StartCoroutine(WinVibrations());
            StartCoroutine(NextLevel());
        }
    }
    public void EndGame()
    {
        Vibration.Vibrate(500);

        levelManager.currentLevelModel.IsBossLevel = false;
        levelManager.currentLevelModel.IsCylinderRotating = false;
        audioManager.StopCylinderSound();
        uiManager.ClearUiKnives();
        //uiManager.ResetUiLevelValue();
        uiManager.ShowWindow(UiWindowName.LooseWindow);

        spawnedKnivesCount = 0;
    }
    public IEnumerator WinVibrations()
    {
        yield return new WaitForSeconds(0.25f);
        Vibration.Vibrate(100);
        yield return new WaitForSeconds(0.25f);
        Vibration.Vibrate(100);
        yield return new WaitForSeconds(0.25f);
    }
    public void BasicMethodsWhenChangingLevel()
    {
        cylinderController.ResetCylinder();//Возвращаем цилиндр в стартовую позицию
        ClearKnifeArray();//Очищаем заспавненные ножи
        cylinderController.ClearCylinderObjects();//Очищаем объекты цилиндра
        cylinderController.SpawnCylinderObjects();//Генерируем объекты на окружности цилиндра: ножи и яблоки 
        skinsManager.SetCurrentCylinderSkin();
        skinsManager.SetSkyboxMaterial();
        audioManager.ChangeCylinderSound(levelManager.currentLevelModel);
        uiManager.MakeUiKnives(levelManager.currentLevelModel.OnLevelKnifeCount);
        uiManager.UpdateUiApplesCount();
        uiManager.UpdateUiLevelValue();
    }
    public void StartFromZero()
    {
        levelManager.SetZeroLevel();//Начинаем игру с нулевого уровня
        uiManager.ShowWindow(UiWindowName.LevelWindow);//Показываем UI уровня
        BasicMethodsWhenChangingLevel();
        spawnedKnives = new GameObject[levelManager.currentLevelModel.OnLevelKnifeCount];
        spawnedKnivesCount = 0;
        SpawnKnife();//Спавним первый нож
    }
    public IEnumerator NextLevel()
    {
        spawnedKnivesCount = 0;//Обнуляем количество заспавненных ножей
        stopKnifeSpawn = true;//Делаем спавн новых ножей невозможным

        levelManager.currentLevelModel.IsBossLevel = false;
        uiManager.ClearUiKnives();
        levelManager.currentLevelModel.IsCylinderRotating = false;
        ScatterKnives();//Разбрасываем ножи
        cylinderController.ScatterCylinderObjects();
        cylinderController.DropCylinder();//Роняем цилиднр
        audioManager.PlayDropCylinderSound();

        yield return new WaitForSeconds(delayBeforeNextLevel);//Задержка перед началом следующего уровня

        
        levelManager.NextLevel();
        uiManager.UpdateUiLevelValue();
        BasicMethodsWhenChangingLevel();

        stopKnifeSpawn = false;
        SpawnKnife();
    }
    public void ScatterKnives()
    {
        foreach (GameObject knife in spawnedKnives)
        {
            cylinderController.ScatterObject(knife);
        }
    }
    public void ClearKnifeArray()
    {
        stopKnifeSpawn = false;

        foreach (GameObject knife in spawnedKnives)
        {
            Destroy(knife);
        }
    }
    IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(levelManager.currentLevelModel.SpawnKnifePause);
        spawnTimeDone = true;
    }
}