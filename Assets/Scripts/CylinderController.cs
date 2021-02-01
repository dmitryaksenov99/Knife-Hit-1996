using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class CylinderController : MonoBehaviour
{
    [SerializeField] public GameObject[] cylinderObjects;
    [SerializeField] public GameObject[] spawnedCylinderObjects;
    [SerializeField] Vector3 cylinderPos;
    [SerializeField] Quaternion cylinderRot;
    [SerializeField] float tempSpeedRotation;
    [SerializeField] float currentTimeCylinderSpeedChange;
    [SerializeField] float currentTimeCylinderRotationSpeedChange;
    [SerializeField] float explosionForse = 100f;
    [SerializeField] Texture2D cylinderDefaultSkin;
    [SerializeField] Texture2D cylinderBossSkin;
    [SerializeField] float cylinderJumpAmplitude = 6f;

    GameManager gameManager;
    LevelManager levelManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();

        cylinderPos = transform.position;
        cylinderRot = transform.rotation;

        StartCoroutine(ChangeCylRotSpeed());
    }

    void Update()
    {
        CylinderRotation();
        CylinderJumping();
    }
    void CylinderRotation()
    {
        if (levelManager.currentLevelModel.IsCylinderRotating)
        {
            tempSpeedRotation = Mathf.Lerp(tempSpeedRotation, currentTimeCylinderRotationSpeedChange, levelManager.currentLevelModel.SmoothTimeCylinderRotationChange * Time.deltaTime);
            levelManager.currentLevelModel.SmoothTimeCylinderRotationChange = 0.7f;
        }
        else
        {
            tempSpeedRotation = Mathf.Lerp(tempSpeedRotation, 0, levelManager.currentLevelModel.SmoothTimeCylinderRotationChange * Time.deltaTime);
            levelManager.currentLevelModel.SmoothTimeCylinderRotationChange = 10f;
        }

        transform.Rotate(new Vector3(0, 0, tempSpeedRotation * Time.deltaTime), Space.World);
    }

    void CylinderJumping()
    {
        if (levelManager.currentLevelModel.IsBossLevel)
        {
            float distance = Mathf.Sin(Time.timeSinceLevelLoad * levelManager.currentLevelModel.CylinderJumpSpeed) / cylinderJumpAmplitude;
            transform.position = new Vector3(0, distance + 2.12f, 0);
        }
    }
    IEnumerator ChangeCylRotSpeed()
    {
        while (true)
        {
            currentTimeCylinderSpeedChange = Random.Range(levelManager.currentLevelModel.TimeChangingSpeedRotationCylinderMin, levelManager.currentLevelModel.TimeChangingSpeedRotationCylinderMax);
            currentTimeCylinderRotationSpeedChange = Random.Range(levelManager.currentLevelModel.CylinderRotationSpeedMin, levelManager.currentLevelModel.CylinderRotationSpeedMax);
            yield return new WaitForSeconds(currentTimeCylinderSpeedChange);
        }
    }
    public void ClearCylinderObjects()
    {
        foreach (GameObject cylinderObject in spawnedCylinderObjects)
        {
            Destroy(cylinderObject);
        }
    }
    public void SpawnCylinderObjects()
    {
        int maxKnivesCount = Random.Range(1, 4);
        int maxCylinderObjectsCount = Random.Range(1, 7);
        int totalMaxCount = maxKnivesCount + maxCylinderObjectsCount;

        List<int> angles = GetRandomAnglesForSpawn(totalMaxCount);

        spawnedCylinderObjects = new GameObject[angles.Count];

        float radius = 0;

        for (int i = 0; i < angles.Count; i++)
        {
            Vector3 knifePos;
            GameObject _obj = null;

            if (i <= maxKnivesCount - 1)
            {
                _obj = Instantiate(gameManager.knife);

                spawnedCylinderObjects[i] = _obj;
                spawnedCylinderObjects[i].GetComponent<Rigidbody>().isKinematic = true;
                spawnedCylinderObjects[i].GetComponent<KnifeController>().enabled = false;

                radius = spawnedCylinderObjects[i].GetComponent<KnifeController>().spawnKnifeY;
            }
            else
            {
                GameObject randomOrgan = cylinderObjects[Random.Range(0, cylinderObjects.Length)];

                float randomValue = Random.Range(float.MinValue, float.MaxValue);
                float currentObjSpawnProbability = randomOrgan.GetComponent<CylinderObjectController>().appleData.SpawnProbability;

                if (randomValue > currentObjSpawnProbability)
                {
                    return;
                }

                _obj = Instantiate(randomOrgan);
                spawnedCylinderObjects[i] = _obj;
                radius = spawnedCylinderObjects[i].GetComponent<CylinderObjectController>().spawnAppleY;
            }

            knifePos.x = Mathf.Cos(angles[i] * Mathf.PI / 180) * radius;
            knifePos.y = Mathf.Sin(angles[i] * Mathf.PI / 180) * radius;
            knifePos.z = 0;
            knifePos += transform.position;

            spawnedCylinderObjects[i].transform.position = knifePos;
            spawnedCylinderObjects[i].transform.eulerAngles = new Vector3(0, 0, 90 + angles[i]);
            spawnedCylinderObjects[i].transform.parent = transform;
        }
    }
    public void ResetCylinder()//Вернуть цилиндр в стартовую позицию
    {
        transform.position = cylinderPos;
        transform.rotation = cylinderRot;

        GetComponent<Rigidbody>().isKinematic = true;
    }
    public void ScatterCylinderObjects()
    {
        foreach (GameObject cylinderObject in spawnedCylinderObjects)
        {
            ScatterObject(cylinderObject);
        }
    }
    public void ScatterObject(GameObject knife)
    {
        try
        {
            float minPosition = 0;
            float maxPosition = 1000;
            float rndDirection = Random.Range(minPosition, maxPosition);

            Vector3 rndV3 = new Vector3(rndDirection, rndDirection, rndDirection);

            knife.transform.parent = null;
            knife.GetComponent<Rigidbody>().isKinematic = false;
            knife.GetComponent<Rigidbody>().AddExplosionForce(explosionForse, transform.position, 100, 0.1f, ForceMode.Force);
            knife.GetComponent<Rigidbody>().AddRelativeTorque(rndV3);
        }
        catch
        {

        }
    }
    public void DropCylinder()//Цилиндр падает вниз
    {
        float explosionForse = 300f;
        float minTqPower = 0;
        float maxTqPower = 1000;

        float rndDirection = Random.Range(minTqPower, maxTqPower);

        Vector3 tqPower = new Vector3(rndDirection, rndDirection, rndDirection);

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddRelativeTorque(tqPower);
        GetComponent<Rigidbody>().AddExplosionForce(
            explosionForse,
            transform.position,
            100,
            0,
            ForceMode.Force);

    }
    List<int> GetRandomAnglesForSpawn(int count)
    {
        List<int> spawnAngles = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int angle;

            for (; ; )
            {
                angle = Random.Range(0, 360) / 25 * 25;

                if (spawnAngles.Count == 0)
                {
                    break;
                }

                if (spawnAngles.Contains(angle))
                {
                    continue;
                }

                break;
            }

            spawnAngles.Add(angle);
        }

        return spawnAngles;
    }
}