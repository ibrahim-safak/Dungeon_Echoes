using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretTrigger : MonoBehaviour , IInteractable
{
    [Header("Ayarlar")]
    public GameObject targetDoor; // Açýlacak olan kapý objesi
    public bool isOneTimeUse = false; // Sadece bir kez mi çalýþsýn?

    private bool hasTriggered = false;

    public void Interact()
    {
        if (isOneTimeUse && hasTriggered) return;

        // Kapýdaki "SecretDoor" scriptini bul ve çalýþtýr
        SecretDoor doorScript = targetDoor.GetComponent<SecretDoor>();

        if (doorScript != null)
        {
            Debug.Log("Gizli mekanizma tetiklendi!");
            doorScript.ToggleDoor(); 
            hasTriggered = true;

            // Opsiyonel: Tetikleyiciye (kitap gibi) bir hareket ver
            transform.localPosition += new Vector3(0, 0, -0.1f);
            Debug.Log("Gizli mekanizma çalýþtý!");
        }
    }
}
