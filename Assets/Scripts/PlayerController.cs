using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    float verticalMov;
    float horizontalMov;
    public float scanRadiusAppearanceTime;
    private float scanStreght = 1;
    private Rigidbody RB;
    private GameManager gameManager;
    public GameObject scanRadius;
    public Material scanMaterial;
    private float scanTimer = 0;
    bool charging = false;
    bool chargingLocked = false;
    void Start()
    {
        scanMaterial = scanRadius.GetComponent<Renderer>().material;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        RB = GetComponent < Rigidbody>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !chargingLocked)
        {
            charging = true;
        }
            if (Input.GetKey(KeyCode.Space) && charging)
        {
            scanStreght += Time.deltaTime * 2;
            scanTimer += Time.deltaTime;
            Vector3 scanVector = new Vector3(scanStreght, 1, scanStreght);
            scanRadius.gameObject.transform.localScale = scanVector;
            if(scanTimer >= 3)
            {
                charging = false;
                scanTimer = 0;
            }
        }
        else if (scanStreght > 1)
        {
            scanMaterial.color = Color.pink;
            scanTimer = 0;
            charging = false;
            scanStreght = 1;
            Invoke("resetScanColor", scanRadiusAppearanceTime);
            chargingLocked = true;
            scanRadius.GetComponent<Collider>().enabled = true;
        }
    }
    private void resetScanColor()
    {
        scanMaterial.color = Color.gray;
        scanRadius.gameObject.transform.localScale = Vector3.one;
        chargingLocked = false;
        scanRadius.GetComponent<Collider>().enabled = false;

    }
    void FixedUpdate()
    {
        verticalMov = Input.GetAxisRaw("Vertical");
        horizontalMov = Input.GetAxisRaw("Horizontal");
        RB.AddForce(horizontalMov * MoveSpeed, 0, verticalMov * MoveSpeed);

        RB.transform.rotation = Quaternion.LookRotation(RB.linearVelocity, transform.up);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameManager.gameOver();
        }
        else if (collision.gameObject.CompareTag("King"))
        {
            gameManager.gameWon();
        }
    }

}
