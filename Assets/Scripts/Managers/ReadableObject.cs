using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadableObject : MonoBehaviour, IInteractable
{
    [Header("Ayarlar")]
    public Transform examinePoint; // Kameranýn önündeki boþ obje (Slot)
    public float moveSpeed = 8f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isReading = false;

    private Behaviour playerMovement;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void Interact(GameObject player)
    {
        if (playerMovement == null && player != null)
        {
            playerMovement = player.GetComponent<Behaviour>();
        }

        if (!isReading)
            StartReading();
        else
            StopReading();
    }

    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Interact(player);
    }

    private void Update()
    {
        if (isReading)
        {
            if (examinePoint != null)
            {
                transform.position = Vector3.Lerp(transform.position, examinePoint.position, Time.deltaTime * moveSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, examinePoint.rotation, Time.deltaTime * moveSpeed);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * moveSpeed);
        }
    }

    private void StartReading()
    {
        isReading = true;
        if (playerMovement != null) playerMovement.enabled = false;

        Debug.Log("Okuma baþladý, hareket kilitlendi.");
    }

    private void StopReading()
    {
        isReading = false;
        if (playerMovement != null) playerMovement.enabled = true;

        Debug.Log("Okuma bitti, hareket açýldý.");
    }
}
