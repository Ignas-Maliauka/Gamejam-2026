using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    float verticalMov;
    float horizontalMov;
    private Rigidbody RB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RB = GetComponent < Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        verticalMov = Input.GetAxisRaw("Vertical");
        horizontalMov = Input.GetAxisRaw("Horizontal");
        Debug.Log(verticalMov);
        RB.AddForce(horizontalMov * MoveSpeed, 0, verticalMov * MoveSpeed);

        RB.transform.rotation = Quaternion.LookRotation(RB.linearVelocity, transform.up);
    }
}
