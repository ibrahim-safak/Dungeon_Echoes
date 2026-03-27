using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Ayarlar")]
    public bool isLocked = false;       // Kapý kilitli mi?
    public string requiredKeyName;      // Gerekli anahtarýn ID'si

    [Header("Referanslar")]
    public Animator doorAnimator;       // Kapý animasyonu için

    private bool isOpen = false;

    // Oyuncu Collider'a girdiði an bu fonksiyon çalýþýr
    private void OnTriggerEnter(Collider other)
    {
        // Gelen objenin "Player" olduðundan emin olalým
        if (other.CompareTag("Player"))
        {
           /* if (isLocked)
            {
                // Oyuncunun üzerindeki envanter sistemini kontrol et
                Inventory playerInv = other.GetComponent<Inventory>();

                if (playerInv != null && playerInv.HasKey(requiredKeyName))
                {
                    OpenDoor();
                }
                else
                {
                    Debug.Log("Kilitli! Anahtar lazým: " + requiredKeyName);
                }
            }*/
            if (!isLocked)
            {
                OpenDoor();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpening", true);
        }
               
    }
    public void CloseDoor()
    {
        isOpen = false;
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpening", false);
        }
        
    }
}