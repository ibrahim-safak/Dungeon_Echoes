using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PhysicalMovement : MonoBehaviour
{
    private Animator animator;
    [Header("Fizik Ayarları")]
    [SerializeField] public float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer; 

    private Rigidbody rb;
    private bool isGrounded;
    private float groundCheckDistance = 1.1f; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CheckGround();
        MoveCharacter();
    }

    public void MoveCharacter()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            move += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += transform.right;
        }

        if (move.sqrMagnitude > 0f)
        {
            // Normalize ile çapraz hareket hızını sabit tutuyoruz
            Vector3 displacement = move.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + displacement);
        }
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
