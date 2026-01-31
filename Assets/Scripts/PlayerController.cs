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
    private Camera cam;


    public GameObject scanArc;
    public float arcCooldownTime = 1f;

    void Start()
    {
        cam = Camera.main;
        scanArc = transform.parent.GetChild(0).gameObject;
        scanMaterial = scanRadius.GetComponent<Renderer>().material;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        RB = GetComponent < Rigidbody>();
    }

    private void Update()
    {
        arcCooldownTime -= Time.deltaTime;
        scanArc.transform.position = transform.position - Vector3.up * 0.4f;
        if (Input.GetKeyDown(KeyCode.E) && arcCooldownTime <= 0)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 direction = hit.point - transform.position;
                float angle = Mathf.Atan2(-direction.x, direction.z) * Mathf.Rad2Deg;
                scanArc.transform.rotation = Quaternion.Euler(-90, -angle, 0);
            }

            scanArc.SetActive(true);
            Invoke("disableScanArc", 0.5f);
            arcCooldownTime = 1f;
        }


        if (Input.GetKeyDown(KeyCode.Space) && !chargingLocked)
        {
            charging = true;
            scanRadius.GetComponent<MeshRenderer>().enabled = true;
        }
            if (Input.GetKey(KeyCode.Space) && charging)
        {
            scanStreght += Time.deltaTime * 7;
            scanTimer += Time.deltaTime;
            Vector3 scanVector = new Vector3(scanStreght, 1, scanStreght);
            scanRadius.gameObject.transform.localScale = scanVector;
            if(scanTimer >= 2)
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
    private void disableScanArc()
    {
        scanArc.SetActive(false);

    }
    private void resetScanColor()
    {
        scanRadius.GetComponent<MeshRenderer>().enabled = false;

        scanMaterial.color = Color.gray;
        scanRadius.gameObject.transform.localScale = Vector3.one;
        chargingLocked = false;
        scanRadius.GetComponent<Collider>().enabled = false;

    }
    void FixedUpdate()
    {
        if(GameManager.controlLock) return;
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
