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
    public AudioSource scanSound;

    public Animator animator;
    public GameObject scanArc;
    public GameObject scanArcFlash;
    public float arcCooldownTime = 1f;
    float time = 0f;
    bool scanning = false;
    private MaterialPropertyBlock mpb;
    public Renderer rend;
    void Start()
    {
        mpb = new MaterialPropertyBlock();
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && arcCooldownTime <= 0)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 direction = hit.point - transform.position;
                float angle = Mathf.Atan2(direction.x, -direction.z) * Mathf.Rad2Deg;
                scanArc.transform.rotation = Quaternion.Euler(0, -angle, 0);
            }
            scanSound.Play();
            time = 0f;
            scanning = true;
            scanArc.SetActive(true);
            Invoke("disableScanArc", 0.5f);
            arcCooldownTime = 1f;
        }
        if (scanning)
        {

            scanerMovingEffect();
        }
        if (RB.linearVelocity.magnitude >= 0.1f)
        {
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
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
    private void scanerMovingEffect()
    {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / 0.5f);
        float value = Mathf.Lerp(1f, 0f, time);
        rend.GetPropertyBlock(mpb);
        mpb.SetFloat("_Float", value);
        rend.SetPropertyBlock(mpb);

        if (time >= 0.5f)
            scanning = false;
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
