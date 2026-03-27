using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDoor : MonoBehaviour 
{
    public Vector3 openPositionOffset; // Kapý açýlýnca nereye gitsin? (Örn: X: 3)
    public float speed = 2f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = false;

    void Start()
    {
        closedPos = transform.localPosition;
        openPos = closedPos + openPositionOffset;
    }

    void Update()
    {
        // Kapýyý hedef konuma yumuþakça kaydýr
        Vector3 target = isOpen ? openPos : closedPos;
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}

