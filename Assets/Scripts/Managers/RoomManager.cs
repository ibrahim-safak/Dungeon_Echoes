using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Kapýlar")]
    public List<Door> doorsToLock;

    [Header("Zombi Ayarlarý")]
    public GameObject zombiePrefab;
    public List<Transform> spawnPoints;
    public int baseZombieCount = 5;
    public int extraZombiesPerEntry = 3;

    private int zombiesRemaining;
    private bool isRoomActive = false;
    private int totalEntries = 0; 

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

        
        zombiesRemaining = baseZombieCount + (totalEntries * extraZombiesPerEntry);

        Debug.Log("Oda Aktif! Zombi Sayýsý: " + zombiesRemaining);

        
        foreach (Door door in doorsToLock)
        {
            if (door != null)
            {
                door.isLocked = true;
                door.CloseDoor();
            }
        }

        StartCoroutine(SpawnZombieWave(zombiesRemaining));
    }

    IEnumerator SpawnZombieWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(2f); 

            int randomIndex = Random.Range(0, spawnPoints.Count);
            Instantiate(zombiePrefab, spawnPoints[randomIndex].position, spawnPoints[randomIndex].rotation);
        }
    }


    public void ZombieDied()
    {
        if (!isRoomActive) return;

        zombiesRemaining--;
        Debug.Log("Zombi öldü, kalan: " + zombiesRemaining);

        if (zombiesRemaining <= 0)
        {
            EndRoomEvent();
        }
    }

    void EndRoomEvent()
    {
        isRoomActive = false;

        StopAllCoroutines();

        totalEntries++;

        foreach (Door door in doorsToLock)
        {
            if (door != null)
            {
                door.isLocked = false;
            }
        }

        Debug.Log("Oda temizlendi! Bir sonraki giriþte +" + extraZombiesPerEntry + " zombi gelecek.");
    }
}