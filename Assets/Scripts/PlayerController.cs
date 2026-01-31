using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    float verticalMov;
    float horizontalMov;
    public float fixedScanCooldown;
    public float scanRadiusAppearanceTime;
    private float scanCooldown = 0;
    private Rigidbody RB;
    private GameManager gameManager;
    public GameObject scanRadius;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        RB = GetComponent < Rigidbody>();
    }
    private void Update()
    {
        if (scanCooldown > 0) {
            scanCooldown -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space) && scanCooldown <= 0)
        {
            scanRadius.gameObject.SetActive(true);
            Invoke("DisableScan", scanRadiusAppearanceTime);
            scanCooldown = fixedScanCooldown;
        }
    }
    void FixedUpdate()
    {
        verticalMov = Input.GetAxisRaw("Vertical");
        horizontalMov = Input.GetAxisRaw("Horizontal");
        RB.AddForce(horizontalMov * MoveSpeed, 0, verticalMov * MoveSpeed);

        RB.transform.rotation = Quaternion.LookRotation(RB.linearVelocity, transform.up);
    }
    private void DisableScan()
    {
        scanRadius.gameObject.SetActive(false);
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
