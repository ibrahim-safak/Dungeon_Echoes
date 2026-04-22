using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Ayarlar")]
    public bool isLocked = false;
    public string KeyId; // Kilitli kapılar için gerekli anahtar bilgisi
    public KeyCode interactKey = KeyCode.E;
    public float autoCloseDelay = 3f;

    [Header("Referanslar")]
    public Animator doorAnimator;

    private bool isOpen = false;
    private Coroutine closeCoroutine;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(interactKey))
            {
                
                Interact(other.gameObject);
            }
        }
    }

    private void Interact(GameObject player)
    {
        // 1. ADIM: Kapı kilitli mi kontrol et?
        if (isLocked)
        {
            InventoryManager inventory = player.GetComponent<InventoryManager>();

            if (inventory != null && inventory.HasKey(KeyId))
            {
                Debug.Log("Doğru anahtar kullanıldı, kilit açıldı!");
                isLocked = false;
                
            }
            else
            {
                Debug.Log("Kapı kilitli ve doğru anahtarın yok!");
                return;
            }
        }

        // 3. ADIM: Eğer kilitli değilse (veya kilit yeni açıldıysa) açma/kapama mantığı
        if (!isOpen)
        {
            OpenDoor(player.transform);

            if (closeCoroutine != null) StopCoroutine(closeCoroutine);
            closeCoroutine = StartCoroutine(CloseDoorAfterDelay());
        }
        else
        {
            CloseDoor();
        }
    }

    public void OpenDoor(Transform player)
    {
        // Mevcut yön algılama mantığın
        float distanceToPlayerB = Vector3.Distance(transform.GetChild(1).position, player.position);
        float distanceToPlayerF = Vector3.Distance(transform.GetChild(2).position, player.position);

        if (distanceToPlayerB < distanceToPlayerF)
        {
            doorAnimator.SetFloat("OpenDirection", -1f);
        }
        else
        {
            doorAnimator.SetFloat("OpenDirection", 1f);
        }

        isOpen = true;
    }

    public void CloseDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetFloat("OpenDirection", 0f);
        }

        isOpen = false;

        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
            closeCoroutine = null;
        }
    }

    IEnumerator CloseDoorAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseDelay);

        if (isOpen)
        {
            CloseDoor();
        }
    }
}