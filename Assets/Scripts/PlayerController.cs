using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    float verticalMov;
    float horizontalMov;
    private Rigidbody RB;

    void Start()
    {
        RB = GetComponent < Rigidbody>();
    }
    void FixedUpdate()
    {
        verticalMov = Input.GetAxisRaw("Vertical");
        horizontalMov = Input.GetAxisRaw("Horizontal");
        RB.AddForce(horizontalMov * MoveSpeed, 0, verticalMov * MoveSpeed);

        RB.transform.rotation = Quaternion.LookRotation(RB.linearVelocity, transform.up);
    }
}
