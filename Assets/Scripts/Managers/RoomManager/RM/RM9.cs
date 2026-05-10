using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RM9 : MonoBehaviour
{
    [Header("Kapı Ayarları")]
    public List<Door> doorsToLock;

    [Header("Boss Ayarları")]
    public GameObject bossPrefab;      
    public Transform bossSpawnPoint;  

    private bool isRoomActive = false;
    private GameObject activeBoss;     

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !isRoomActive)
        {
            StartRoomEvent();
        }
    }

    void StartRoomEvent()
    {
        isRoomActive = true;

        
        foreach (Door door in doorsToLock)
        {
            if (door != null)
            {
                door.isLocked = true;
                door.CloseDoor();
            }
        }

        
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            activeBoss = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);

           
            
        }
    }

    public void BossKilled()
    {
        Debug.Log("Boss yenildi! Kapılar açılıyor.");

        foreach (Door door in doorsToLock)
        {
            if (door != null)
            {
                door.isLocked = false;
            }
        }

        isRoomActive = false;
        Destroy(gameObject, 1f);
    }
}