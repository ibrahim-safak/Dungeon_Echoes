using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTrigger : MonoBehaviour, IInteractable
{
    [Header("Connections")]
    public RM5 connectedRoom;

    [Header("Key Spawn Settings")]
    public GameObject keyPrefab;
    public Transform spawnPoint;

    private bool hasTriggered = false;

    public void Interact()
    {
        if (hasTriggered || connectedRoom.isFogDisabled) return;

        connectedRoom.isFogDisabled = true;

        if (keyPrefab != null && spawnPoint != null)
        {
            Instantiate(keyPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        hasTriggered = true;
        Debug.Log("butona basýldý, sis kaldýrýldý ve anahtar spawnlandý!");
    }
}