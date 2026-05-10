using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Animator animator;
    private PhysicalMovement physicalMovement;
    private CameraController cameraController;
    private Warrior warrior;
    private Archer archer;
    private Sorcerer sorcerer;
    private PlayerCharacter playerCharacter;


    public float VerticalKey;
    public float HorizontalKey;
    
    private float currentVerticalAim = 0f;
    public float AimSensitivity = 1f;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        physicalMovement = GetComponent<PhysicalMovement>();
        cameraController = GetComponent<CameraController>();
        warrior = GetComponent<Warrior>();
        archer = GetComponent<Archer>();
        sorcerer = GetComponent<Sorcerer>();
        playerCharacter = GetComponent<PlayerCharacter>();

    }

    private void Update()
    {
        
        float xValue = Input.GetAxis("Horizontal");
        float zValue = Input.GetAxis("Vertical");
         animator.SetFloat("HorizontalKey", xValue);
         animator.SetFloat("VerticalKey", zValue);



        float Horizontal = Input.GetAxis("Mouse X");
        float Vertical = Input.GetAxis("Mouse Y");
        cameraController.RotateCamera(new Vector2(Horizontal, Vertical));


        if (warrior != null)
        {
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
            

            if (Input.GetMouseButtonDown(1))
            {
                animator.SetBool("isCharge", true);
            }
            if (Input.GetMouseButton(1))
            {
                animator.SetBool("isWait", true);

                float my = Input.GetAxis("Mouse Y");

                
                currentVerticalAim += my * AimSensitivity * Time.deltaTime;

                currentVerticalAim = Mathf.Clamp(currentVerticalAim, -1f, 1f);

                animator.SetFloat("VerticalAim", currentVerticalAim);

            }

            if (Input.GetMouseButtonUp(1))
            {
                animator.SetBool("isWait", false);
                animator.SetTrigger("Attack");
                animator.SetBool("isCharge", false);
                archer.Attack();
            }

            

            if (Input.GetMouseButtonDown(0))
            {
                archer.SpecialAbility();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                archer.UltimateAbility();
            }
        }

        if (sorcerer != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                sorcerer.Attack();
            }
            if (Input.GetMouseButtonDown(1))
            {
                sorcerer.SpecialAbility();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                sorcerer.UltimateAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            physicalMovement.Jump();

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerCharacter?.Interact();
        }
    }
}

