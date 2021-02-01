using UnityEngine;

public class CylinderObjectController : MonoBehaviour
{
    [SerializeField] public float spawnAppleY = 0.555f;
    private UIManager uiManager;
    public CylinderObjectData appleData;
    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<KnifeController>() == null)
        {
            return;
        }

        Destroy(gameObject);
        UpdateApplesCount();
        uiManager.UpdateUiApplesCount();
    }
    public void UpdateApplesCount()
    {
        int lc = PlayerPrefs.GetInt("applesCount");
        lc++;
        PlayerPrefs.SetInt("applesCount", lc);
    }
}

