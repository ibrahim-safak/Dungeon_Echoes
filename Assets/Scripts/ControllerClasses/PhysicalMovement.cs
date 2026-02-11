using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PhysicalMovement : MonoBehaviour
{
    [Header("Fizik Ayarlarý")]
    [SerializeField] public float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer; 

    private Rigidbody rb;
    private bool isGrounded;
    private float groundCheckDistance = 1.1f; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    public void MoveCharacter(Vector3 direction)
    {
        Vector3 targetVelocity = direction * moveSpeed;

        targetVelocity.y = rb.velocity.y;

        rb.velocity = targetVelocity;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }
}
