using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerInput : MonoBehaviour
{
    private Animator animator;
    private PhysicalMovement physicalMovement;
    private CameraController cameraController;
    private Warrior warrior;
    private Archer archer;

    public bool Walk = false;
    public bool WalkingBackward = false;
    public bool WalkLeft = false;
    public bool WalkRight = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        physicalMovement = GetComponent<PhysicalMovement>();
        cameraController = GetComponent<CameraController>();
            warrior = GetComponent<Warrior>();
            archer = GetComponent<Archer>();

    }

    private void Update()
    {
        Walk = Input.GetKey(KeyCode.W);
        WalkingBackward = Input.GetKey(KeyCode.S);
        WalkLeft = Input.GetKey(KeyCode.A);
        WalkRight = Input.GetKey(KeyCode.D);

        // Null-safe animator çaðrýlarý
        animator?.SetBool("Walk", Walk);
        animator?.SetBool("WalkingBackward", WalkingBackward);
        animator?.SetBool("WalkLeft", WalkLeft);
        animator?.SetBool("WalkRight", WalkRight);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        cameraController.RotateCamera(new Vector2(mouseX, mouseY));


        if(warrior !=null ) { 
            if (Input.GetMouseButtonDown(0))
            {
                warrior.Attack();
            }
            if (Input.GetMouseButtonDown(1))
            {
                warrior.SpecialAbility();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                warrior.UltimateAbility();
            }
        }
        if (archer != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                archer.Attack();
            }
            if (Input.GetMouseButtonDown(1))
            {
                archer.SpecialAbility();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                archer.UltimateAbility();
            }
        }


        }
}

