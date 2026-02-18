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
        if(Input.GetKey(KeyCode.W))
        {
            rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.S))
        {
            rb.MovePosition(transform.position - transform.forward * moveSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.A))
        {
            rb.MovePosition(transform.position - transform.right * moveSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D))
        {
            rb.MovePosition(transform.position + transform.right * moveSpeed * Time.deltaTime);
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
