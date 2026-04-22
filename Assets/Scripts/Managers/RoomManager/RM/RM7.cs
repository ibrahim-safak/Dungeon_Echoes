using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RM7 : MonoBehaviour
{
    [Header("Kapý Ayarlarý")]
    public List<Door> doorsToLock;

    [Header("Düþman Prefablarý")]
    public GameObject zombiePrefab;
    public GameObject goblinPrefab;

    [Header("Özel Spawn Noktalarý")]
    public Transform zombieSpawnPoint;    
    public Transform goblinSpawnPoint1;   
    public Transform goblinSpawnPoint2;   

    [Header("Dalga Ayarlarý")]
    public int waveCount = 3;             
    public float timeBetweenWaves = 5f;   

    private int enemiesRemaining;
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

        enemiesRemaining = waveCount * 3;

        Debug.Log("Oda Aktif! Toplam Düþman: " + enemiesRemaining);

        foreach (Door door in doorsToLock)
        {
            if (door != null)
            {
                door.isLocked = true;
                door.CloseDoor();
            }
        }

        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        for (int i = 0; i < waveCount; i++)
        {
            Debug.Log("Dalga " + (i + 1) + " geliyor!");

            Instantiate(zombiePrefab, zombieSpawnPoint.position, zombieSpawnPoint.rotation);

            Instantiate(goblinPrefab, goblinSpawnPoint1.position, goblinSpawnPoint1.rotation);

            Instantiate(goblinPrefab, goblinSpawnPoint2.position, goblinSpawnPoint2.rotation);

            if (i < waveCount - 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }

    public void EnemyDied()
    {
        if (!isRoomActive) return;

        enemiesRemaining--;
        Debug.Log("Düþman öldü, kalan toplam: " + enemiesRemaining);

        if (enemiesRemaining <= 0)
        {
            EndRoomEvent();
        }
    }

    void EndRoomEvent()
    {
        isRoomActive = false;
        StopAllCoroutines();

        totalEntries++;

        ;

        foreach (Door door in doorsToLock)
        {
            if (door != null)
            {
                door.isLocked = false;
            }
        }

        Debug.Log("Oda temizlendi!");
    }
}