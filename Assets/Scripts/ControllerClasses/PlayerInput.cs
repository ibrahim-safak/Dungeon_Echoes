using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private PhysicalMovement physicalMovement;
    private CameraController cameraController;

    private void Awake()
    {
        physicalMovement = GetComponent<PhysicalMovement>();
        cameraController = GetComponent<CameraController>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal"); 
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.right * x + transform.forward * z).normalized;

        physicalMovement.MoveCharacter(moveDirection);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        cameraController.RotateCamera(new Vector2(mouseX, mouseY));

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            physicalMovement.Jump();
        }
    }
}

