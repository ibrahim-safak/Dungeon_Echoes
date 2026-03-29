using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Ayarlar")]
    public bool isLocked = false;
    public string requiredKeyName; 
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
                Interact(other.transform);
            }
        }
    }

    private void Interact(Transform playerTransform)
    {
        if (isLocked) 
        {
            Debug.Log("Kapı kilitli!");
            return;
        }

        if (!isOpen)
        {
            OpenDoor(playerTransform);
            
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