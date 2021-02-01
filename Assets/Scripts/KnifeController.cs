using UnityEngine;

public class KnifeController : MonoBehaviour
{
    [SerializeField] public GameObject knifeSkin;
    [SerializeField] float smoothTime = 0.05f;
    [SerializeField] float currentVelocity = 0.0f;
    [SerializeField] float explosionForse = 100f;

    [SerializeField] public bool isMove = false;
    [SerializeField] bool isFirst = false;
    [SerializeField] Quaternion startAngels;
    [SerializeField] public float spawnKnifeY = 0.666f;

    [SerializeField] float smoothOffset = 0.05f;
    [SerializeField] float zeroY = 0.0f;

    [SerializeField] bool touchedKnife = false;

    GameManager gameManager;
    CylinderController cylinderController;
    SkinsManager skinsManager;
    AudioManager audioManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        cylinderController = FindObjectOfType<CylinderController>();
        skinsManager = FindObjectOfType<SkinsManager>();
        audioManager = FindObjectOfType<AudioManager>();

        SetCurrentKnifeSkin();
        ResetKnife();

        transform.position = new Vector3(transform.position.x, -(cylinderController.transform.position.y - spawnKnifeY), transform.position.z);
        startAngels = transform.rotation;
    }
    void SetCurrentKnifeSkin()
    {
        KnifeSkin kn = skinsManager.GetCurrentKnifeSkin();
        knifeSkin.GetComponent<Renderer>().material.SetTexture("_MainTex", kn.Texture2D);
    }

    void Update()
    {
        if (isFirst)
        {
            float newPosition = Mathf.SmoothDamp(transform.position.y, smoothOffset, ref currentVelocity, smoothTime);
            transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);

            if (transform.position.y > zeroY)
            {
                isFirst = false;
                transform.position = new Vector3(transform.position.x, zeroY, transform.position.z);
            }
        }

        if (isMove)
        {
            float newPosition = Mathf.SmoothDamp(transform.position.y, cylinderController.transform.position.y - spawnKnifeY + smoothOffset, ref currentVelocity, smoothTime);
            transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);

            
            if (transform.position.y >= cylinderController.transform.position.y - spawnKnifeY - smoothOffset)
            {
                transform.position = new Vector3(transform.position.x, cylinderController.transform.position.y - spawnKnifeY, transform.position.z);
                ConnectToCylinder();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<KnifeController>() == null)
        {
            return;
        }

        if (isMove)
        {
            touchedKnife = true;
            gameObject.transform.parent = null;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            audioManager.PlayKnifeClangSound();
            gameManager.EndGame();
        }
    }
    public void ConnectToCylinder()
    {
        isMove = false;

        if (touchedKnife)
        {
            return;
        }

        audioManager.PlayKnifeHitSound();
        gameManager.WinCheck();
        transform.parent = cylinderController.transform;
        
        Vibration.Vibrate(100);
    }
    public void ResetKnife()
    {
        gameObject.transform.parent = null;
        transform.position = Vector3.zero;
        transform.rotation = startAngels;
        isMove = false;
    }
    public void FirstSpawn()
    {
        isMove = false;
        isFirst = true;
    }
    public void Move()
    {
        isMove = true;
        isFirst = false;
    }
}
